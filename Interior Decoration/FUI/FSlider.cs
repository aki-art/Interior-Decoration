using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static InteriorDecoration.FUI.UISoundsHelper;

namespace InteriorDecoration.FUI
{
    public class FSlider : KMonoBehaviour, IEventSystemHandler, IDragHandler, IPointerDownHandler
    {
        public event System.Action OnChange;
        public event System.Action OnMaxReached;

        public Slider slider;

        private readonly float movePlayRate = 0.05f;
        private float lastMoveTime;
        private float lastMoveValue;
        private bool playedBoundaryBump;

        public delegate float MapValue(float val);
        public MapValue mapValue = x => x;
        public MapValue reverseMapValue = x => x;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            slider = gameObject.GetComponent<Slider>();
        }

        private void UpdateSlider()
        {
            OnChange?.Invoke();

            if (slider.value == slider.maxValue)
                OnMaxReached?.Invoke();
        }

        // This is a little lazy, but it works just fine
        public void OnDrag(PointerEventData eventData)
        {
            if (KInputManager.isFocused)
            {
                KInputManager.SetUserActive();
                PlayMoveSound();

                UpdateSlider();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (KInputManager.isFocused)
            {
                KInputManager.SetUserActive();
                PlaySound(UISoundHelper.SliderStart);
                OnChange?.Invoke();
            }
        }


        public float Value
        {
            get
            {
                return mapValue(slider.value);
            }
            set
            {
                slider.value = reverseMapValue(value);
            }
        }

        // Based on Ksliders sounds
        // Minor bug: the pitch is a little too high
        public void PlayMoveSound()
        {
            if (KInputManager.isFocused)
            {
                float timeSinceLast = Time.unscaledTime - lastMoveTime;
                if (!(timeSinceLast < movePlayRate))
                {
                    float inverseLerpValue = Mathf.InverseLerp(slider.minValue, slider.maxValue, slider.value);
                    string sound_path = null;
                    if (inverseLerpValue == 1f && lastMoveValue == 1f)
                    {
                        if (!playedBoundaryBump)
                        {
                            sound_path = UISoundHelper.SliderBoundaryHigh;
                            playedBoundaryBump = true;
                        }
                    }
                    else
                    {
                        if (inverseLerpValue == 0f && lastMoveValue == 0f)
                        {
                            if (!playedBoundaryBump)
                            {
                                sound_path = UISoundHelper.SliderBoundaryLow;
                                playedBoundaryBump = true;
                            }
                        }
                        else if (inverseLerpValue >= 0f && inverseLerpValue <= 1f)
                        {
                            sound_path = UISoundHelper.SliderMove;
                            playedBoundaryBump = false;
                        }
                    }
                    if (sound_path != null && sound_path.Length > 0)
                    {
                        lastMoveTime = Time.unscaledTime;
                        lastMoveValue = inverseLerpValue;
                        FMOD.Studio.EventInstance ev = KFMOD.BeginOneShot(sound_path, Vector3.zero, 1f);
                        ev.setParameterValue("sliderValue", inverseLerpValue);
                        ev.setParameterValue("timeSinceLast", timeSinceLast);
                        KFMOD.EndOneShot(ev);
                    }
                }
            }
        }
    }
}
