using UnityEngine;


[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Potion")]
public class Potion :  Item
{
    public Recipe craftingRecipe;
}