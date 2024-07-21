using UnityEngine;


[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Potion")]
public class Potion :  ScriptableObject
{
    public Item item;
    public Recipe craftingRecipe;
}