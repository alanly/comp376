using UnityEngine;
using System.Collections;

namespace Helium.Core
{
	public class GameOver : MonoBehaviour
	{
		void Update()
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				GameObject gameCtrl = GameObject.FindGameObjectWithTag("GameController");
				Destroy(gameCtrl);
				Instantiate(GameResources.Load("Prefabs/GameController"));

				GameObject clusterCtrl = GameObject.FindGameObjectWithTag("BalloonClusterController");
				Destroy(clusterCtrl);
				Instantiate(GameResources.Load("Prefabs/BalloonClusterController"));

				Application.LoadLevel("Game");
			}
		}
	}
}
