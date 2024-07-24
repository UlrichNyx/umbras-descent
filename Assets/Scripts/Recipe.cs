using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : ScriptableObject
{
    public Item[] requirements;
    public int[] amounts;
}
