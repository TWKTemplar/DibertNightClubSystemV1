
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;
namespace Templar
{
    public class GeneratePlayerScrollViewer : UdonSharpBehaviour
    {
        public HidePlayerManager hidePlayerManager;
        public RectTransform buttonParent;
        public RectTransform buttonTemplate;
        public Color ColorBiasForUI = Color.white;
        public string[] DebugArray;
        public void GenerateUI(VRCPlayerApi[] playersInRoom)
        {

            //Ughhh, tODO: Compare playersInRoom to HiddenPlayers and filter out any in both x.x
            ////Sort out any Hidden players
            //VRCPlayerApi[] playersInRoomThatAreNotHidden = new VRCPlayerApi[playersInRoom.Length];
            //VRCPlayerApi[] HiddenPlayers = hidePlayerManager.GetHiddenPlayers();
            //DebugArray = new string[playersInRoom.Length];
            //int index = 0;
            //for (int j = 0; j < playersInRoom.Length; j++)
            //{
            //    for (int i = 0; i < HiddenPlayers.Length; i++)
            //    {
            //        if (playersInRoom[j] == HiddenPlayers[i])
            //        {
            //            playersInRoom[j] = null;
            //            break;
            //        }
            //        playersInRoomThatAreNotHidden[index] = playersInRoom[j];
            //        DebugArray[index] = playersInRoom[j].displayName;
            //        index++;
            //    }
            //}





            // destroy any pre-existing buttons
            for (int i = 0; i < buttonParent.childCount; i++)
            {
                RectTransform tf = (RectTransform)buttonParent.GetChild(i);
                if (tf != buttonTemplate)
                    Destroy(tf.gameObject);
            }
            // measure the dimensions of the UI
            int len = playersInRoom.Length;
            float buttonHeight = buttonTemplate.sizeDelta.y;
            buttonParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, buttonHeight * len);
            // instantiate buttons
            for (int i = 0; i < len; i++)
            {
                // instantiate and transform the button
                GameObject newButton = VRCInstantiate(buttonTemplate.gameObject);
                RectTransform newButtonTf = newButton.GetComponent<RectTransform>();
                newButtonTf.SetParent(buttonParent);
                newButtonTf.localScale = Vector3.one;
                newButtonTf.localRotation = Quaternion.identity;
                newButtonTf.anchoredPosition3D = buttonHeight * i * Vector3.down;
                newButton.SetActive(true);

                // modify the button's data
                PlayerUITag buttonProgram = newButton.GetComponent<PlayerUITag>();
                buttonProgram._ConfigureButton(playersInRoom[i], ColorBiasForUI);
            }
        }
    }
}
