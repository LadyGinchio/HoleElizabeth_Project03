using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;

    [SerializeField] AudioClip _jump;
    [SerializeField] AudioClip _laneShift;
    [SerializeField] AudioClip _slide;

    private int desiredLane = 1;
    public float laneDistance = 4;

    public bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    public float jumpForce;
    public float Gravity = -20;

    public bool isSliding;
    public float originalHeight;
    public float reducedHeight;
    
    

    public Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;
    }

    // Update is called once per frame
    void Update()
    {

        animator.SetBool("isGameStarted", true);
        direction.z = forwardSpeed;

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.15f, groundLayer);
        
        animator.SetBool("isSliding", isSliding);
        

        if (controller.isGrounded)
        {

            direction.y = -2;
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || swipeManager.swipeUp)
            {

                Jump();
                animator.SetBool("isGrounded", false);
            }

        }
        else {
            direction.y += Gravity * Time.deltaTime; 
            animator.SetBool("isGrounded",true);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || swipeManager.swipeRight) 
        {
            AudioSource audioSource = playerAudio.PlayClip2D(_laneShift, 1);
            desiredLane++;
            if (desiredLane == 3) {
                desiredLane = 2;
                
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || swipeManager.swipeLeft)
        {
            AudioSource audioSource = playerAudio.PlayClip2D(_laneShift, 1);
            desiredLane--;
            if (desiredLane == -1)
            {
                desiredLane = 0;
                
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || swipeManager.swipeDown) {
            isSliding = true;
            Slide();
            Invoke("stopSlide", 1.0f);
            
        }
     
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (desiredLane == 0) {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2) {
            targetPosition += Vector3.right * laneDistance;
        }

        transform.position = targetPosition;
        //transfom.position = Vector3.Lerp(transform.position, targetPosition, 70 * Time.deltaTime);
        controller.center = controller.center;
        /*
            if(transform.position == targetPosition)
                return;
            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
            if(moveDir.sqMagnitude < diff.sqrMagnitude)
                controller.Move(moveDir);
            else
                controller.Move(diff);
         
         */

    }

    private void FixedUpdate()
    {
        controller.Move(direction * Time.fixedDeltaTime);
    }
    private void Jump()
    {
        direction.y = jumpForce;
        AudioSource audioSource = jumpAudio.PlayClip2D(_jump, 1);
    }
    private void Slide() {
        AudioSource audioSource = playerAudio.PlayClip2D(_slide, 1);
        controller.height = reducedHeight;
        controller.center = new Vector3(controller.center.x, controller.center.y / 2, controller.center.z);

    }

    private void stopSlide() {
        isSliding = false;
        controller.height = originalHeight;
        controller.center = new Vector3(controller.center.x, controller.center.y * 2, controller.center.z);
    }

    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle") 
        {
            playerManager.gameOver = true;
        }
    }
}
