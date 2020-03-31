using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace InteriorDecoration.Buildings.FossilDisplay
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class FossilStand : KMonoBehaviour, IEffectDescriptor
    {
        private Reactable fossilReactable;
        private string effect;
        private const string inspired1 = "Inspired1";
        private const string inspired2 = "Inspired2";
        private const string inspired3 = "Inspired3";

        protected override void OnCleanUp()
        {
            if (fossilReactable != null)
            {
                fossilReactable.Cleanup();
                fossilReactable = null;
            }
        }

        public void CreateInspiredReactable(Artable.Status status)
        {
            switch (status)
            {
                case Artable.Status.Ugly:
                    effect = inspired1;
                    break;
                case Artable.Status.Okay:
                    effect = inspired2;
                    break;
                default:
                    effect = inspired3;
                    break;
            }
            if (fossilReactable == null)
            {
                fossilReactable = new EmoteReactable(gameObject, "Inspired", Db.Get().ChoreTypes.Emote, "anim_react_starry_eyes_kanim", 15, 8, 0, 10f, float.PositiveInfinity)
                    .AddThought(Db.Get().Thoughts.Angry);
                    /*.AddStep(new EmoteReactable.EmoteStep
                    {
                        anim = "react",
                        startcb = new Action<GameObject>(AddReactionEffect)
                    });*/
                //.AddThought(Db.Get().Thoughts.Happy);
                //.AddPrecondition(new Reactable.ReactablePrecondition(ReactorIsOnFloor)); 
            }
        }

        private void AddReactionEffect(GameObject reactor)
        {
            var effects = reactor.GetComponent<Klei.AI.Effects>();
            bool hasSmall = effects.HasEffect(inspired1);
            bool hasMedium = effects.HasEffect(inspired2);
            bool hasSuper = effects.HasEffect(inspired3);

            switch (effect)
            {
                case inspired1:
                    if (!hasMedium && !hasSuper)
                        reactor.GetComponent<Klei.AI.Effects>().Add(inspired1, true);
                    break;
                case inspired2:
                    if (hasSmall)
                        reactor.GetComponent<Klei.AI.Effects>().Remove(inspired1);
                    if (!hasSuper)
                        reactor.GetComponent<Klei.AI.Effects>().Add(inspired2, true);
                    break;
                case inspired3:
                    if (hasSmall)
                        reactor.GetComponent<Klei.AI.Effects>().Remove(inspired1);
                    if (hasMedium)
                        reactor.GetComponent<Klei.AI.Effects>().Remove(inspired2);
                    reactor.GetComponent<Klei.AI.Effects>().Add(inspired3, true);
                    break;
                default:
                    Log.Warning($"Something went wrong trying to add an Inspired Reaction effect. Effect ({effect}) is invalid.");
                    break;
            };
        }
        private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition)
        {
            return transition.end == NavType.Floor;
        }


        public List<Descriptor> GetDescriptors(BuildingDef def)
        {
            return new List<Descriptor>();
        }

    }

}

