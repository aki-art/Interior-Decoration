using STRINGS;
using System;

namespace InteriorDecoration.Buildings.Lantern
{
    class Lantern : StateMachineComponent<Lantern.StatesInstance>
    {
#pragma warning disable 649
        [MyCmpReq]
        private Operational operational;
        [MyCmpReq]
        private ElementConverter elementConverter;
#pragma warning restore 649

        private static readonly Operational.Flag requiresFuelFlag = new Operational.Flag("hasFuel", Operational.Flag.Type.Requirement);


        protected override void OnSpawn()
        {
            base.OnSpawn();
            smi.StartSM();
        }
        public class States : GameStateMachine<States, StatesInstance, Lantern>
        {
            public State disabled;
            public State off;
            public State on;

            private string AwaitingFuelResolveString(string str, object obj)
            {
                ElementConverter elementConverter = ((GenericInstance)obj).master.elementConverter;
                string elementName = elementConverter.consumedElements[0].tag.ProperName();
                string formattedMass = GameUtil.GetFormattedMass(
                    mass: elementConverter.consumedElements[0].massConsumptionRate, timeSlice: GameUtil.TimeSlice.PerSecond,
                    massFormat: GameUtil.MetricMassFormat.UseThreshold, includeSuffix: true, floatFormat: "{0:0.#}");

                str = string.Format(str, elementName, formattedMass);
                return str;
            }

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = disabled;

                disabled
                  .Enter("Waiting", smi => smi.master.operational.SetFlag(requiresFuelFlag, false))
                  .EventTransition(
                      GameHashes.FunctionalChanged,
                      off,
                      smi => smi.master.operational.IsFunctional);
                off
                    .EventTransition(
                        GameHashes.FunctionalChanged,
                        disabled,
                        smi => !smi.master.operational.IsFunctional)
                    .Enter("Waiting", smi => smi.master.operational.SetFlag(requiresFuelFlag, false))
                    .ToggleStatusItem(
                        name: BUILDING.STATUSITEMS.AWAITINGFUEL.NAME,
                        tooltip: BUILDING.STATUSITEMS.AWAITINGFUEL.TOOLTIP,
                        icon: string.Empty,
                        icon_type: StatusItem.IconType.Exclamation,
                        notification_type: NotificationType.BadMinor,
                        allow_multiples: false,
                        render_overlay: new HashedString(),
                        status_overlays: 129022,
                        resolve_string_callback: AwaitingFuelResolveString, resolve_tooltip_callback: null, category: null)
                    .EventTransition(
                        GameHashes.OnStorageChange,
                        on,
                        smi => smi.master.elementConverter.HasEnoughMassToStartConverting());
                on
                    .EventTransition(
                        GameHashes.FunctionalChanged,
                        disabled,
                        smi => !smi.master.operational.IsFunctional)
                    .Enter("Running", smi => smi.master.operational.SetFlag(requiresFuelFlag, true))
                    .EventTransition(
                        GameHashes.OnStorageChange,
                        off,
                        smi => !smi.master.elementConverter.HasEnoughMassToStartConverting());
            }
        }

        public class StatesInstance : GameStateMachine<States, StatesInstance, Lantern, object>.GameInstance
        {
            public StatesInstance(Lantern master) : base(master)
            {
            }
        }
    }
}
