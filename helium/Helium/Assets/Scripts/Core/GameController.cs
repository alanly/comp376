using UnityEngine;
using System.Collections;
using Helium.Entities;

namespace Helium.Core
{
	public class GameController : ScriptableObject
	{
		public const int   MAX_BALLOONS    = 36;
		public const int   INITIAL_LIVES   = 3;
		public const float SPEED_UP_FACTOR = 3f;

		public  static GameObject Player;
		public  static GameObject Score;
		public  static GameObject LivesCounter;
		public  static GameObject ClusterController;
		public  static bool       GameEnabled;
		public  static bool       HasSpeedUp;
		public  static bool       UsedSpecialShoot;
		private static int        currentScore;
		private static int        remainingLives;
		private static int        activeClusterCount;
		private static int        spawnedMasterClusterCount;
		private static int        activeBalloonCount;
		private static int        poppedBalloonCount;
		private static bool[]     hotAirBalloonStatus;

		void Awake()
		{
			GameEnabled = true;
			ClusterController = GameObject.FindGameObjectWithTag("BalloonClusterController");
			hotAirBalloonStatus = new bool[3];
		}

		void Start()
		{
			this.LoadWorld();

			// Configure the default values.
			GameEnabled         = true;
			HasSpeedUp          = false;
			UsedSpecialShoot    = false;
			currentScore        = 0;
			remainingLives      = INITIAL_LIVES;
			activeClusterCount  = 0;
			spawnedMasterClusterCount = 0;
			activeBalloonCount  = 0;
			poppedBalloonCount  = 0;
		}

		void FixedUpdate()
		{
			if (! GameEnabled) return;

			// Handle the event that all balloons have been popped.
			if (PercentageOfBalloonsDestroyed() >= 1f)
			{
				// Destroy the player.
				GameObject player = GameObject.FindGameObjectWithTag("Player");
				Destroy(player);

				// Create the GameOver screen.
				GameObject gameOver = Instantiate(GameResources.Load("Prefabs/GameOver")) as GameObject;
				gameOver.transform.position = new Vector2(0, 0);
				gameOver.GetComponent<TextMesh>().text = "YOU WIN. PRESS \"ESC\" FOR A NEW GAME.";

				// Disable the game.
				GameEnabled = false;
			}

			// Create hot air balloons at fixed intervals.
			this.HandleHotAirBalloons();

			// If 80% of balloons have been popped, speed up clusters/balloons
			if (! GameController.HasSpeedUp && PercentageOfBalloonsDestroyed() > 0.8f)
			{
				GameController.HasSpeedUp = true;
				this.SpeedUp();
			}

			// Create a cluster if there are none on screen and we still have more to make.
			if (activeClusterCount < 1 && (BalloonClusterController.BALLOONS_PER_CLUSTER * spawnedMasterClusterCount) < MAX_BALLOONS)
			{
				ClusterController.SendMessage("MakeClusterAndBalloons");
				++spawnedMasterClusterCount;
			}
		}
		
		private void LoadWorld()
		{
			// Add the Score counter.
			this.LoadScoreCounter();

			// Add the Lives counter
			this.LoadLivesCounter();

			// Add the Player object.
			LoadPlayer();
		}

		private void LoadScoreCounter()
		{
			// Load the prefab resource.
			Object scorePrefab = GameResources.Load("Prefabs/Score");

			// Instantiate the prefab at the appropriate location.
			Score = GameObject.Instantiate(scorePrefab) as GameObject;
			Score.transform.position = new Vector3(0, 4.5f, -5f);
		}

		private void LoadLivesCounter()
		{
			// Load the prefab resource.
			Object scorePrefab = GameResources.Load("Prefabs/LivesCounter");

			// Instantiate the prefab at the appropriate location.
			LivesCounter = GameObject.Instantiate(scorePrefab) as GameObject;
			LivesCounter.transform.position = new Vector3(9.5f, 4.5f, -5f);

			GameController.UpdateLivesCounter();
		}

		private static void LoadPlayer()
		{
			// Load the prefab
			Object player = GameResources.Load("Prefabs/Player");

			// Instantiate player in center of screen.
			Player = GameObject.Instantiate(player) as GameObject;
			Player.transform.position = new Vector3(0f, 0f, 1f);
		}

		public static int GetCurrentScore()
		{
			return currentScore;
		}

		public static int GetRemainingLives()
		{
			return remainingLives;
		}

		public static void IncrementScore(int points)
		{
			// Increment the score count.
			currentScore += points;
			
			// Format the score into a string
			string scoreString = ""+currentScore;
			scoreString = scoreString.PadLeft(4, '0');

			TextMesh scoreText = Score.GetComponent<TextMesh>();
			scoreText.text = scoreString;
		}

		public static void UpdateLivesCounter()
		{
			string livesString = "LIVES    "+remainingLives;
			TextMesh livesText = LivesCounter.GetComponent<TextMesh>();
			livesText.text = livesString;
		}

		public static void PlayerDied()
		{
			// Decrement lives remaining.
			remainingLives--;

			UpdateLivesCounter();

			if (remainingLives == 0)
			{
				GameObject gameOver = Instantiate(GameResources.Load("Prefabs/GameOver")) as GameObject;
				gameOver.transform.position = new Vector2(0, 0);
				GameEnabled = false;
			}
			else
			{
				LoadPlayer();
			}
		}

		public static float PercentageOfBalloonsDestroyed()
		{
			return ((float)poppedBalloonCount / MAX_BALLOONS);
		}

		public static void AddBalloonDestroyed()
		{
			++poppedBalloonCount;
		}

		public static void IncrementBalloonCounter()
		{
			++activeBalloonCount;
		}

		public static void IncrementClusterCounter()
		{
			++activeClusterCount;
		}

		public static void DecrementClusterCounter()
		{
			--activeClusterCount;
		}

		protected void SpeedUp()
		{
			// Speed up cluster
			GameObject[] clusters = GameObject.FindGameObjectsWithTag("BalloonCluster");
			foreach (GameObject cluster in clusters)
			{
				cluster.rigidbody2D.velocity *= SPEED_UP_FACTOR;
			}

			// Speed up un-clustered balloons
			GameObject[] balloons = GameObject.FindGameObjectsWithTag("Balloon");
			foreach (GameObject balloon in balloons)
			{
				if (balloon.transform.parent != null) continue;

				balloon.rigidbody2D.velocity *= SPEED_UP_FACTOR;
			}
		}

		protected void CreateHotAirBalloon()
		{
			Camera camera = Camera.main;
			GameObject hab = Instantiate(GameResources.Load("Prefabs/HotAirBalloon")) as GameObject;

			// Determine which side the HAB comes in from.
			float hAxis = Random.value > 0.5f ? 1.1f : -0.1f;
			float yAxis = 0.85f;
			Vector3 transformedPosition = camera.ViewportToWorldPoint(new Vector3(hAxis, yAxis, camera.nearClipPlane));
			hab.transform.position = new Vector2(transformedPosition.x, transformedPosition.y);

			Debug.Log("Spawn HAB at " + hab.transform.position);

			// Set the velocity
			float hTarget = hAxis > 0.5f ? -1f : 1f;
			hab.rigidbody2D.velocity = new Vector2(hTarget, 0);
		}

		protected void HandleHotAirBalloons()
		{
			// Create first HAB at 30% progress
			if (! hotAirBalloonStatus[0] && PercentageOfBalloonsDestroyed() >= 0.3f)
			{
				this.CreateHotAirBalloon();
				hotAirBalloonStatus[0] = true;
			}
			
			// Create second HAB at 60% progress
			if (! hotAirBalloonStatus[1] && PercentageOfBalloonsDestroyed() >= 0.6f)
			{
				this.CreateHotAirBalloon();
				hotAirBalloonStatus[1] = true;
			}

			// Create third HAB at 90% progress
			if (! hotAirBalloonStatus[2] && PercentageOfBalloonsDestroyed() >= 0.9f)
			{
				this.CreateHotAirBalloon();
				hotAirBalloonStatus[2] = true;
			}
		}
	}
}
