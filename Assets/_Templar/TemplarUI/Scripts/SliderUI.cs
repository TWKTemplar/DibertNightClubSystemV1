
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VRC.SDKBase;
using VRC.Udon;
namespace CyBar
{
    public class SliderUI : UdonSharpBehaviour
    {
        [Header("Settings")]
        public bool LimitSigFigs = false;
        [Header("Read Only")]
        [Range(0f,1f)]public float SliderValue;
        [Header("Ref")]
        public Slider slider;
        public TextMeshProUGUI text;
        public Image imageToColor;


        private void OnValidate()
        {
            if (slider == null) slider = GetComponent<Slider>();
            SliderValue = slider.value;
            SyncUIToValue();
        }
        private void Start()
        {
            if (slider == null) slider = GetComponent<Slider>();
            SliderValue = slider.value;
            SyncUIToValue();
        }
        public override void Interact()
        {
            SliderValue = slider.value;
            SyncUIToValue();
        }
        //Internal Calls

        private void SyncUIToValue()
        {
            if(text != null)
            {
                string str = "";
                if(LimitSigFigs) str = Mathf.Clamp( Mathf.Round(SliderValue*10) , 0 , 10 ).ToString();
                else str = (Mathf.Round(SliderValue * 10) * 0.1f).ToString();
                text.text = str;
            }
                
        }
    }
    
}