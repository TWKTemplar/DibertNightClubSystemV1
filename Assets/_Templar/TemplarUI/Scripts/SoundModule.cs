
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
namespace CyBar
{
    public class SoundModule : UdonSharpBehaviour
    {
        public AudioSource[] sounds;
        int i = 0;

        public override void Interact()
        {
            sounds[i].Play();
            i++;
            if (i == sounds.Length)
            {
                i = 0;
            }
        }
    }
}