using UnityEngine.EventSystems;
using UnityEngine.UI;
using static InteriorDecoration.FUI.UISoundsHelper;

namespace InteriorDecoration.FUI
{
    class FToggle : KMonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerEnterHandler
    {
        public Toggle toggle; 

        public bool IsOn
        {
            get
            {
                return toggle.isOn;
            }
            set
            {
                toggle.isOn = value;
            }
        }
        
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            toggle = gameObject.GetComponent<Toggle>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (KInputManager.isFocused)
            {
                KInputManager.SetUserActive();
                PlaySound(UISoundHelper.Click);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (KInputManager.isFocused)
            {
                KInputManager.SetUserActive();
                PlaySound(UISoundHelper.MouseOver);
            }
        }
    }
}
