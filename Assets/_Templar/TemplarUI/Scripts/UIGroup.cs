
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;
namespace CyBar
{
    public class UIGroup : UdonSharpBehaviour
    {

        [Header("Settings")]
        public Color SelectedColor = Color.red;
        public bool ColorOutline = false;
        public bool ForceOneToBeSelected = false;
        [Header("Ref")]
        public Image[] images;
        public Image[] imagesOutline;
        public TMPro.TextMeshProUGUI[] texts;
        [Header("Internal Read only")]
        public int Selected = -1;
        // Internal
        private void OnValidate()
        {
            SetSelected(-1);
        }
        private void SetSelected(int selected)
        {
            if(selected == -1)
            {
                if (ForceOneToBeSelected) Selected = 0;
                else Selected = -1;
            }
            else if(selected == Selected)
            {
                if (ForceOneToBeSelected) Selected = selected;
                else Selected = -1;
            }
            else
            {
                Selected = selected;
            }
            ColorUI(Selected);
        }
        private void ColorUI(int selected)
        {
            if (texts.Length > 0) foreach (var txt in texts) txt.color = Color.white;
            if (images.Length > 0) foreach (var img in images) img.color = Color.white;
            if (imagesOutline.Length > 0) foreach (var img in imagesOutline) img.color = Color.white;
            
            if(selected != -1)
            {
                if (texts.Length > 0) texts[selected].color = SelectedColor;
                if (images.Length > 0) images[selected].color = SelectedColor;
                if (imagesOutline.Length > 0 && ColorOutline) imagesOutline[selected].color = SelectedColor;
            }
        }
        //External Ref calls from UI
        public void ResetSelection() { SetSelected(-1); }
        public void Select0() { SetSelected(0); }
        public void Select1() { SetSelected(1); }
        public void Select2() { SetSelected(2); }
        public void Select3() { SetSelected(3); }

    }
}
