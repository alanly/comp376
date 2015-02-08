using UnityEngine;
using System.Collections;

namespace Helium.Entities
{
	public class WaterBalloon : Balloon
	{
		new void Awake()
		{
			base.Awake();
			
			this.updateCounts = false;
		}
	}
}
