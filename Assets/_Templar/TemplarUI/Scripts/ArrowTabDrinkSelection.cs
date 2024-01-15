
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
namespace CyBar
{
    public class ArrowTabDrinkSelection : UdonSharpBehaviour
    {
        public bool UseXAxis = true;
        public bool UseYAxis = false;
        public float SetXPos = -1000;
        public float SetYPos = 0;
        public RectTransform TargetRect;

        public override void Interact()
        {
            SetXPosition(SetXPos,SetYPos);
        }

        public void SetXPosition(float xpos, float ypos)
        {
            Vector2 anchoredPosition = TargetRect.anchoredPosition;
            if(UseXAxis) anchoredPosition.x = xpos;
            if(UseYAxis) anchoredPosition.y = ypos;
            TargetRect.anchoredPosition = anchoredPosition;
        }

    }
}







