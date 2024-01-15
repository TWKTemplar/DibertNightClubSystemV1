
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DrinkNameLookUp : UdonSharpBehaviour
{
    public string[] DrinkNames;
    public string GetDrinkName(int drinkID)
    {
        return DrinkNames[drinkID];
    }
}
