using UnityEngine;
using System.Collections;

namespace Helium.Entities
{
	public class Bullet : WrappingEntity
	{
		void Start()
		{
			// Ignore player collisions
			Collider2D playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
			Physics2D.IgnoreCollision(this.collider2D, playerCollider);

			// Ignore collisions with other bullets
			Physics2D.IgnoreCollision(this.collider2D, this.collider2D);
		}

		public void Hit()
		{
			Destroy(this.gameObject);
		}
	}
}
