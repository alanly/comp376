using UnityEngine;
using System.Collections;

namespace Hydrogen.Core {

	public class GameController : ScriptableObject {

		/**
		 * Singleton Instance
		 */

		private static GameController _instance;

		public static GameController instance
		{
			get { return _instance; }
		}


		/**
		 * Class Body
		 */

		public int        TotalNumberOfLives = 3;
		public int        TotalNumberOfAtoms = 32;
		public GameObject BoundaryBox;
		public GameObject ScoreText;
		public GameObject LifeText;
		public GameObject GameOverText;

		private EntityManager   _entities;
		private BoundaryManager _boundaries;
		private GameObject      _player;
		private int             _score;
		private int             _remainingLives;
		private int             _remainingAtoms;
		private bool            _gameOver = false;

		private void Awake()
		{
			// Assign the GameController instance
			GameController._instance = this;

			// Create an EntityManager instance
			this._entities = new EntityManager();

			// Setup counters
			this._remainingLives = this.TotalNumberOfLives;
			this._remainingAtoms = this.TotalNumberOfAtoms;
		}

		private void Start()
		{
			/**
			 * Setup the world.
			 */

			// Get the Player instance.
			this._player = GameObject.FindGameObjectWithTag("Player");

			// Get the BoundaryManager instance
			this._boundaries = BoundaryBox.GetComponent<BoundaryManager>();

			// Spawn a new atom
			AtomController.instance.SpawnNewAtom();
		}

		public EntityManager entities {
			get { return this._entities; }
		}

		public BoundaryManager boundaries {
			get { return this._boundaries; }
		}

		public GameObject player {
			get { return this._player; }
		}

		public int score {
			get { return this._score; }
		}

		public int AddScore(int points)
		{
			this._score += points;

			this.ScoreText.guiText.text = "SCORE: " + this._score;

			return this._score;
		}

		public void PizzarinoDeath()
		{
			this._remainingLives = 0;
		}

		public void PlayerDied()
		{
			this._remainingLives--;

			if (this._remainingLives < 0) this._remainingLives = 0;

			this.LifeText.guiText.text = "  LIVES: " + this._remainingLives;

			if (this._remainingLives == 0)
			{
				this.GameOverText.guiText.text = "GAME OVER";
				this.GameOverText.guiText.enabled = true;
				this._gameOver = true;
				this._player.SendMessage("DisableInput");
				this._player.SendMessage("DisableCollider");
				return;
			}

			this.ReloadPlayer();
		}

		public void AtomHasBeenHit()
		{
			AtomController.instance.SpawnNewAtom();
		}

		public void AtomHasDied()
		{
			if (--this._remainingAtoms == 0)
			{
				this.GameOverText.guiText.text = "YOU ARE WINNER!";
				this.GameOverText.guiText.enabled = true;
				this._gameOver = true;
				this._player.SendMessage("DisableInput");
				this._player.SendMessage("DisableCollider");
				return;
			}

			int numOfAtomsKilled = this.TotalNumberOfAtoms - this._remainingAtoms;

			Debug.Log (((float)numOfAtomsKilled / this.TotalNumberOfAtoms) + " of atoms killed.");

			int firstWave = (int) (this.TotalNumberOfAtoms * 0.3);
			int secondWave = (int) (this.TotalNumberOfAtoms * 0.6);
			int thirdWave = (int) (this.TotalNumberOfAtoms * 0.9);

			if (numOfAtomsKilled == firstWave || numOfAtomsKilled == secondWave || numOfAtomsKilled == thirdWave)
			{
				AtomController.instance.SpawnNewHotAtomBalloon();
				return;
			}

			AtomController.instance.SpawnNewAtom();
		}

		private void ReloadPlayer()
		{
			this._player.SendMessage("Restart");
		}

	}

}
