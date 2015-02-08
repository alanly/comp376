using UnityEngine;
using System.Collections;
using Hydrogen.Core;

namespace Hydrogen.Entities {

	public class Player : WrappingEntity {

		public float translationForce = 10f;
		public float lookAroundSpeed = 5f;
		public float maxVerticalAngle = 90f;
		public float timeBetweenShots = 1f;
		public GameObject bulletObject;
		public GameObject fadeScreen;
		public AudioClip  DeathAudio;

		private float   timeOfLastShot = 0f;
		private Vector3 lookAtAngles;
		private FadeScreen fader;
		private bool clearFader = false;
		private bool isInputEnabled = true;
		private bool isAlive = true;

		new protected void Start ()
		{
			base.Start();

			// Fetch the initial angles
			this.lookAtAngles = this.transform.eulerAngles;

			// Lock the cursor on screen
			Screen.lockCursor = true;

			// Get the FadeScreen instance.
			this.fader = this.fadeScreen.GetComponent<FadeScreen>();
		}
		
		// Update is called once per frame
		private void Update() {
			this.InputHandler();

			if (this.clearFader && ! this.fader.IsFading())
			{
				this.clearFader = false;
				this.fader.FadeToTarget(Color.clear);
			}
		}

		private void InputHandler()
		{
			if (this.isInputEnabled)
			{
				this.HandleKeyInput();
				this.HandleMouseInput();
			}
		}

		private void HandleKeyInput()
		{
			// Handle Movement
			this.HandleKeyMovement();

			// Handle Shooting
			this.HandleShooting();
		}

		private void HandleKeyMovement()
		{
			// Move fore and aft
			float verticalMove = Input.GetAxis ("Vertical") * this.translationForce;
			// Strafe left or right
			float horizontalMove = Input.GetAxis ("Horizontal") * this.translationForce;
			
			// Add translating force
			this.rigidbody.AddRelativeForce (new Vector3 (horizontalMove, 0, verticalMove));
		}

		private void HandleShooting()
		{
			if (Input.GetButtonDown("Fire1"))
			{
				/**
				 * Check the time between shots.
				 */

				float durationFromLastShot = Time.time - this.timeOfLastShot;
				
				if (durationFromLastShot < this.timeBetweenShots) return;

				// Shoot
				this.Shoot();
			}
		}

		private void HandleMouseInput()
		{
			// Look up and down
			this.lookAtAngles.y -= Input.GetAxis ("Mouse Y") * this.lookAroundSpeed;
			// Look left and right
			this.lookAtAngles.x += Input.GetAxis ("Mouse X") * this.lookAroundSpeed;
			
			// Clamp the vertical angle
			this.lookAtAngles.y = Mathf.Clamp (this.lookAtAngles.y, -this.maxVerticalAngle, this.maxVerticalAngle);
			
			// Calculate the rotation quaternion based on the Euler angles.
			Quaternion headRotation = Quaternion.Euler (this.lookAtAngles.y, this.lookAtAngles.x, 0);

			// Rotate.
			this.transform.rotation = headRotation;
		}

		private void Shoot()
		{
			// Create a bullet instance with the same position and orientation as the player.
			GameObject bullet = GameObject.Instantiate(this.bulletObject) as GameObject;
			bullet.transform.position = this.transform.position;
			bullet.transform.forward = this.transform.forward;

			// Ignore collision between bullets and the player.
			Physics.IgnoreCollision(bullet.collider, this.collider);

			// Fire the bullet.
			bullet.SendMessage("Fire");
		}

		override protected void OnBoundaryEnter(Collider collider)
		{
			GameObject collisionObj = collider.gameObject;

			if (collisionObj.CompareTag("Boundary"))
			{
				this.fader.FadeToTarget(Color.black);

				if (! this.isAlive)
				{
					this.Die();
					return;
				}

				this.clearFader = true;
			}
		}

		private void OnCollisionEnter(Collision collision)
		{
			GameObject collidingObj = collision.gameObject;
			
			if (collidingObj.CompareTag("Atom") || collidingObj.CompareTag("Pizzarino"))
			{
				// Get the position
				Vector3 atomPosition = collidingObj.transform.position;

				if (collidingObj.CompareTag("Pizzarino"))
				{
					GameController.instance.PizzarinoDeath();
				}

				// Blow that atom up.
				collidingObj.SendMessage("Die");

				// Disable input for the player.
				this.isInputEnabled = false;

				// Play the death audio.
				if (this.isAlive) AudioSource.PlayClipAtPoint(this.DeathAudio, Camera.main.transform.position);

				// Look towards the atom.
				this.transform.forward = atomPosition;

				// Drop the player on the floor.
				this.rigidbody.useGravity = true;
				this.rigidbody.mass = 500f;
				this.rigidbody.drag = 0;

				// This player is dead.
				this.isAlive = false;
			}
		}

		public void Die()
		{
			// Notify the GameController
			GameController.instance.PlayerDied();
		}

		public void EnableCollider()
		{
			this.collider.enabled = true;
		}

		public void DisableCollider()
		{
			this.collider.enabled = false;
		}

		public void Restart()
		{
			// Reset to centre of the world
			this.transform.position = Vector3.zero;

			// Reset instance variables.
			this.clearFader = true;
			this.isInputEnabled = true;
			this.isAlive = true;

			// Reset rigidbody instance
			this.rigidbody.useGravity = false;
			this.rigidbody.mass = 0.3f;
			this.rigidbody.drag = 4;

			this.DisableCollider();
			this.Invoke("EnableCollider", 2);
		}

		public void DisableInput()
		{
			this.isInputEnabled = false;
		}

		public void EnableInput()
		{
			this.isInputEnabled = true;
		}

	}

}
