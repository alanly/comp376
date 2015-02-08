using UnityEngine;
using System.Collections;
using Hydrogen.Core;

namespace Hydrogen.Entities {

	public abstract class WrappingEntity : MonoBehaviour {

		private GameObject ignoreBoundary;
		private BoundaryManager boundaryManager;

		protected virtual void Start()
		{
			// Get the Boundary Manager instance.
			GameObject boundaryBox = GameObject.FindGameObjectWithTag("BoundaryBox");

			if (boundaryBox != null)
			{
				this.boundaryManager = boundaryBox.GetComponent<BoundaryManager>();
			}
		}

		protected virtual void OnBoundaryEnter(Collider boundary) {}

		protected virtual void OnBoundaryExit(Collider boundary) {}

		protected void OnTriggerEnter(Collider collider)
		{
			// Get the reference of the colliding object.
			GameObject collidingObject = collider.gameObject;

			// Handle collision with a Boundary object.
			if (collidingObject.CompareTag("Boundary"))
			{
				// Check if we should ignore this boundary or not
				if (collidingObject == this.ignoreBoundary) return;

				// Wrap this entity around.
				this.WrapEntity(collidingObject);

				// Call the OnBoundaryEnter event in-case someone wants to act on it.
				this.OnBoundaryEnter(collider);
			}
		}

		protected void OnTriggerExit(Collider collider)
		{
			// Get the reference of the game object we're leaving
			GameObject exitObject = collider.gameObject;

			// Handle a Boundary object
			if (exitObject.CompareTag("Boundary"))
			{
				// Reset the boundary ignore field once we've left it.
				if (exitObject == this.ignoreBoundary) this.ignoreBoundary = null;

				this.OnBoundaryExit(collider);
			}
		}

		protected void WrapEntity(GameObject boundary)
		{
			// Get the MonoBehaviour of the boundary instance.
			BoundaryPlane plane = boundary.GetComponent<BoundaryPlane>();

			// Get the axis of this boundary plane.
			Vector3 planeAxis = plane.GetAxis();

			// Calculate the position of this entity on the opposing side.
			Vector3 currentPosition = this.transform.position;
			Vector3 targetPosition = currentPosition + Vector3.Scale(currentPosition, (planeAxis * -2f));

			// Translate the current entity to that point.
			this.transform.position = targetPosition;

			// Set which boundary to ignore (because when we wrap, we will end up colliding with the opposing boundary)
			this.ignoreBoundary = this.boundaryManager.GetMirrorBoundary(plane.cardinal);
		}

	}

}
