using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour{

    public int hits = 0;

    [Range(1, 100)] public float speed;
	[Range(1, 100)] public float jumpFactor;

	CameraScript cam;

	private Transform myTransform;
	private Rigidbody2D myRigidbody2D;
	private Collider2D myCollider2D;

	private int pressedSpace;
	private bool onGround;
	public bool canDoubleJump = true;

	private bool dead;


	// Use this for initialization
	void Awake(){
        cam = GameObject.Find("Main Camera").GetComponent<CameraScript>();

		myTransform = GetComponent<Transform>();
		myRigidbody2D = GetComponent<Rigidbody2D>();
		myCollider2D = GetComponent<Collider2D>();
	}

	private void FixedUpdate(){
		float fixtime = Time.fixedDeltaTime;	
		Vector2 currentVelocity = myRigidbody2D.velocity;
		Vector2 impulse	= Vector2.zero;
		float desiredSpeed;
			
        //controlled mode
		if (Input.GetKey(KeyCode.RightArrow)){
			desiredSpeed = speed;
		}
		else if (Input.GetKey(KeyCode.LeftArrow)){
			desiredSpeed = -speed;
		}
        //auto mode
		desiredSpeed = speed;
		impulse.x = desiredSpeed - currentVelocity.x;
		
		
		if (pressedSpace>0){
			--pressedSpace;
            cam.totalJumps++;
		
			if (onGround){
				impulse.y = jumpFactor;
			}

			
			if (!onGround && currentVelocity.y <= 0 && canDoubleJump){
                cam.totalJumps++;
                canDoubleJump = false;
				impulse.y= jumpFactor*0.8f;

				

			}
		}		
		myRigidbody2D.AddForce(impulse,ForceMode2D.Impulse);
		
		//TODO test raycast approach instead of colliders
	}


	//feet trigger
	private void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag("Floor")){
			canDoubleJump=onGround = true;
			
		}
	}
	
	private void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.CompareTag("Floor")){
			canDoubleJump=onGround = true;
		}
	}

	
	private void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.CompareTag("Floor")){
			onGround = false;
		}
	}


	// Update is called once per frame
	private void Update(){
		pressedSpace += Input.GetKeyDown(KeyCode.Space) ? 1 : 0;
		if (myTransform.position.y+myCollider2D.bounds.size.y<= cam.getlowerBound()){
			Debug.Log("player dead");
			dead = true;
		}
	}

	private void OnDestroy(){
		Debug.Log("I am dead");
	}

	public bool getAlive(){
		return !dead;
	}

    public void setAlive() {
        dead = false;
    }
}
