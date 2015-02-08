using UnityEngine;
using System.Collections;
using Helium.Core;

namespace Helium.Entities
{
	public class Balloon : WrappingEntity
	{
		protected bool updateCounts = true;

		void Start()
		{
			if (this.updateCounts) GameController.IncrementBalloonCounter();
		}

		void OnDestroy()
		{
			if (this.updateCounts) GameController.AddBalloonDestroyed();
		}

		public void DisableWrapping()
		{
			this.wrapEnabled = false;
		}

		public void EnableWrapping()
		{
			this.wrapEnabled = true;
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			GameObject collidingObject = collider.gameObject;

			// Handle colliding with a bullet.
			if (collidingObject.tag == "Bullet")
			{
				GameController.IncrementScore(2);
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
