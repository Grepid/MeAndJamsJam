using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PilotMinigame : MonoBehaviour,Iinteractable
{
    public bool isAcceptingInputs;
    public Canvas canvas;
    public GameObject ship;
    public Image shipImage;
    public List<GameObject> obstacles;
    public Image obstacleImage;
    public GameObject destination;
    public Image destinationImage;

    private float lastUseTime;
    public float useCooldown;

    // Update is called once per frame
    void Update()
    {
        if (isAcceptingInputs)
        {
            Inputs();
        }
    }

    private void Inputs()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleActive();
        }
    }

    public void Interact()
    {
        ToggleActive();
    }

    private void AdjustCamera()
    {
        Camera c = Camera.main;
        c.transform.position = canvas.transform.position + (canvas.transform.forward * 3);
        c.transform.LookAt(canvas.transform.position);
    }




    public void ToggleActive()
    {
        if (Time.time < lastUseTime + useCooldown) return;
        if(!isAcceptingInputs) AdjustCamera();
        lastUseTime = Time.time;
        isAcceptingInputs = !isAcceptingInputs;
        PlayerController.instance.acceptingInputs = !PlayerController.instance.acceptingInputs;
        PlayerController.instance.Pickaxe.SetActive(!PlayerController.instance.Pickaxe.activeSelf);
    }
}
