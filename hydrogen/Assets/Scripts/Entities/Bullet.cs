using UnityEngine;
using System.Collections;

namespace Hydrogen.Entities {

	public class Bullet : WrappingEntity {

		public float firingForce = 1000f;
		public float maxLifetime = 10f;

		private float startTime = 0;

		public void Fire()
		{
			this.rigidbody.velocity = this.transform.forward * this.firingForce;
		}

		new private void Start()
		{
			base.Start();

			this.startTime = Time.fixedTime;
		}

		private void OnCollisionEnter(Collision collision)
		{
			GameObject collidingObj = collision.gameObject;

			if (collidingObj.CompareTag("Atom"))
			{
				collidingObj.SendMessage("Hit");
			}

			Destroy (this.gameObject);
		}

		override protected void OnBoundaryEnter(Collider collider)
		{
			Destroy (this.gameObject);
		}

		private void Update()
		{
			if ((Time.time - this.startTime) > this.maxLifetime) Destroy (this.gameObject);
		}

	}

}
