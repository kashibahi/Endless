using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;
    private int desiredLane = 1;
    public float laneDistance = 4;
    public float jumpForce;
    public float Gravity = -20;
    public Animator animator;
    public float maxSpeed;
    private bool isSliding = false;
    private bool isGrounded = false;
    public AudioSource audioSource;
    public AudioSource audioSource1;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "collect")
        {
            audioSource.Play();


        }
        if (other.gameObject.tag == "Obstacle")
        {
            audioSource1.Play();


        }
    }
    
       
    
    // Update is called once per frame
    void Update()
    {
        
        if (!PlayerManager.isGameStarted)
            return;
        if (forwardSpeed < maxSpeed)
            forwardSpeed += 0.1f * Time.deltaTime;
        animator.SetBool("isGameStarted", true);
        direction.z = forwardSpeed;
      //  isGrounded = Physics.CheckSphere(groundCheck.position, 0.15f, groundLayer);
      //  animator.SetBool("isGrounded", isGrounded);
        if(controller.isGrounded)
        {
            direction.y = -1;
            if (Input.GetKeyDown(KeyCode.UpArrow)|| SwipeManager.swipeUp|| Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
               
            }
        }
        else
        {
            direction.y += Gravity * Time.deltaTime;
        }
        if(SwipeManager.swipeDown && !isSliding)
        {
            StartCoroutine(Slide());
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
            {
            StartCoroutine(Slide());
        }
            if (Input.GetKeyDown(KeyCode.RightArrow)|| SwipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
           
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)|| SwipeManager.swipeLeft)
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;

        }
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if(desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance; 
        }
       else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }
        transform.position = targetPosition;
        /*  if(transform.position == targetPosition)
          {
              return;
              Vector3 diff = targetPosition - transform.position;
              Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
              if (moveDir.sqrMagnitude < diff.sqrMagnitude)
                  controller.Move(moveDir);
              else
                  controller.Move(diff);
          }*/
        controller.center = controller.center;
    }
    private void FixedUpdate()
    
    {
        if (!PlayerManager.isGameStarted)
            return;
        controller.Move(direction * Time.fixedDeltaTime);
        animator.SetBool("isGrounded", false);
    }
    private void Jump()
    {
        direction.y = jumpForce;
        isGrounded = true;
        animator.SetBool("isGrounded", true);
      //  yield return new WaitForSeconds(1.3f);
       
        
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
            audioSource1.Play();
        }
    }
    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;
        yield return new WaitForSeconds(1.3f);

        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;
        animator.SetBool("isSliding", false);
        isSliding = false;
    }
}
