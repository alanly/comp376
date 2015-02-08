using UnityEngine;
using System.Collections.Generic;
using Helium.Core;

namespace Helium.Entities
{
	public class BalloonClusterController : ScriptableObject
	{
		public const int    BALLOONS_PER_CLUSTER     = 6;
		public const int    MIN_BALLOONS_PER_CLUSTER = 3;
		public const string BALLOON_PREFAB_PATH      = "Prefabs/ShadyBalloon";
		public const string CLUSTER_PREFAB_PATH      = "Prefabs/BalloonCluster";

		private GameObject balloonPrefab;
		private GameObject clusterPrefab;
		private Camera     camera;

		protected void Awake()
		{
			this.balloonPrefab = GameResources.Load(BalloonClusterController.BALLOON_PREFAB_PATH);
			this.clusterPrefab = GameResources.Load(BalloonClusterController.CLUSTER_PREFAB_PATH);
			this.camera = Camera.main;
		}

		public void DivideCluster(BalloonCluster cluster)
		{
			Stack<GameObject> clusterBalloons = cluster.GetBalloons();
			GameObject clusterGameObject = cluster.gameObject;
			int clusterSize = clusterBalloons.Count;

			// If the target cluster size is too small, just break up the cluster.
			if ((clusterSize / 2) < BalloonClusterController.MIN_BALLOONS_PER_CLUSTER)
			{
				this.BreakCluster(cluster);
				return;
			}

			// Take half the balloons in the cluster.
			int newClusterSize = clusterSize / 2;
			GameObject[] newClusterBalloons = new GameObject[newClusterSize];

			for (int i = 0; i < newClusterSize; i++)
			{
				newClusterBalloons[i] = clusterBalloons.Pop();
			}

			// Make a new cluster
			GameObject newCluster = this.MakeClusterWithBalloons(newClusterBalloons);
			newCluster.transform.position = clusterGameObject.transform.position + new Vector3(0.1f, 0.1f, 0);
			CircleCollider2D collider = newCluster.GetComponent<CircleCollider2D>();
			collider.radius = collider.radius / 2;
		}

		private void BreakCluster(BalloonCluster cluster)
		{
			Stack<GameObject> clusterBalloons = cluster.GetBalloons();
			GameObject clusterGameObject = cluster.gameObject;
			
			foreach (GameObject balloon in clusterBalloons)
			{
				if (balloon == null) continue;

				// Unparent the balloon
				balloon.transform.parent = null;

				// Enable the balloon's collider
				balloon.collider2D.enabled = true;

				// Enable wrapping
				balloon.SendMessage("EnableWrapping");

				// Give the balloon a velocity
				balloon.rigidbody2D.isKinematic = false;
				balloon.rigidbody2D.velocity = clusterGameObject.rigidbody2D.velocity;

				Vector2 direction = Vector2.MoveTowards(balloon.transform.position, clusterGameObject.transform.position, -1f);
				balloon.rigidbody2D.AddForce(direction * 10f);
			}

			Destroy(clusterGameObject);
		}

		private GameObject MakeCluster()
		{
			// Create our cluster and our balloons
			GameObject cluster = this.CreateClusterInstance();

			// Determine where to place the cluster
			float yAxis = Random.Range(0.2f, 0.8f);
			float xAxis = (Random.value > 0.5f) ? 0.9f : 0.1f;
			Vector3 viewportPosition = new Vector3(xAxis, yAxis, camera.nearClipPlane);
			Vector3 worldPosition = camera.ViewportToWorldPoint(viewportPosition);
			worldPosition.z = 0;

			cluster.SendMessage("SetPosition", worldPosition);

			return cluster;
		}

		private GameObject MakeClusterAndBalloons()
		{
			GameObject cluster = this.MakeCluster();
			GameObject[] balloons = this.MakeBalloons(cluster);

			cluster.SendMessage("SetBalloons", balloons);

			return cluster;
		}

		private GameObject MakeClusterWithBalloons(GameObject[] balloons)
		{
			GameObject cluster = this.MakeCluster();

			foreach (GameObject balloon in balloons)
			{
				balloon.transform.parent = cluster.transform;
				
				Vector3 positionOffset = (Random.insideUnitCircle / 2f) * ((float)balloons.Length / (float)BalloonClusterController.BALLOONS_PER_CLUSTER);
				balloon.transform.position = cluster.transform.position + positionOffset;
			}

			cluster.SendMessage("SetBalloons", balloons);

			return cluster;
		}

		private GameObject CreateClusterInstance()
		{
			return GameObject.Instantiate(this.clusterPrefab) as GameObject;
		}

		private GameObject[] MakeBalloons(GameObject parent)
		{
			GameObject[] balloons = new GameObject[BalloonClusterController.BALLOONS_PER_CLUSTER];

			for (int i = 0; i < BalloonClusterController.BALLOONS_PER_CLUSTER; i++)
			{
				// Generate a position offset for the balloon, relative to the cluster center.
				Vector3 positionOffset = Random.insideUnitCircle / 2f;

				balloons[i] = this.MakeBalloon(parent.transform.position + positionOffset, parent);
			}

			return balloons;
		}

		private GameObject CreateBalloonInstance()
		{
			return GameObject.Instantiate(this.balloonPrefab) as GameObject;
		}

		private GameObject MakeBalloon(Vector3 position, GameObject parent)
		{
			// Create a random colour
			Color color = new Color(
				Random.Range(0.25f, 1f),
				Random.Range(0.25f, 1f),
				Random.Range(0.25f, 1f)
			);
			
			// Create the balloon instance
			GameObject balloon = this.CreateBalloonInstance();
			balloon.transform.position = position;
			balloon.transform.parent = parent.transform;

			// Disable the balloon's collider
			balloon.collider2D.enabled = false;

			// Disable wrapping
			balloon.SendMessage("DisableWrapping");

			// Set the color
			SpriteRenderer sprite = balloon.GetComponent<SpriteRenderer>();
			sprite.color = color;

			return balloon;
		}
	}
}
