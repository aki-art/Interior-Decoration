using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

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
            effect = FXHelpers.CreateEffect("fab_fx_kanim", kBatchedAnimController.transform.GetPosition(), kBatchedAnimController.transform, false, Grid.SceneLayer.Front, false);
            effect.destroyOnAnimComplete = false;
            effect.randomiseLoopedOffset = true;
            if(kBatchedAnimController.FlipX)
            {
                effect.FlipX = true;
                effect.transform.SetLocalPosition(new Vector3(-0.5f, 0, 1f));
            }
            else
                effect.transform.SetLocalPosition(new Vector3(0.5f, 0, 1f));
            effect.Play("effect", KAnim.PlayMode.Loop, 0.4f, UnityEngine.Random.Range(0.0f, 1.0f));


            StartCoroutine(ShiftColors());

            sparkleFx = Util.KInstantiate(EffectPrefabs.Instance.SparkleStreakFX, kBatchedAnimController.transform.GetPosition() + new Vector3(1f, 0.5f, 0.4f));
            sparkleFx.transform.SetParent(kBatchedAnimController.transform);
            sparkleFx.SetActive(true);
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
