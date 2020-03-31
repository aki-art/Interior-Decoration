using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace InteriorDecoration.FUI
{

    public class FCycle : KMonoBehaviour
    {
        public event System.Action OnChange;

        private FButton leftArrow;
        private FButton rightArrow;

        private Text label;
        private Text description;

        private int currentIndex = 0;

        public bool showDescriptions = false;

        private bool HasOptions
        {
            get
            {
                return Options.Count > 0;
            }
        }

        public List<CycleOption> Options;

        protected override void OnPrefabInit()
        {
            #region setting object references
            leftArrow = transform.Find("LightShapeLeftButton").gameObject.AddComponent<FButton>();
            rightArrow = transform.Find("LightShapeRightButton").gameObject.AddComponent<FButton>();
            label = transform.Find("Text").GetComponent<Text>();
            description = transform.Find("Text/Note").GetComponent<Text>();
            #endregion

            leftArrow.OnClick += CycleLeft;
            rightArrow.OnClick += CycleRight;

        }
        public void CycleLeft()
        {
            if (HasOptions)
            {
                currentIndex = (currentIndex + Options.Count - 1) % Options.Count;
                UpdateLabel();
                OnChange?.Invoke();
            }
        }
        public void CycleRight()
        {
            if (HasOptions)
            {
                currentIndex = (currentIndex + 1) % Options.Count;
                UpdateLabel();
                OnChange?.Invoke();
            }
        }

        public void SetValue(string id)
        {
            currentIndex = Options.FindIndex(o => o.id == id);
            if(currentIndex == -1)
            {
                Log.Warning("Element not found. Resetting to first element.");
                currentIndex = 0;
            }
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            if (HasOptions)
            {
                label.text = Options[currentIndex].label;
                if(showDescriptions) description.text = Options[currentIndex].desc;
            }
        }

        public string GetValue()
        {
            return HasOptions ? Options[currentIndex].id : null;
        }

        public class CycleOption
        {
            public string id;
            public string label;
            public string desc;

            public CycleOption(string id, string label, string desc)
            {
                this.id = id;
                this.label = label;
                this.desc = desc;
            }
        }
    }
}
