using UnityEngine;
using System.Collections;
using Hydrogen.Core;

namespace Hydrogen.Entities.Atom {

	public class Atom : WrappingEntity {

		public int   initialHealth = 3;
		public int   hitPoint = 1;
		public int   killPoint = 2;
		public float growthStep = 1.5f;
		public float growthTime = 2f;
		public float moveSpeed = 0.75f;
		public GameObject deathParticle;

		private int     hits = 0;
		private GameObject nucleusCluster;
		private Vector3 targetGrowth;
		private bool isDead = false;
		private bool isGrowing = false;
		private bool isFollowPlayer = false;

		public void Hit()
		{
			// Notify the AtomController that the Atom has been initially hit.
			if (this.hits == 0) AtomController.instance.AtomHit();

			// Increment hit count of this Atom
			this.hits++;

			// Destroy the Atom if it's dead.
			if (this.hits == this.initialHealth)
			{
				// Increment the current score by two.
				GameController.instance.AddScore(this.killPoint);

				// Die
				this.Die();
				return;
			}

			// Increment the current score by one.
			GameController.instance.AddScore(this.hitPoint);

			// Grow the Atom's nucleus if it's still alive.
			this.Grow();

			// Move towards the player
			this.isFollowPlayer = true;
		}

		new private void Start()
		{
			base.Start ();

			this.nucleusCluster = this.GetComponentInChildren<NucleusCluster>().gameObject;
		}

		private void Grow()
		{
			// Increase the size of the nucleus
			this.targetGrowth = this.nucleusCluster.transform.localScale * this.growthStep;

			// Increase the size of the sphere collider.
			SphereCollider sc = this.GetComponent<SphereCollider>();
			sc.radius = (sc.radius * this.growthStep) - (this.growthStep * 0.1f);

			this.isGrowing = true;
		}

		public void Die()
		{
			// Instantiate the particle system and play it.
			GameObject particle = GameObject.Instantiate (this.deathParticle) as GameObject;
			particle.transform.position = this.transform.position;
			particle.particleSystem.Play();
			Destroy (particle, 5f);

			// Declare this item dead.
			this.isDead = true;

			// Shrink the nucleus
			this.targetGrowth = Vector3.zero;
			this.growthTime *= 5f;
			this.isGrowing = true;

			// Disable collider
			SphereCollider sc = this.GetComponent<SphereCollider>();
			sc.enabled = false;

			// Notify the AtomController
			AtomController.instance.AtomDied();
		}

		private void FixedUpdate()
		{
			if (this.isGrowing)
			{
				float lerpTime = Time.deltaTime * this.growthTime;
				Vector3 currentScale = this.nucleusCluster.transform.localScale;

				if (currentScale == this.targetGrowth)
				{
					this.isGrowing = false;
					return;
				}

				this.nucleusCluster.transform.localScale = Vector3.MoveTowards(
					currentScale, this.targetGrowth, lerpTime
				);
			}

			if (! this.isGrowing && this.isDead)
			{
				Destroy (this.gameObject);
			}

			if (this.isFollowPlayer)
			{
				Vector3 playerPosition = GameController.instance.player.transform.position;
				float moveStep = this.moveSpeed * Time.deltaTime;
				this.transform.position = Vector3.MoveTowards(this.transform.position, playerPosition, moveStep);
			}
		}

		public void EnableCollider()
		{
			Debug.Log ("Enable atom collider");
			this.collider.enabled = true;
		}

		public void DisableCollider()
		{
			Debug.Log ("Disable atom collider");
			this.collider.enabled = false;
		}

	}

}
