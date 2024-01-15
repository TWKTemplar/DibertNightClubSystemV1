
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;
namespace CyBar
{
    public class UIMenu : UdonSharpBehaviour
    {
        private void Update()
        {
            SyncVisualData();
        }
        public void SyncVisualData()
        {
            SyncSliders();//4 float sliders 0 to 1
            SyncDrink();//int drink selector 0 to 14
            SyncIce();
            SyncMixMode();
            //SyncGarnishs();
            //SyncIsTrippy();
        }

        //4 Sliders (float[]) 0 to 1
        public float[] DrinkSliders;
        public SliderUI[] sliderUIs;
        private void SyncSliders()
        {
            for (int i = 0; i < sliderUIs.Length; i++)
            {
                DrinkSliders[i] = sliderUIs[i].SliderValue;
            }
        }
        //1 Drink selector (int)  0 to 14
        public int DrinkSelected;
        public DrinkSelector drinkSelector;
        private void SyncDrink()
        {
            DrinkSelected = drinkSelector.SelectedDrink;
        }
        //1 Ice selector (int) -1 and 0 to 3 
        public int SelectedIce;
        public UIGroup IceUIGroup;
        private void SyncIce()
        {
            SelectedIce = IceUIGroup.Selected;
        }
        //1 Mix Mode (int) 0 to 2
        public int MixMode;
        public UIGroup MixModeUIGroup;
        private void SyncMixMode()
        {
            MixMode = MixModeUIGroup.Selected;
        }
        //2 Garnishs (int[]) 0 to 
        public int[] Garnishs;
        //public GarnishScript GarnishScript; 
        //private void SyncGarnishs()

        //IsTrippy (bool) true/false
        public bool IsTrippy;
        // private void SyncIsTrippy()
    }
}
