using UnityEngine;
using System.Collections;
using Helium.Core;

namespace Helium.Entities
{
	public class Player : WrappingEntity
	{
		public float      rotationForce    = 5f;
		public float      translationForce = 10f;
		public float      shotDelay        = 0.25f;
		public float      immunityTime     = 3f;
		public float      multiShotTime    = 3f;
		public GameObject bullet;

		private bool  isDead           = false;
		private bool  isMultiShot      = false;
		private bool  invokedDisableMS = false;
		private float timeOfLastShot   = 0;

		void Start()
		{
			this.EnableImmunity();
			Invoke("DisableImmunity", 3f);
		}
		
		new void FixedUpdate()
		{
			base.FixedUpdate();

			// Death
			if (this.isDead)
			{
				if (! this.IsVisible())
				{
					Destroy(this.gameObject);
					GameController.PlayerDied();
				}

				return;
			}

			this.HandleMovementInput();
			this.HandleShooting();
		}

		private void HandleMovementInput()
		{
			// Handle moving forwards and backwards.
			float vMove = Input.GetAxis("Vertical");
			this.rigidbody2D.AddRelativeForce(new Vector2(0, vMove * this.translationForce));

			// Handle rotating in-place.
			float hMove = -Input.GetAxis("Horizontal");
			this.transform.Rotate(new Vector3(0f, 0f, hMove * this.rotationForce));
		}

		private void HandleShooting()
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				this.isMultiShot = true;
			}
			else
			{
				this.isMultiShot = false;
			}

			if (Input.GetButton("Fire1") || Input.GetButton("Jump"))
			{
				if (Time.time < (this.timeOfLastShot + this.shotDelay)) return;

				this.timeOfLastShot = Time.time;
				this.FireBullet(true);

				if (! GameController.UsedSpecialShoot && this.isMultiShot)
				{
					this.FireBullet(false);

					if (! this.invokedDisableMS)
					{
						Invoke("DisableMultiShoot", this.multiShotTime);
						this.invokedDisableMS = true;
					}
				}
			}
		}

		public void FireBullet(bool forward)
		{
			Vector3 bulletPosition = this.transform.position + this.transform.up * 1.01f;
			GameObject bullet = (GameObject)Instantiate(this.bullet, bulletPosition, Quaternion.identity);

			bullet.transform.rotation = this.transform.rotation;
			
			if (! forward)
			{
				bullet.transform.Rotate(Vector3.forward, 180f);

				bullet.GetComponent<SpriteRenderer>().color = new Color(0, 1f, 0);
			}

			bullet.rigidbody2D.AddRelativeForce(new Vector2(0, 500f));
			Object.Destroy(bullet, 2.5f);
		}

		public void Die()
		{
			// Let other methods know that we're dead, and don't wrap as we fall.
			this.isDead = true;
			this.wrapEnabled = false;

			// Disable collider so we don't affect any other entity on the way down.
			this.collider2D.enabled = false;

			// Fall.
			this.rigidbody2D.gravityScale = 5f;
		}

		public void EnableImmunity()
		{
			this.collider2D.enabled = false;
			SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
			sr.color = new Color(1f, 0, 0);

			Debug.Log("Player is Immune");
		}

		public void DisableImmunity()
		{
			this.collider2D.enabled = true;
			SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
			sr.color = new Color(1f, 1f, 1f);

			Debug.Log("Player is no longer Immune");
		}

		public void DisableMultiShoot()
		{
			GameController.UsedSpecialShoot = true;
		}
	}
}
