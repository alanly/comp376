using UnityEngine;
using System.Collections;
using Helium.Core;

namespace Helium.Entities
{
	public class HotAirBalloon : MonoBehaviour
	{
		public float      ThrowInterval = 5f;
		public float      ThrowVelocity = 3f;
		public GameObject balloonPrefab;

		private bool       isJustSpawned = true;
		private float      timeOfLastThrow = 0;
		private GameObject player;

		/**
		 * Throw a balloon at every ThrowInterval.
		 */
		void FixedUpdate()
		{
			if ((Time.time - this.timeOfLastThrow) > this.ThrowInterval)
			{
				this.timeOfLastThrow = Time.time;
				this.player = GameObject.FindGameObjectWithTag("Player");
				this.ThrowABalloon();
			}
		}

		void ThrowABalloon()
		{
			// Wait for the player to respawn
			if (this.player == null) return;

			// Get the player's position
			Vector2 playerPosition = this.player.transform.position;

			// Instantiate a balloon and shoot it towards the player.
			GameObject balloon = Instantiate(balloonPrefab) as GameObject;
			balloon.transform.position = this.transform.position;
			balloon.rigidbody2D.isKinematic = false;
			balloon.rigidbody2D.velocity = -1f * Vector2.MoveTowards(this.transform.position, playerPosition, this.ThrowVelocity);
		}

		void OnBecameVisible()
		{
			this.isJustSpawned = false;
		}

		/**
		 * If this hot air balloon is no longer visible, destroy it.
		 */
		void OnBecameInvisible()
		{
			if (! this.isJustSpawned) Destroy(this.gameObject);
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			GameObject collidingObject = collider.gameObject;

			// Handle colliding with a bullet.
			if (collidingObject.tag == "Bullet")
			{
				GameController.IncrementScore(10);
				collidingObject.SendMessage("Hit");
				Destroy(this.gameObject);
			}

			// Handle colliding with a player.
			if (collidingObject.tag == "Player")
			{				
				collidingObject.SendMessage("Die");
				Destroy(this.gameObject);
			}
		}
	}
}
