using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour,Iinteractable
{
    public GameObject[] Drops;
    [SerializeField]
    private int health;
    public void Break()
    {
        //Play break Noise
        GameObject drop = Instantiate(DecideDrop(),transform.position,Quaternion.identity);
        Destroy(gameObject);
    }

    private GameObject DecideDrop()
    {
        if (Drops.Length <= 0) return new GameObject();
        return Drops[Random.Range(0, Drops.Length)];
    }

    public void Interact()
    {
        AudioManager.Play("TempMining", transform.position);
        health--;
        if(health <= 0)
        {
            Break();
        }
    }
}
