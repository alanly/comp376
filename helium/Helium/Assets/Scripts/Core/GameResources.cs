using UnityEngine;
using System.Collections.Generic;

namespace Helium.Core
{
	public static class GameResources
	{
		public static Dictionary<string, GameObject> ObjectCache = new Dictionary<string, GameObject>();

		public static GameObject Load(string path)
		{
			GameObject go = null;

			if (ObjectCache.ContainsKey(path))
			{
				ObjectCache.TryGetValue(path, out go);
			}
			else
			{
				go = Resources.Load(path) as GameObject;
				ObjectCache.Add(path, go);
			}

			return go;
		}
	}
}
