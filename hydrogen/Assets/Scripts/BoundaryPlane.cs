using UnityEngine;
using System.Collections;

namespace Hydrogen.Core {

	public class BoundaryPlane : MonoBehaviour {

		public BoundaryCardinals cardinal;

		private Vector3 axis;

		protected void Start()
		{
			this.axis = this.CalculateAxisValue();
		}

		private Vector3 CalculateAxisValue()
		{
			Vector3 normalized = this.transform.localPosition.normalized;
			return Vector3.Scale(normalized, normalized);
		}

		public Vector3 GetAxis()
		{
			return this.axis;
		}

	}

}
