using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDrop : MonoBehaviour,Iinteractable
{


    public void Interact()
    {
        PlayerController.instance.HoldObject(gameObject);
    }
}
