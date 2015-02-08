using UnityEngine;
using System.Collections;
using Hydrogen.Entities.Atom;

namespace Hydrogen.Core {

	public class AtomController : ScriptableObject {

		/**
		 * Singleton Instance
		 */

		private static AtomController _instance;

		public static AtomController instance
		{
			get { return _instance; }
		}


		/**
		 * Class Body
		 */

		public GameObject Atom;
		public GameObject HotAtomBalloon;

		private void Awake()
		{
			// Assign the AtomController instance.
			AtomController._instance = this;
		}

		public GameObject MakeAtom(Vector3 position, Vector3 forward, Vector3 velocity)
		{
			// Get a new Atom instance
			GameObject atom = GameObject.Instantiate(this.Atom) as GameObject;

			atom.transform.position = position;
			atom.rigidbody.velocity = velocity;

			return atom;
		}

		public void AtomDied()
		{
			GameController.instance.AtomHasDied();
		}

		public void AtomHit()
		{
			GameController.instance.AtomHasBeenHit();
		}

		public GameObject SpawnNewAtom()
		{
			// Get a random boundary plane
			GameObject boundary = GameController.instance.boundaries.GetRandomBoundary();

			// Determine the necessary vectors
			Vector3 position = boundary.transform.position;
			position -= position.normalized;
			Vector3 forward = - position;
			Vector3 velocity = forward * 0.25f;

			Debug.Log ("Atom instantiated at "+position+" with velocity "+velocity);

			// Create the Atom instance
			GameObject atom = this.MakeAtom(position, forward, velocity);

			return atom;
		}

		public GameObject SpawnNewHotAtomBalloon()
		{
			Debug.Log ("Spawning Balloon");

			GameObject balloon = GameObject.Instantiate (this.HotAtomBalloon) as GameObject;

			return balloon;
		}

	}

}
