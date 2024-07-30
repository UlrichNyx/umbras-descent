using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixieCircle : MonoBehaviour
{
    public GameObject InnerCircle;
    public bool IsPlayerInside;
    public void SetInnerCircleSize(float percentValue)
    {
        InnerCircle.transform.localScale = new Vector3(percentValue,percentValue,percentValue);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponent<PlayerStats>())
        {
            IsPlayerInside = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.GetComponent<PlayerStats>())
        {
            IsPlayerInside = false;
        }
    }
}
