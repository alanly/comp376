using UnityEngine;
using System.Collections;
using Hydrogen.Core;

namespace Hydrogen.Entities {

	public class HotAtomBalloon : MonoBehaviour {

		public float      TranslationSpeed = 0.5f;
		public float      TimeBetweenAtoms = 2f;
		public GameObject HotAtom;

		private float lastAtomTime = 0;

		protected void OnTriggerEnter(Collider collider)
		{
			// Get the reference of the colliding object.
			GameObject collidingObject = collider.gameObject;
			
			// Handle collision with a Boundary object.
			if (collidingObject.CompareTag("Boundary"))
			{
				// Destroy once it hits the boundary.
				Destroy (this.gameObject);
			}
		}

		protected void Start()
		{
			// Pick a horizontal boundary to appear from.
			GameObject boundary = GameController.instance.boundaries.GetRandomBoundary(true);
			BoundaryPlane plane = boundary.GetComponent<BoundaryPlane>();

			Vector3 boundaryPosition = boundary.transform.position;

			Vector3 scale = Vector3.Scale (boundaryPosition, plane.GetAxis());

			// Determine the position to instantiate at
			Vector3 position = boundaryPosition - (boundaryPosition.normalized * 10f);
			position.y = Vector3.Magnitude(scale) - 25;
			Vector3 forward = - position;
			Vector3 velocity = forward * this.TranslationSpeed;
			velocity.y = 0;

			this.transform.position = position;
			this.transform.forward = forward;
			this.rigidbody.velocity = velocity;

			Debug.Log ("Balloon spawned at " + position + " going towards " + velocity);
		}

		protected void FixedUpdate()
		{
			float timeDelta = Time.time - this.lastAtomTime;

			if (timeDelta > this.TimeBetweenAtoms)
			{
				// Shoot a hot wad
				GameObject atom = GameObject.Instantiate (this.HotAtom) as GameObject;

				atom.transform.position = this.transform.position;
				atom.rigidbody.velocity = Vector3.MoveTowards(
					this.transform.position,
					GameController.instance.player.transform.position,
					3f
				);

				this.lastAtomTime = Time.time;
			}
		}

	}

}
