
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
namespace CyBar
{

    public class GarnishSelector : UdonSharpBehaviour
    {
        [Header("Settings")]
        public Color SelectedColor = Color.green;
        [Header("Read Only")]
        public bool[] Selected;
        [Header("Ref")]
        public Image[] images;
        
        private void Start()
        {
            GetChildImages();
        }
        private void OnValidate()
        {
            GetChildImages();
        }
        private void GetChildImages()
        {
            images = GetComponentsInChildren<Image>();
        }
        // UI Selection 
        public void SelectMe(DrinkButton drinkButton)//This assumes that the order of images is in assending order down in unity
        {
            ColorDrinks(drinkButton.image);
            //SelectedDrink = FindDrinkID(drinkButton.image);
        }
        private void SelecDrink(int drinkint = 0)
        {
            ColorDrinks(images[drinkint]);
            //SelectedDrink = drinkint;
        }
        //Internal
        private int FindDrinkID(Image SelectedDrinkImage)
        {
            for (int i = 0; i < images.Length; i++)
            {
                if (images[i] == SelectedDrinkImage) return i;
            }
            Debug.Log("Unable to find Image ID");
            return -1;
        }
        private void ColorDrinks(Image SelectedDrinkImage)
        {
            foreach (var img in images)
            {
                img.color = Color.white;
            }
            SelectedDrinkImage.color = SelectedColor;
        }
    }
}