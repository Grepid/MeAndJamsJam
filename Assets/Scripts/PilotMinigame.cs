using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PilotMinigame : MonoBehaviour,Iinteractable
{
    public static PilotMinigame Instance;

    private bool isReady;
    private bool isAcceptingInputs;
    private bool isPlaying;
    public Canvas canvas;
    private GameObject ship;
    public Image shipImage;
    private List<GameObject> obstacles;
    public Image obstacleImage;
    private GameObject destination;
    public Image destinationImage;
    public GameObject beginText;

    private float lastUseTime;
    public float useCooldown;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAcceptingInputs)
        {
            Inputs();
            if (isReady)
            {
                GameInputs();
            }
        }
    }

    private void Inputs()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isPlaying)
        {
            ToggleActive();
        }
    }

    private void GameInputs()
    {
        if(Input.GetMouseButton(0) && !isPlaying) StartGame();
    }

    public void Interact()
    {
        ToggleActive();
    }

    private void AdjustCamera()
    {
        Camera c = Camera.main;
        c.transform.position = canvas.transform.position + (canvas.transform.forward * -3);
        c.transform.LookAt(canvas.transform.position);
    }

    public void IsReady()
    {
        beginText.SetActive(true);
        isReady = true;
    }

    public void StartGame()
    {
        beginText.SetActive(false);
        isPlaying = true;
        //Make this the top so it goes down
        ship = MakeNew("Ship");
        destination = MakeNew("Destination");
    }
    private GameObject MakeNew(string name)
    {
        GameObject go = new GameObject(name);
        go.transform.position = canvas.transform.position;
        go.transform.rotation = canvas.transform.rotation;
        go.transform.SetParent(canvas.transform);
        go.AddComponent<CanvasRenderer>();
        go.AddComponent<Image>();
        return go;
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
