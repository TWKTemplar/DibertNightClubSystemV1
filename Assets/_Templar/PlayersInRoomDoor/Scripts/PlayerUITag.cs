
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
namespace Templar
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class PlayerUITag : UdonSharpBehaviour
    {
        public Text label;
        public Image BackgroundImage;
        [HideInInspector] public VRCPlayerApi vrcPlayer;
        public void _ConfigureButton(VRCPlayerApi playerInput, Color ColorBias)
        {
            vrcPlayer = playerInput;
            label.text = playerInput.displayName;
            AssignRandomColor(ColorBias);
        }
        public void AssignRandomColor(Color ColorBias)
        {
            if (BackgroundImage != null)
            {
                Color randomColor = ColorBias;

                randomColor.r = Mathf.Clamp(randomColor.r + (Random.value*0.5f)+0.5f,0,1);
                randomColor.g = Mathf.Clamp(randomColor.g + (Random.value*0.5f)+0.5f,0,1);
                randomColor.b = Mathf.Clamp(randomColor.b + (Random.value*0.5f)+0.5f,0,1);
                randomColor.a = 1;
                BackgroundImage.color = randomColor;
            }
        }
    }
}
