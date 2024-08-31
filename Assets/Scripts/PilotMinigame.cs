using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private List<GameObject> obstacles = new List<GameObject>();
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

    private float moveDirection;

    private void GameInputs()
    {
        if(Input.GetMouseButton(0) && !isPlaying) StartGame();
        if (isPlaying)
        {
            OverlapChecks();
            moveDirection = Input.GetAxis("Horizontal");
        }
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
        destination.transform.position *= 1000;
        obstacles.Add(MakeNew("Enemy"));
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

    private void OverlapChecks()
    {
        if (ship == null) return;
        if (destination == null) return;

        if (IsOverlapping(ship, destination)) Win();
        foreach(GameObject obstacle in obstacles)
        {
            if (obstacle == null) continue;
            if (IsOverlapping(obstacle, ship)) Lose();
        }
    }


    private bool IsOverlapping(GameObject A,GameObject B)
    {
        var image1rect = A.GetComponent<RectTransform>().rect;
        var image2rect = B.GetComponent<RectTransform>().rect;

        var image1rt = A.GetComponent<RectTransform>();
        var image2rt = B.GetComponent<RectTransform>();

        if (image1rt.localPosition.x < image2rt.localPosition.x + image2rect.width &&
            image1rt.localPosition.x + image1rect.width > image2rt.localPosition.x &&
            image1rt.localPosition.y < image2rt.localPosition.y + image2rect.height &&
            image1rt.localPosition.y + image1rect.height > image2rt.localPosition.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Win()
    {
        print("Won");
    }
    private void Lose()
    {
        print("Lost");
        gameObject.AddComponent<PilotMinigame>();
        Destroy(this);
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
