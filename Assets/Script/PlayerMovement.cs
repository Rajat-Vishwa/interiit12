
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

//This script requires you to have setup your animator with 3 parameters, "InputMagnitude", "InputX", "InputZ"
//With a blend tree to control the inputmagnitude and allow blending between animations.
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {

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
	private float Speed;
	public float allowPlayerRotation = 0.1f; 
	public Camera cam;
	public CharacterController controller;	private bool isGrounded;
    public bool isJumping;
	private float verticalVelocity;


    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    public float HorizontalAnimSmoothTime = 0.2f;
    [Range(0, 1f)]
    public float VerticalAnimTime = 0.2f;
    [Range(0,1f)] 
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;

    private Vector3 moveVector;

	void Start () 
    {
		anim = GetComponent<Animator> ();
		cam = Camera.main;
		controller = GetComponent<CharacterController>();
	}
	
	void Update () 
    {
		InputX = Input.GetAxis ("Horizontal");	
		InputZ = Input.GetAxis ("Vertical");

		anim.SetFloat ("InputZ", InputZ, VerticalAnimTime, Time.deltaTime * 2f);
		anim.SetFloat ("InputX", InputX, HorizontalAnimSmoothTime, Time.deltaTime * 2f);

		Speed = new Vector2(InputX, InputZ).sqrMagnitude;

		if (Speed > allowPlayerRotation) {
			anim.SetFloat ("Blend", Speed, StartAnimTime, Time.deltaTime);
			PlayerMoveAndRotation ();
		} else if (Speed < allowPlayerRotation) {
			anim.SetFloat ("Blend", Speed, StopAnimTime, Time.deltaTime);
		}

		//isGrounded = controller.isGrounded;
		
		if (controller.isGrounded){
			verticalVelocity = 0;  
			isJumping = false; 
		}
		else{
			verticalVelocity += gravity * Time.deltaTime; // apply gravity on y-axis
		}

		if (Input.GetButtonDown("Jump") && !isJumping){
			anim.SetTrigger("Jump");
			//verticalVelocity = jumpForce; // Apply the jump force
			isJumping = true;
		}
		//controller.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime); // Apply vertical velocity to the character
    }

    void PlayerMoveAndRotation() {
		var camera = Camera.main;
		var forward = cam.transform.forward;
		var right = cam.transform.right;

		forward.y = 0f;
		right.y = 0f;

		forward.Normalize ();
		right.Normalize ();

		desiredMoveDirection = forward * InputZ + right * InputX;
	
		if (blockRotationPlayer == false) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (desiredMoveDirection), desiredRotationSpeed);
            controller.Move(desiredMoveDirection * Time.deltaTime * moveSpeed);
		}
	}

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Ground" || true){
            isJumping = false;
        }
    }

}
