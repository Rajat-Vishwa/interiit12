using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float moveSpeed;
    public float jumpForce = 100f;
	public float gravity = -20.0f;
    [Space]

	private float InputX;
	private float InputZ;
	private Vector3 desiredMoveDirection;
	public bool blockRotationPlayer;
	public float desiredRotationSpeed = 0.1f;
	public Animator anim;
	public float allowPlayerRotation = 0.1f; 
	public CharacterController controller;	private bool isGrounded;
    public bool isJumping;
	private float verticalVelocity;
    public float minX, maxX;

    private Vector3 moveVector;

	void Start () 
    {
		anim = GetComponent<Animator> ();
		controller = GetComponent<CharacterController>();
	}
	
	void Update () 
    {
		InputX = Input.GetAxis ("Horizontal");	
		//InputZ = Input.GetAxis ("Vertical");
        InputZ = 1f;
        
        PlayerMoveAndRotation();
		//isGrounded = controller.isGrounded;
		
		if (controller.isGrounded){
			verticalVelocity = 0;  
			isJumping = false; 
		}
		else{
			verticalVelocity += gravity * Time.deltaTime; // apply gravity on y-axis
		}

		// if (Input.GetButtonDown("Jump") && !isJumping){
		// 	anim.SetTrigger("Jump");
		// 	verticalVelocity = jumpForce; // Apply the jump force
		// 	isJumping = true;
		// }
		// controller.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime); // Apply vertical velocity to the character
        
        // Clamp X position 
        var pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }

    private void PlayerMoveAndRotation() {

		desiredMoveDirection = Vector3.forward * InputZ + Vector3.right * InputX;
    
		if (blockRotationPlayer == false) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (desiredMoveDirection), desiredRotationSpeed);
            controller.Move(Vector3.right * InputX * Time.deltaTime * moveSpeed);
		}
	}

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Ground" || true){
            isJumping = false;
        }
    }
}
