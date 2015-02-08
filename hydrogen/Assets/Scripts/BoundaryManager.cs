using UnityEngine;
using System.Collections;

namespace Hydrogen.Core {

	public class BoundaryManager : MonoBehaviour {

		public GameObject Heaven;
		public GameObject Earth;
		public GameObject North;
		public GameObject South;
		public GameObject East;
		public GameObject West;

		public GameObject GetMirrorBoundary(BoundaryCardinals cardinal)
		{
			switch(cardinal)
			{
				case BoundaryCardinals.HEAVEN:
					return this.Earth;
				case BoundaryCardinals.EARTH:
					return this.Heaven;
				case BoundaryCardinals.NORTH:
					return this.South;
				case BoundaryCardinals.SOUTH:
					return this.North;
				case BoundaryCardinals.EAST:
					return this.West;
				case BoundaryCardinals.WEST:
					return this.East;
				default:
					return null;
			}
		}

		public GameObject GetRandomBoundary(bool horizontalOnly)
		{
			GameObject[] boundaries = new GameObject[6];
			
			boundaries[0] = this.Heaven;
			boundaries[1] = this.Earth;
			boundaries[2] = this.North;
			boundaries[3] = this.South;
			boundaries[4] = this.East;
			boundaries[5] = this.West;

			int randomIndex = (new System.Random())
				.Next(
					(horizontalOnly ? 2 : 0),
					5
				);
			
			return boundaries[randomIndex];
		}

		public GameObject GetRandomBoundary()
		{
			return this.GetRandomBoundary(false);
		}

	}

}
