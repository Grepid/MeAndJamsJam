using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDropZone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        RockDrop rock = collision.gameObject.GetComponent<RockDrop>();
        if(rock != null)
        {

        }
    }
}
