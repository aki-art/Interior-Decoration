using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
//🌈✨ Aki the Fabulous 🦄🌈

namespace InteriorDecoration.Buildings.GlassSculpture
{
    class Fabulousness : KMonoBehaviour
    {
        [MyCmpReq]
        private readonly KBatchedAnimController kBatchedAnimController;
        private  KBatchedAnimController effect;
        private GameObject sparkleFx;
        private Color[] colors;
        private int currentIndex = 0;
        private int nextIndex = 1;
        bool shiftColors = true;
        const float duration = 0.75f;
        float elapsedTime = 0f;

        public void ForceToFront()
        {
            effect.SetLayer((int)Grid.SceneLayer.BuildingFront);
            effect.SetSceneLayer(Grid.SceneLayer.BuildingFront);/*
            var pos = effect.transform.localPosition;
            pos.z = -0.5f;
            effect.transform.SetLocalPosition(pos);*/
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            colors = new Color[] { 
                new Color32(230, 124, 124, 255),
                new Color32(124, 230, 127, 255),
                new Color32(230, 127, 124, 255)
            };
            Activate();
        }
        public void Activate()
        {
            //this.link = new KAnimLink(building_controller, meter_controller); 
            effect = FXHelpers.CreateEffect("fab_fx_kanim", kBatchedAnimController.transform.GetPosition(), kBatchedAnimController.transform);
            effect.destroyOnAnimComplete = false;
            effect.randomiseLoopedOffset = true;
            effect.FlipX = kBatchedAnimController.FlipX;

            var pos = new Vector3(effect.FlipX ? -0.5f : 0.5f, 0, -1f);
            effect.transform.SetLocalPosition(pos);
            ForceToFront();
            effect.Play("effect", KAnim.PlayMode.Paused, 1f);


            StartCoroutine(ShiftColors());

            sparkleFx = Util.KInstantiate(EffectPrefabs.Instance.SparkleStreakFX, kBatchedAnimController.transform.GetPosition() + new Vector3(1f, 0.5f, 0.4f));
            sparkleFx.transform.SetParent(kBatchedAnimController.transform);
            sparkleFx.SetActive(true);
        }

        private KBatchedAnimController AddEffect()
        {
            var id = "FabulousFx";

            GameObject template = EntityTemplates.CreateEntity(id, id, false);

            KBatchedAnimController controller = template.AddOrGet<KBatchedAnimController>();
            controller.materialType = KAnimBatchGroup.MaterialType.Simple;
            controller.initialAnim = "";
            controller.initialMode = KAnim.PlayMode.Paused;
            controller.isMovable = true;
            controller.destroyOnAnimComplete = false;
            controller.AnimFiles = new KAnimFile[] { Assets.GetAnim("fab_fx_kanim") };

            return controller;
        }


        public void Deactivate()
        {
            StopCoroutine(ShiftColors());

            sparkleFx.SetActive(false);
            sparkleFx.DeleteObject();

            effect.enabled = false;
            effect.DeleteObject();
        }
        IEnumerator ShiftColors()
        {
            while(shiftColors)
            { 
                elapsedTime += Time.deltaTime;
                var dt = elapsedTime / duration;

                effect.TintColour = Color.Lerp(colors[currentIndex], colors[nextIndex], dt);

                if (elapsedTime >= duration)
                {
                    currentIndex = currentIndex == colors.Length - 1 ? 0 : currentIndex + 1;
                    nextIndex = nextIndex == colors.Length - 1 ? 0 : nextIndex + 1;
                    if (elapsedTime > duration) elapsedTime = 0;
                }

                yield return new WaitForSeconds(.2f);
            }
        }
    }
}
