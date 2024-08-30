using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PilotMinigame : MonoBehaviour
{
    public bool isAcceptingInputs;
    public Canvas canvas;
    public GameObject ship;
    public Image shipImage;
    public List<GameObject> obstacles;
    public Image obstacleImage;
    public GameObject destination;
    public Image destinationImage;

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

    }












    public void ToggleActive()
    {
        isAcceptingInputs = !isAcceptingInputs;
        PlayerController.instance.acceptingInputs = !PlayerController.instance.acceptingInputs;
    }
}
