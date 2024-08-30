using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDropZone : MonoBehaviour
{
    public int rocksCollected;
    private void OnTriggerEnter(Collider other)
    {
        RockDrop rock = other.gameObject.GetComponent<RockDrop>();
        if (rock != null)
        {
            RockCollected();
            Destroy(other.gameObject);
        }
    }
    private void RockCollected()
    {
        rocksCollected++;
        print(rocksCollected);
    }
}
