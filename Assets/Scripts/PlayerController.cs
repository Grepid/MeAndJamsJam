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

    private bool acceptingInputs;
    private CharacterController cc;
    private Camera cam;
    Vector2 moveInput;
    Vector2 lookXY;
    public float horizontalMouse;
    public float verticalMouse;
    private Vector3 velocity;
    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundLayerMask;
    bool isGrounded;
    public float speed;
    public float speedMultiplier;
    public GameObject heldObject;
    public GameObject heldGraphic;
    private MeshRenderer heldMeshRenderer;
    private MeshFilter heldMeshFilter;
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
    }
    private void Update()
    {
        AssignVariables();
        if (!acceptingInputs)
        {
            Move();
            Rotate();
            CheckInputs();
        }
    }

    private void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
        if (Input.GetKeyDown(KeyCode.E)) Interact();
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
            cc.Move(velocity * Time.deltaTime);
        }
    }
    private void Rotate()
    {
        transform.eulerAngles = new Vector3(0, lookXY.x, 0);
        cam.transform.eulerAngles = new Vector3(lookXY.y, lookXY.x, 0);
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
    }
    private void Interact()
    {
        if(heldObject != null)
        {
            DropObject();
            return;
        }
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Physics.Raycast(ray, out RaycastHit hit);
        if (hit.collider == null) return;
        Iinteractable interact = hit.collider.GetComponent<Iinteractable>();
        if (interact == null) return;
        interact.Interact();
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
        heldGraphic.SetActive(false);
        Destroy(heldObject);
        speedMultiplier = 1f;
    }
}
