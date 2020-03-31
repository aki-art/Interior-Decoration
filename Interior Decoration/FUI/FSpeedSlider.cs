using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InteriorDecoration.FUI
{

    public class FSpeedSlider : KMonoBehaviour, IEventSystemHandler
    {
        private const string SPEED_MULTIPLIER_PREFIX = "Speed bonus: ";

        private Text speedMultiplerLabel;
        private Text speedRangeLabel;

        public List<Range> ranges;

        public FSlider fSlider;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            #region  set object references
            fSlider = gameObject.AddComponent<FSlider>();
            speedMultiplerLabel = transform.Find("SliderLabel").GetComponent<Text>();
            speedRangeLabel = transform.Find("SliderRangeLabel").GetComponent<Text>();
            #endregion

            fSlider.OnChange += UpdateLabels;
            UpdateLabels();
        }

        public void AssignRanges(List<Range> rangeList)
        {
            ranges = rangeList;
            UpdateLabels();
        }

        public void SetValue(float val)
        {
            fSlider.Value = val;
            UpdateLabels();
        }

        public void UpdateLabels()
        {
            speedMultiplerLabel.text = SPEED_MULTIPLIER_PREFIX + fSlider.Value.ToString() + "x";
            UpdateRange(fSlider.Value);
        }

        private void UpdateRange(float val)
        {
            if (ranges != null && ranges.Count > 0)
            {
                int currentIndex = ranges.FindLastIndex(r => r.min <= val);
                var currentRange = ranges[currentIndex];

                if (currentRange.name != null)
                {
                    speedRangeLabel.text = currentRange.name;
                    speedRangeLabel.color = currentRange.color;
                }
            }
        }

        public struct Range
        {
            public float min;
            public string name;
            public Color color;

            public Range(float minimum, string rangeName, Color rangeColor)
            {
                min = minimum;
                name = rangeName;
                color = rangeColor;
            }
        }


    }
}
