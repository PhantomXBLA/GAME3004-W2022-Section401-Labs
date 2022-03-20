using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public CharacterController controller;
    public float gravity = -30f;
    public float speed = 10f;
    public float jumpHeight = 3f;
    public Vector3 velocity;

    public Transform groundCheck;
    public float groundRadius = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded;

    public UIController uiController;


    public GameObject onScreenControls;
    public Joystick leftJoystick;

    public GameObject miniMap;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);

        if (isGrounded && velocity.y < 0.0f)
        {
            velocity.y = -2.0f;
        }

        float x = Input.GetAxis("Horizontal") + leftJoystick.Horizontal;
        float z = Input.GetAxis("Vertical") + leftJoystick.Vertical;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButton("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        // apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hazard"))
        {
            uiController.TakeDamage(5);
        }
    }

    public void OnJumpButtonPressed()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }
    }

    public void OnMapButtonPressed()
    {
        miniMap.SetActive(!miniMap.activeInHierarchy);
    }
}

