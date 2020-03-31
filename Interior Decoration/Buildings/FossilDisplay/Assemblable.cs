using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InteriorDecoration.Buildings.FossilDisplay
{

    /// <summary>
    /// Assemblabe is an Artable for the Fossil Stand
    /// </summary>
    public class Assemblable : Artable
    {
        /*[SerializeField]
        new public List<Stage> stages = new List<Stage>();*/

#pragma warning disable 649
        [MyCmpReq]
        private readonly FossilStand fossilStand;
#pragma warning restore 649

        private readonly Dictionary<Status, StatusItem> statuses;
        [Serialize]
        private string currentStage;

        private WorkChore<Assemblable> chore;
        private static KAnimFile[] sculptureOverrides;

        protected Assemblable()
        {
            faceTargetWhenWorking = true;
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            workerStatusItem = Db.Get().DuplicantStatusItems.Researching;
            attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
            skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
            requiredSkillPerk = Db.Get().SkillPerks.IncreaseLearningSmall.Id;
            SetWorkTime(80f);

            if (sculptureOverrides == null)
                sculptureOverrides = new KAnimFile[] { Assets.GetAnim("anim_interacts_vet_kanim") };
                //sculptureOverrides = new KAnimFile[1] { Assets.GetAnim("anim_interacts_research2_kanim") };
            overrideAnims = sculptureOverrides;
            synchronizeAnims = false;
        }


        protected override void OnSpawn()
        {
            shouldShowSkillPerkStatusItem = false;

            if (string.IsNullOrEmpty(currentStage))
                currentStage = "Default";

            SetStage(currentStage, true);

            if (currentStage == "Default")
            {
                shouldShowSkillPerkStatusItem = true;
                Prioritizable.AddRef(gameObject);
                chore = new WorkChore<Assemblable>(Db.Get().ChoreTypes.Research, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
                chore.AddPrecondition(ChorePreconditions.instance.ConsumerHasTrait, requiredSkillPerk);
            }

            base.OnSpawn();
        }

        protected override void OnCompleteWork(Worker worker)
        {
            Status scientist_skill = Status.Ugly;
            MinionResume component = worker.GetComponent<MinionResume>();

            if (component != null)
            {
                if (component.HasPerk(Db.Get().SkillPerks.AllowInterstellarResearch.Id))
                    scientist_skill = Status.Great;
                else if (component.HasPerk(Db.Get().SkillPerks.CanStudyWorldObjects.Id))
                    scientist_skill = Status.Okay;
            }

            List<Stage> potential_stages = new List<Stage>();
            potential_stages = stages.FindAll(s => s.statusItem.Equals(scientist_skill));
            potential_stages.Shuffle();
            SetStage(potential_stages[0].id, false);

            if (potential_stages[0].cheerOnComplete)
            {
                EmoteChore emoteChore1 = new EmoteChore(worker.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, "anim_cheer_kanim", new HashedString[3]
                {
                 "cheer_pre",
                 "cheer_loop",
                 "cheer_pst"
                }, null);
            }
            else
            {
                EmoteChore emoteChore2 = new EmoteChore(worker.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, "anim_disappointed_kanim", new HashedString[3]
                {
                 "disappointed_pre",
                 "disappointed_loop",
                 "disappointed_pst"
                }, null);
            }

            shouldShowSkillPerkStatusItem = false;
            UpdateStatusItem(null);
            Prioritizable.RemoveRef(gameObject);
        }

        public override void SetStage(string stage_id, bool skip_effect)
        {
            base.SetStage(stage_id, skip_effect);

            Stage stage = stages[0];
            foreach (Stage s in stages)
            {
                if (s.id == stage_id)
                {
                    stage = s;
                    break;
                }
            }

            if (skip_effect || !(CurrentStage != "Default"))
                return;

            // Smoke effect
            KBatchedAnimController effect = FXHelpers.CreateEffect("sculpture_fx_kanim", transform.GetPosition(), transform, false, Grid.SceneLayer.Front, false);
            effect.destroyOnAnimComplete = true;
            effect.transform.SetLocalPosition(new Vector3(0.5f, -0.5f));
            effect.Play("poof", KAnim.PlayMode.Once, 1f, 0.0f);

            // Create reactable
            fossilStand.CreateInspiredReactable(stage.statusItem);
        }
    }
}

