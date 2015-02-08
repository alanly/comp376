using UnityEngine;
using System.Collections.Generic;
using Helium.Core;

namespace Helium.Entities
{
	public class BalloonCluster : WrappingEntity
	{
		public float velocity = 1f;
		private Stack<GameObject> balloons;

		new protected void Awake()
		{
			base.Awake();

			GameController.IncrementClusterCounter();
		}

		void OnDestroy()
		{
			GameController.DecrementClusterCounter();
		}

		protected void Update()
		{
			if (this.balloons.Count == 0)
			{
				Debug.Log("Empty Cluster");
				Destroy(this.gameObject);
			}
		}

		public void SetBalloons(GameObject[] balloons)
		{
			this.balloons = new Stack<GameObject>(balloons);
		}

		public Stack<GameObject> GetBalloons()
		{
			return this.balloons;
		}

		public void SetPosition(Vector3 position)
		{
			this.transform.position = position;

			Vector2 velocity = position.normalized * this.velocity;
			this.rigidbody2D.velocity = velocity;
		}

		private void Divide()
		{
			GameObject clusterController = GameObject.FindGameObjectWithTag("BalloonClusterController");
			clusterController.SendMessage("DivideCluster", this);
		}

		public void OnTriggerEnter2D(Collider2D collider)
		{
			GameObject collidingObject = collider.gameObject;

			// Handle collision with player
			if (collidingObject.tag == "Player")
			{
				collidingObject.SendMessage("Die");
				this.Divide();
			}

			if (collidingObject.tag == "Bullet")
			{
				GameController.IncrementScore(1);
				collidingObject.SendMessage("Hit");
				this.Divide();
			}
		}
	}
}
