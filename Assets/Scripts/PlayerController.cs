using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Iinteractable
{
    public void Interact();
}



public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public bool acceptingInputs;
    private CharacterController cc;
    private Camera cam;
    Vector2 moveInput;
    Vector2 lookXY;
    public float horizontalMouse;
    public float verticalMouse;
    private Vector3 velocity;
    Transform groundCheck;
    public float groundDistance;
    public LayerMask groundLayerMask;
    bool isGrounded;
    public float speed;
    public float speedMultiplier;
    public GameObject heldObject;
    public GameObject heldGraphic;
    private MeshRenderer heldMeshRenderer;
    private MeshFilter heldMeshFilter;

    public Light flashlight;
    public GameObject Pickaxe;
    private Coroutine pickaxeSwing;
    private Vector3 pickaxeRestRot;

    public float interactRange;
    public LayerMask interactLayerMask;

    [Tooltip("In seconds")]
    public float FlashlightCharge;
    public float FlashlightMaxCharge;
    Vector3 moveDirection
    {
        get
        {
            return (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;
        }
    }




    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        instance = this;
        heldGraphic.SetActive(false);
    }
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;
        groundCheck = transform.Find("GroundCheckLocation");
        heldMeshFilter = heldGraphic.GetComponent<MeshFilter>();
        heldMeshRenderer = heldGraphic.GetComponent<MeshRenderer>();
        pickaxeRestRot = Pickaxe.transform.localRotation.eulerAngles;
    }
    private void Update()
    {
        
        if (acceptingInputs)
        {
            AssignVariables();
            Move();
            Rotate();
            CheckInputs();
        }
    }

    private void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
        if (Input.GetKeyDown(KeyCode.E)) Interact();
        if (Input.GetMouseButtonDown(0)) RockInteract();
        if (Input.GetKeyDown(KeyCode.F)) ToggleFlashlight();
    }
    private void Jump()
    {
        if (isGrounded)
        {
            velocity.y = 5f;
        }
    }
    private void Move()
    {
        cc.Move(moveDirection * speed *speedMultiplier *Time.deltaTime);

        if (isGrounded && velocity.y < 0) velocity = Vector3.down * 2;
        else
        {
            velocity += Physics.gravity * Time.deltaTime;
            
        }
        cc.Move(velocity * Time.deltaTime);
    }
    private void Rotate()
    {
        transform.eulerAngles = new Vector3(0, lookXY.x, 0);
        cam.transform.eulerAngles = new Vector3(lookXY.y, lookXY.x, 0);
        cam.transform.localPosition = new Vector3(0, 0.58f, 0);
    }

    private void AssignVariables()
    {
        Vector2 look;
        look.x = Input.GetAxis("Mouse X") * horizontalMouse * Time.smoothDeltaTime;
        look.y = Input.GetAxis("Mouse Y") * -verticalMouse * Time.smoothDeltaTime;

        lookXY += look;
        lookXY.y = Mathf.Clamp(lookXY.y, -90, 90);

        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayerMask);

        if (flashlight.gameObject.activeSelf) ChargeFlashlight(-Time.deltaTime);
    }
    private void Interact()
    {
        if(heldObject != null)
        {
            DropObject();
            return;
        }
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Physics.Raycast(ray,out RaycastHit hit, interactRange,interactLayerMask,QueryTriggerInteraction.Ignore);
        if (hit.collider == null) return;
        print(hit.collider.gameObject.name);
        Iinteractable interact = hit.collider.GetComponent<Iinteractable>();
        if (interact == null) return;
        if (interact as Rock != null) return;
        interact.Interact();
    }

    private void RockInteract()
    {
        if (heldObject != null)
        {
            DropObject();
            return;
        }
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayerMask, QueryTriggerInteraction.Ignore);
        if (hit.collider == null) return;
        Iinteractable interact = hit.collider.GetComponent<Iinteractable>();
        if (interact == null) return;
        if (interact as Rock != null)
        {
            SwingPickaxe();
            interact.Interact();
        }
    }

    public void HoldObject(GameObject go)
    {
        if (heldObject != null) return;
        heldGraphic.SetActive(true);
        heldObject = go;
        MeshFilter targetFilter = go.GetComponent<MeshFilter>();
        MeshRenderer targetRender = go.GetComponent<MeshRenderer>();
        heldMeshFilter.sharedMesh = targetFilter.sharedMesh;
        heldMeshRenderer.material = targetRender.material;
        go.SetActive(false);
        speedMultiplier = 0.5f;
    }
    public void DropObject()
    {
        if (heldObject == null)
        {
            print("Held obj was null when trying to drop");
            return;
        }
        GameObject go = Instantiate(heldObject, heldGraphic.transform.position, heldGraphic.transform.rotation);
        go.SetActive(true);
        go.name = heldObject.name;
        heldGraphic.SetActive(false);
        Destroy(heldObject);
        speedMultiplier = 1f;
    }
    public void ToggleFlashlight()
    {
        if (FlashlightCharge <= 0) return;
        flashlight.gameObject.SetActive(!flashlight.gameObject.activeSelf);
    }
    public void ChargeFlashlight(float chargeAmount)
    {
        FlashlightCharge = Mathf.Clamp(FlashlightCharge + chargeAmount, 0, FlashlightMaxCharge);
        if(FlashlightCharge <= 0) flashlight.gameObject.SetActive(false);
    }
    public void SwingPickaxe()
    {
        if(pickaxeSwing != null)
        {
            StopCoroutine(pickaxeSwing);
            Pickaxe.transform.localRotation = Quaternion.Euler(pickaxeRestRot);
        }
        pickaxeSwing = StartCoroutine(SwingPick());
    }

    //The single worst bit of code I've ever wrote
    public IEnumerator SwingPick()
    {
        while(true)
        {
            float x = Pickaxe.transform.localRotation.eulerAngles.x + (500 * Time.deltaTime);
            Pickaxe.transform.localRotation = Quaternion.Euler(new Vector3(Mathf.Clamp(x, pickaxeRestRot.x, 55), pickaxeRestRot.y, pickaxeRestRot.z));
            if (Mathf.Approximately(Pickaxe.transform.localRotation.eulerAngles.x,55)) break;
            yield return null;
        }
        while (Pickaxe.transform.localRotation.eulerAngles.x != pickaxeRestRot.x)
        {
            float x = Pickaxe.transform.localRotation.eulerAngles.x + (500 * -Time.deltaTime);
            Pickaxe.transform.localRotation = Quaternion.Euler(new Vector3(Mathf.Clamp(x, pickaxeRestRot.x, 55), pickaxeRestRot.y, pickaxeRestRot.z));
            if (Pickaxe.transform.localRotation.eulerAngles.x == pickaxeRestRot.x) break;
            yield return null;
        }
    }
}
