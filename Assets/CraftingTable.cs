using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : Interactable
{
    public override void Interact()
    {
        UIManager.instance.ToggleCraftingUI();
    }
}
