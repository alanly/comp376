using UnityEngine;
using System.Collections;

namespace Hydrogen.Entities.Atom {

	public abstract class RevolvingParticles : MonoBehaviour {

		public float   rotationSpeed;
		public Vector3 rotationAxis;

		protected void Update() {
			// Rotate around the parent origin.
			this.RotateAboutParent();
		}

		private void RotateAboutParent() {
			this.transform.RotateAround(
				this.transform.parent.position,
				this.rotationAxis,
				this.rotationSpeed * Time.deltaTime
			);
		}

	}

}
