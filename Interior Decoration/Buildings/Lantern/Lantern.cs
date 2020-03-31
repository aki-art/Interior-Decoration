using STRINGS;
using System;

namespace InteriorDecoration.Buildings.Lantern
{
    class Lantern : StateMachineComponent<Lantern.StatesInstance>
    {
        [MyCmpReq]
        private Operational operational;
        [MyCmpReq]
        private ElementConverter elementConverter;
        [MyCmpReq]
        private Storage storage;

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
                string formattedMass = GameUtil.GetFormattedMass(elementConverter.consumedElements[0].massConsumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
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
                        BUILDING.STATUSITEMS.AWAITINGFUEL.NAME,
                        BUILDING.STATUSITEMS.AWAITINGFUEL.TOOLTIP,
                        string.Empty,
                        StatusItem.IconType.Exclamation,
                        NotificationType.BadMinor,
                        false,
                        new HashedString(),
                        129022,
                        AwaitingFuelResolveString, null, null)
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
