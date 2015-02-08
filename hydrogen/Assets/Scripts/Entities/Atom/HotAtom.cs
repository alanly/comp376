using UnityEngine;
using System.Collections;
using Hydrogen.Core;
using Hydrogen.Entities;

namespace Hydrogen.Entities.Atom {

	public class HotAtom : WrappingEntity {

		public int KillPoint = 10;
		public GameObject DeathParticle;

		public void OnCollisionStay(Collision collision)
		{
			GameObject obj = collision.gameObject;
			
			if (obj.CompareTag("Atom"))
			{
				this.rigidbody.AddExplosionForce(100f, this.transform.position, 20f);
			}
		}

		public void Die()
		{
			// Instantiate the particle system and play it.
			GameObject particle = GameObject.Instantiate (this.DeathParticle) as GameObject;
			particle.transform.position = this.transform.position;
			particle.particleSystem.Play();
			Destroy (particle, 5f);

			// Notify the AtomController
			AtomController.instance.AtomDied();

			Destroy (this.gameObject);
		}

		public void Hit()
		{
			// Increment the current score by two.
			GameController.instance.AddScore(this.KillPoint);
			
			// Die
			this.Die();
			return;
		}

	}

}
