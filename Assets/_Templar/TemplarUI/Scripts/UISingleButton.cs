
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
        public float buttonpressedTime;//goes to 0 and then sets the IsSelected To true
        public bool IsSelected = false;
        public void Start()
        {
            if(UpdateColorUIOnStart) ColorUI(IsSelected);
        }
        // Internal
        private void Update()
        {
            if (IsSelected && !IsToggleButton)
            {
                buttonpressedTime -= Time.deltaTime;
                if(buttonpressedTime <= 0)
                {
                    SetSelected(false);
                }
            }
        }
        public void SetSelected(bool isSelected)
        {
            ColorUI(isSelected);
            if(IsSelected == isSelected)
            {
                Debug.Log("Already That selection type");
                return;
            }
            if(isSelected && !IsToggleButton) buttonpressedTime = ButtonPressedLengthInSeconds;
            if (!isSelected) buttonpressedTime = 0;
            IsSelected = isSelected;
            Debug.Log("Set selected to " + isSelected);
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