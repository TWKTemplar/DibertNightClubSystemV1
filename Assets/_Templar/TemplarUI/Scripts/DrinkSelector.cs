
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
namespace CyBar
{
    public class DrinkSelector : UdonSharpBehaviour
    {
        [Header("Settings")]
        public Color SelectedColor = Color.red;
        [Header("Read Only")]
        public int SelectedDrink = 0;
        [Header("Ref")]
        public Image[] images;
        private void Update()
        {
            //AnimateImage(images[SelectedDrink], TimeSinceLastDrinkSelected);
        }
        //start up
        private void Start()
        {
            GetChildImages();
            SelecDrink();
        }
        private void OnValidate()
        {
            GetChildImages();
            SelecDrink();
        }
        private void GetChildImages()
        {
            images = GetComponentsInChildren<Image>();
        }
        // UI Selection 
        public void SelectMe(DrinkButton drinkButton)//This assumes that the order of images is in assending order down in unity
        {
            ColorDrinks(drinkButton.image);
            SelectedDrink = FindDrinkID(drinkButton.image);
        }
        private void SelecDrink(int drinkint = 0)
        {
            ColorDrinks(images[drinkint]);
            SelectedDrink = drinkint;
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