using UnityEngine;
using System.Collections;

namespace Helium.Entities
{
	public class WrappingEntity : MonoBehaviour
	{
		private bool isWrappingX = false;
		private bool isWrappingY = false;
		private bool isVisible;

		protected bool wrapEnabled = true;

		void OnBecameVisible()
		{
			this.isVisible = true;
		}

		void OnBecameInvisible()
		{
			this.isVisible = false;
		}

		// Retrieve the renderers for the scene.
		protected void Awake()
		{
			this.isVisible = false;
		}

		// Update is called once per frame
		protected void FixedUpdate()
		{
			if (this.wrapEnabled)
			{
				this.HandleWrapAround();
			}
		}

		// Determine whether or not this entity is currently visible on a screen.
		protected bool IsVisible()
		{
			return this.isVisible;
		}

		// Handle wrapping around edges of the screen.
		private void HandleWrapAround()
		{
			bool isVisible = this.IsVisible();

			if (isVisible)
			{
				this.isWrappingX = this.isWrappingY = false;
				return;
			}

			if (this.isWrappingX && this.isWrappingY) return;

			Camera mainCamera = Camera.main;
			Vector3 viewportPosition = mainCamera.WorldToViewportPoint(this.transform.position);
			Vector3 supposedPosition = transform.position;

			if (! this.isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
			{
				supposedPosition.x = - supposedPosition.x;
				this.isWrappingX = true;
			}

			if (! this.isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
			{
				supposedPosition.y = - supposedPosition.y;
				this.isWrappingY = true;
			}

			if (this.isWrappingX || this.isWrappingY)
			{
				this.transform.position = supposedPosition;
			}
		}
	}
}