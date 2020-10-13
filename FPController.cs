using JetBrains.Annotations;
using System.Collections;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]

public class FPController : MonoBehaviour
{
    public Slider playerHealthSlider;
    public Gradient gradient;
    public Image fill;
    public float movementSpeed = 7.5f;
    public float jumpForce = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public int playerHealth = 100;
    public float dashSpeed;
    public float dashTime;
    public bool isDashing;


    //We want to keep track of the xRotation so we can use clamping to keep the Y rotation from going too far. 
    float xRotation = 0f;

   public CharacterController characterController;
    [HideInInspector]
    public Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;


    [HideInInspector]
    public bool canMove = true;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
    }

    void Update()
    {
        Dash();

        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX = canMove ? movementSpeed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? movementSpeed * Input.GetAxis("Horizontal") : 0;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && canMove)
            {
                moveDirection.y = jumpForce;
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            //These vars grab the Mouse X and Y Input entries in unity, these arround for the mouses movement, as well as joysticks on common controllers.
            //Multiply by mousesensitivity to allow the speed to be modified, and Time.deltaTime so that it moves the same way reguardless of framerate. 
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            xRotation -= mouseY;
            //ensures we can never look behind outselves and much turn around
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            //This rotates the playerBody object references earlier using the Vector3.up (Y axis) along the mouseX's range of movement, rotating not only the camera but the body object
            playerBody.Rotate(Vector3.up * mouseX);
        }
       void Dash()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine(Dasher());
                isDashing = true;
            }
            else
            {
                isDashing = false;
            }
        }

    }
    IEnumerator Dasher()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            characterController.Move(moveDirection * dashSpeed * Time.deltaTime);
            

            yield return null;
          //yield return new WaitForSeconds(3);
        }
    }
    //this code is tandem with a healthbar attached to a UI object elsewhere
    public void SetPlayerHealth(int playerHealth)
    {
        playerHealthSlider.value = playerHealth;
        fill.color = gradient.Evaluate(playerHealthSlider.normalizedValue);
    }

    //if an enemy projectile comes in contact with the player collider, take damage
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyProjectile")
            playerHealth = playerHealth - 15;
            SetPlayerHealth(playerHealth);
       
        /*
        if(playerHealth <= 0)
        {
            //player death animation
            //player restarts scene
        }
        */
            
    }
}