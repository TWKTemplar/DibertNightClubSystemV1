
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
        public bool IsToggleButton = false;
        public Color SelectedImageColor = Color.red;
        public bool ColorOutline = false;
        public float ButtonPressedLengthInSeconds = 0.25f;
        [Header("Ref")]
        public Image image;
        public Image imageOutline;
        public TMPro.TextMeshProUGUI text;

        [Header("Internal Read only")]
        public float buttonpressedTime;//goes to 0 and then sets the IsSelected To true
        public Color SelectedTextColor = new Color(0.5f,0.5f,0.5f);
        public bool IsSelected = false;
        // Internal
        private void Update()
        {
            if (IsSelected && !IsToggleButton)
            {
                buttonpressedTime -= Time.deltaTime;
                if(buttonpressedTime <= 0)
                {
                    IsSelected = false;
                    SetSelected(false);
                }
            }
        }
        public void SetSelected(bool isSelected)
        {
            if(isSelected && !IsToggleButton) buttonpressedTime = ButtonPressedLengthInSeconds;
            if (!isSelected) buttonpressedTime = 0;
            IsSelected = isSelected;
            ColorUI(IsSelected);
        }
        public bool GetSelected()
        {
            return IsSelected;
        }
        private void ColorUI(bool isSelected)
        {
            if (text!=null) text.color = Color.white;
            if (image != null) image.color = Color.white;
            if (imageOutline != null) imageOutline.color = Color.white;

            if (IsSelected)
            {
                if (text != null) text.color = SelectedTextColor;
                if (image != null) image.color = SelectedImageColor;
                if (imageOutline != null) imageOutline.color = SelectedImageColor;
            }
        }
        //External Ref calls from UI
        public void ResetButton() { SetSelected(false); }
        public override void Interact() 
        {
            if (!IsToggleButton)
            {
                SetSelected(true);
            }
            else
            {
                SetSelected(!GetSelected());
            }
        }
    }
}