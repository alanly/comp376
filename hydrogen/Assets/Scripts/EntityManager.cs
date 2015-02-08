using UnityEngine;
using System.Collections;

namespace Hydrogen.Core {

	public class EntityManager {

		public Object LoadResource(string path) {
			return Resources.Load(path);
		}

		public Object LoadEntity(string name) {
			return this.LoadResource("Prefabs/Entities/"+name);
		}

		public GameObject GetEntityInstance(string name) {
			return GameObject.Instantiate(this.LoadEntity(name)) as GameObject;
		}

	}

}
