using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerScript : MonoBehaviour
{
    private GameObject inventory;
    private bool invOpen;
    public float movementSpeed;
    public float sensitivity = 10f;
    public float maxYAngle = 80f;
    private Vector2 currentRotation;
    Vector3 gravity;
    private CharacterController cc;
    public Transform groundCheck;
    private bool isGrounded;
    public LayerMask groundMask;
    public float jumpForce;
    public float Gravity = -9.81f;
    public ÏnventoryScript inv;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        inv = FindObjectOfType<ÏnventoryScript>();
        cc = GetComponent<CharacterController>();
        inventory = GameObject.FindGameObjectWithTag("Inventory");
    }
    void Update()
    {
        ToggleInv();
        if (!invOpen)
        {
            Movement();
            Rotation();
            Interact();
        }
    }
    private void Interact()
    {
        if (Input.GetKey(KeyCode.F))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 15f) && hit.transform.tag == "Container")
            {
                if (hit.transform.GetComponent<ContainerScript>() != null)
                {
                    hit.transform.GetComponent<ContainerScript>().RayCastOpenContainer();
                    invOpen = !invOpen;
                }
            }
        }
    }
    private void Movement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundMask);
        if (isGrounded && gravity.y < 0)
            gravity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 velocity = transform.right * x + transform.forward * z;
        cc.Move(velocity * movementSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            gravity.y = Mathf.Sqrt(jumpForce * -2f * Gravity);
        }

        gravity.y += Gravity * Time.deltaTime;
        cc.Move(gravity * Time.deltaTime);
    }
    private void ToggleInv()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            invOpen = !invOpen;
            if (!invOpen)
            {
                inv.CloseContainer();
            }
        }
        if (!invOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            inventory.GetComponent<Canvas>().enabled = false;
        }
        else if (invOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            inventory.GetComponent<Canvas>().enabled = true;
        }
    }
    private void Rotation()
    {
        currentRotation.x += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
        currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);

        Camera.main.transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
        transform.rotation = Quaternion.Euler(0, currentRotation.x, 0);
    }
}