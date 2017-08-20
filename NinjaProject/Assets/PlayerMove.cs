using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

	public float force_move = 100;
	public float jumpVelocity = 100;
	private Animator anim;
	private bool isGround = false;
	private bool isWall = false;
	private bool isSlide = false;

	private Transform wallTrans;

	void Awake(){
		anim = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis ("Horizontal");
		Vector2 velocity = rigidbody2D.velocity;

		if (isSlide == false) {
			if (h > 0.05f) {
				rigidbody2D.AddForce (Vector2.right * force_move);
			} else if (h < -0.05f) {
				rigidbody2D.AddForce (-Vector2.right * force_move);
			}

			//修改方向
			if (h > 0.05f) {			//朝向右方向
				transform.localScale = new Vector3 (1, 1, 1);
			} else if (h < -0.05f) {	//朝向左方向
				transform.localScale = new Vector3 (-1, 1, 1);
			}

			anim.SetFloat ("horizontal", Mathf.Abs (h));
		}else{
			//当我们在墙上滑行的时候
		}
		
		if (isGround && Input.GetKeyDown (KeyCode.Space)) {
			//进行跳跃
			velocity.y = jumpVelocity;
			rigidbody2D.velocity = velocity;
			if (isWall) {
				rigidbody2D.gravityScale = 5;
			}
		}
		anim.SetFloat ("vertical", rigidbody2D.velocity.y);

		if (isWall == false || isGround == true) {
			isSlide = false;
		}
	}

	public void OnCollisionEnter2D(Collision2D col){
		if (col.collider.tag == "Ground") {
			isGround = true;
			rigidbody2D.gravityScale = 30;
		}
		if (col.collider.tag == "Wall") {
			isWall = true;
			rigidbody2D.velocity = Vector2.zero;
			rigidbody2D.gravityScale = 5;
			wallTrans = col.collider.transform;
		}
		anim.SetBool ("isGround", isGround);
		anim.SetBool ("isWall", isWall);
	}

	public void OnCollisionExit2D(Collision2D col){
		if (col.collider.tag == "Ground") {
			isGround = false;
		}
		if (col.collider.tag == "Wall") {
			isWall = false;
			rigidbody2D.gravityScale = 30;
		}
		anim.SetBool ("isGround", isGround);
		anim.SetBool ("isWall", isWall);
	}
	//更改朝向
	public void ChangeDir(){
		isSlide = true;
		if (wallTrans.position.x < transform.position.x) {
			transform.localScale = new Vector3 (1, 1, 1);
		} else {
			transform.localScale = new Vector3 (-1, 1, 1);
		}
	}
}
