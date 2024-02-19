using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f; 
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private float animationPlayTransition = 0.15f;

    public CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    
    private Animator animator;
    private int MoveX;
    private int MoveZ;
    private int jumpAnimation;

    private void Awake() {
        animator = GetComponent<Animator>();
        MoveX = Animator.StringToHash("MoveX");
        MoveZ = Animator.StringToHash("MoveZ");
        jumpAnimation = Animator.StringToHash("FG_Jump_Start_A");
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

        animator.SetFloat(MoveX, Input.GetAxis("Horizontal"));
        animator.SetFloat(MoveZ, Input.GetAxis("Vertical"));

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if ((Input.GetButtonDown("Jump") || Input.GetButton("Jump")) && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.CrossFade(jumpAnimation, animationPlayTransition);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}