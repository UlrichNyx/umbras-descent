using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Inventory/Recipe")]
public class Recipe : ScriptableObject
{
    public string Name;
    public string description;
    public Item[] requirements;
    public int[] amounts;
    public int ShadowEssence;
    public Item Result;
}
