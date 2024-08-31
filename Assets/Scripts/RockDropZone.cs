using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RockDropZone : MonoBehaviour
{
    public int rocksCollected;
    public TextMeshPro text;
    public int rocksToCollect;
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
        text.text = rocksCollected.ToString();
        if(rocksCollected >= rocksToCollect)
        {
            PilotMinigame.Instance.IsReady();
        }
        
    }
}
