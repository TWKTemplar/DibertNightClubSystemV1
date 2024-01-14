
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerSelectionBuilderMenuSensor : UdonSharpBehaviour
{
    [Header("Player Teleport Ref")]
    public PlayerSelectionBuilder playerSelectionBuilder;
    [Header("Update Ref")]
    public HandMenu handMenu;
    public bool IsMenuOpen = false; 
    void Start()
    {
        if (handMenu == null) handMenu.GetComponent<HandMenu>();
    }

    public void MenuOpened()
    {
        playerSelectionBuilder._Refresh();
        playerSelectionBuilder._ShowUI();
    }
    public void MenuClosed()
    {
        playerSelectionBuilder._HideUI();
    }
    public void Update()
    {
        if(handMenu.isMenuOpen != IsMenuOpen)
        {
            IsMenuOpen = handMenu.isMenuOpen;
            if (IsMenuOpen) MenuOpened();
            if (!IsMenuOpen) MenuClosed();
        }
    }

}
