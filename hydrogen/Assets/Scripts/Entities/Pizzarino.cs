using UnityEngine;
using System.Collections;
using Hydrogen.Core;

namespace Hydrogen.Entities {
	public class Pizzarino : MonoBehaviour {

		private void Start()
		{
		}

		private void Update()
		{
			this.transform.Rotate(new Vector3(1, 1, 0) * Time.deltaTime * 100f);
			this.transform.position = Vector3.MoveTowards(
				this.transform.position,
				GameController.instance.player.transform.position,
				Time.deltaTime * 2f
			);
		}

		public void Die()
		{
			Destroy (this.gameObject);
		}

	}
}
