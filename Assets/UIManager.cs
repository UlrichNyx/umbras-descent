using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Slider shadowEssenceSlider;

    public void SetShadowEssenceSlider(int currentShadowEssence)
    { 
        shadowEssenceSlider.value = ((float) currentShadowEssence / (float) Stats.maxShadowEssence);
    }
}
