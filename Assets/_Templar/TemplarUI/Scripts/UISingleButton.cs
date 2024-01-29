
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;
namespace CyBar
{
    public class UISingleButton : UdonSharpBehaviour
    {

        [Header("Settings")]
        public bool UpdateColorUIOnStart = true;
        public bool FlipColors = false;
        public bool IsToggleButton = false;
        public bool ColorOutline = false;
        public float ButtonPressedLengthInSeconds = 0.25f;
        [Header("Colors")]
        public Color SelectedImageColor = Color.red;
        public Color SelectedTextColor = new Color(0.5f,0.5f,0.5f);
        [Header("Ref")]
        public Image image;
        public Image imageOutline;
        public TMPro.TextMeshProUGUI text;

        [Header("Internal Read only")]
        public bool IsSelected = false;
        public void Start()
        {
            if(UpdateColorUIOnStart) ColorUI(IsSelected);
        }
        public void NonToggleButtonReset()
        {
            SetSelected(false);
        }
        public void SetSelected(bool isSelected)
        {
            ColorUI(isSelected);
            if(IsSelected == isSelected)
            {
                
                return;
            }
            if(isSelected && !IsToggleButton) SendCustomEventDelayedSeconds("NonToggleButtonReset", ButtonPressedLengthInSeconds);
            IsSelected = isSelected;
        }
        private void ColorUI(bool isSelected)
        {
            if (FlipColors) isSelected = !isSelected;

            if (text!=null) text.color = Color.white;
            if (image != null) image.color = Color.white;
            if (imageOutline != null) imageOutline.color = Color.white;

            if (isSelected)
            {
                if (text != null) text.color = SelectedTextColor;
                if (image != null) image.color = SelectedImageColor;
                if (imageOutline != null) imageOutline.color = SelectedImageColor;
            }
        }
        //External Ref calls from UI
        public override void Interact() 
        {
            if (IsToggleButton)
            {
                SetSelected(!IsSelected);
            }
            else
            {
                SetSelected(true);
            }
        }
    }
}