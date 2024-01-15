
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;
namespace CyBar
{
    public class UITextReadout : UdonSharpBehaviour
    {
        [Header("Data Ref")]
        public UIMenu UIMenu;
        public DrinkNameLookUp drinkNameLookUp;
        [Header("Text Slots")]
        public TextMeshProUGUI[] SliderText;
        public TextMeshProUGUI DrinkText;

        private void Start()
        {
            if (UIMenu == null) Debug.LogError("Needs a UIMenu to display drink data");
        }
        public override void Interact()
        {
            SyncTextWithUIMenu();
        }
        private void OnValidate()
        {
            SyncTextWithUIMenu();
        }
        private void Update()
        {
            SyncTextWithUIMenu();
        }
        [ContextMenu("SyncTextWithUIMenu")]
        public void SyncTextWithUIMenu()
        {
            //sliders
            for (int i = 0; i < UIMenu.DrinkSliders.Length; i++)
            {
                string str = "";
                str += Mathf.Clamp(Mathf.Round(UIMenu.DrinkSliders[i] * 10), 0, 10).ToString();
                str += " ";
                str += DTN(i);
                SliderText[i].text = str;
            }
            //Drink Name
            DrinkText.text = drinkNameLookUp.GetDrinkName(UIMenu.DrinkSelected);
        }
        private string DTN(int i) //DrinkTypeName
        {
            if (i == 0) return "Liqueur";
            if (i == 1) return "Spirits";
            if (i == 2) return "Beer";
            if (i == 3) return "Wine";
            return "Error";
        }
    }
}