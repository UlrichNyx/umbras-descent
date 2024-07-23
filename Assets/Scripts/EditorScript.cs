using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class EditorScript : EditorWindow
{

    [MenuItem("Helper/FindItems")]
    static void FindItems() {
        List<Item> items = new List<Item>();
        Debug.Log(Application.dataPath + "/ScriptableObjects/MonsterParts");
        Item[] objects = Resources.LoadAll<Item>("MonsterParts");
        Item[] objects2 = Resources.LoadAll<Item>("Potions");
        Debug.Log("items found " + objects.Length);
        items.AddRange(objects);
        items.AddRange(objects2);
        GameManager.instance.AllItems = items.ToArray();
    }


    [MenuItem("Helper/Debug/Print Global Position")]
    public static void PrintGlobalPosition()
    {
        if (Selection.activeGameObject != null)
        {
            Debug.Log(Selection.activeGameObject.name + " is at " + Selection.activeGameObject.transform.position);
        }
    }

    [MenuItem("Helper/Debug/Print Global Rotation")]
    public static void PrintGlobalRotation()
    {
        if (Selection.activeGameObject != null)
        {
            Debug.Log(Selection.activeGameObject.name + " is at " + Selection.activeGameObject.transform.eulerAngles);
        }
    }
}
