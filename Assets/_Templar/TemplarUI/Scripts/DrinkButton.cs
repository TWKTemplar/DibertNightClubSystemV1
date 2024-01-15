
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
namespace CyBar
{ 
    public class DrinkButton : UdonSharpBehaviour
    {

        public DrinkSelector drinkSelector;
        public Image image;
        public void OnValidate()
        {
            if(drinkSelector == null) drinkSelector = gameObject.GetComponentInParent<DrinkSelector>();
            if (image == null) image = gameObject.GetComponent<Image>();
        }

        public override void Interact()
        {
            drinkSelector.SelectMe(this);
        }
    }
}