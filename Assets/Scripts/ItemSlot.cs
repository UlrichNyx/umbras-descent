using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Potion potion;

    void Start()
    {
        GetComponent<Image>().sprite = potion.itemIcon;
    }
    
    public void OnSelect()
    {
        UIManager.instance.SetSelectedPotion(potion);
    }
}