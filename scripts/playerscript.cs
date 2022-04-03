using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerscript : MonoBehaviour
{
    //player
    public CharacterController characterController;
    public Transform playerHead, firingPosition;

    //movement
    public float speed = 12.5f, sprintSpeed = 25f, jumpHieght = 1f, currentSlideTime, slideSpeed = 15f, maxSlideTime = 3f;
    public bool readytoJump, isSprinting, isSliding, isWalking, isIdle;

    //Animation
    public Animator myAnimator;

    //effects
    public GameObject bigSplash, bulletImpact, bullet;
    private GameObject muzzleFlash;
    public float particleffectLifetime = 1f;

    //physics
    public Rigidbody myRigidBody;
    public Vector3 velocity;
    public float gravityModifier;

    //camera/gun
    private float verticalCamerarotation;
    public float mouseSensitivity = 10f;
    public float weaponRange = 100f;

    //groundcheck
    public LayerMask grondMask;
    public GameObject groundcheck;

    //crouching
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 bodyScale;
    public Transform myBody;
    private float initialControllerHeight;
    public float crouchSpeed = 6f;
    public bool isCrouching = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        bodyScale = myBody.transform.localScale;
        initialControllerHeight = characterController.height;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        CameraMovement();
        jump();
        crouch();
        SlideCounter();
    }

    void crouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
            StartCrouching();

        if (Input.GetKeyUp(KeyCode.C) || currentSlideTime > maxSlideTime)
            StopCrouching();     
    }

    void jump()
    {
       readytoJump = Physics.OverlapSphere(groundcheck.transform.position, 0.5f, grondMask).Length > 0;

        if (Input.GetButtonDown("Jump") && readytoJump)
        {
            velocity.y = Mathf.Sqrt(jumpHieght * -2f * Physics.gravity.y) * Time.deltaTime;
            velocity.y = jumpHieght;
        }
        characterController.Move(velocity);
    }



    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = x * transform.right + z * transform.forward;

        if (Input.GetKey(KeyCode.W))
            isWalking = true;
        else
            isWalking = false;

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
            isSprinting = true;
        else
            isSprinting = false;

        if (isSprinting == true)
            movement = movement * sprintSpeed * Time.deltaTime;

        else if (isWalking == true)
            movement = movement * speed * Time.deltaTime;

        else if (isCrouching == true)
            movement = movement * crouchSpeed * Time.deltaTime;

        myAnimator.SetBool("IsWalking", isWalking);
        myAnimator.SetBool("isSprinting", isSprinting);
        myAnimator.SetFloat("playerSpeed", movement.magnitude);
       // Debug.Log(movement.magnitude);

        characterController.Move(movement);

        velocity.y += Physics.gravity.y * Mathf.Pow(Time.deltaTime, 2) * gravityModifier;

        characterController.Move(velocity);

        if (characterController.isGrounded)
        {
            velocity.y = Physics.gravity.y * Time.deltaTime;
        }
    }

    private void CameraMovement()
    {
        float mousex = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mousey = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        verticalCamerarotation -= mousey;
        verticalCamerarotation = Mathf.Clamp(verticalCamerarotation, -90f, 90f);

        transform.Rotate(Vector3.up * mousex);
        playerHead.localRotation = Quaternion.Euler(verticalCamerarotation, 0f, 0f); 
    }
    void StartCrouching()
    {
        myBody.transform.localScale = crouchScale;
        playerHead.position -= new Vector3(0, 0.5f, 0);
        characterController.height /= 2;
        isCrouching = true;

        if (isSprinting && currentSlideTime < maxSlideTime)
        {
            isSliding = true;
            velocity = Vector3.ProjectOnPlane(playerHead.transform.forward, Vector3.up).normalized * slideSpeed * Time.deltaTime;
        }
    }

    void StopCrouching()
    {
        isCrouching = false;
        isSliding = false;
        velocity = new Vector3(0, 0, 0);
        currentSlideTime = 0f;

        myBody.transform.localScale = bodyScale;
        playerHead.position += new Vector3(0, 0.5f, 0);
        characterController.height = initialControllerHeight;
    }

    void SlideCounter()
    {
        if (isSliding)
        {
           // Debug.Log(currentSlideTime);
            currentSlideTime += Time.deltaTime;
        }
    }
}
