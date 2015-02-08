using UnityEngine;
using System.Collections;

namespace Hydrogen.Entities {

	public class FadeScreen : MonoBehaviour {

		public float fadeTime = 5f;

		private bool isFading = false;
		private Color targetColor;

		private void Awake()
		{
			this.guiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
			this.guiTexture.color = Color.clear;
			this.targetColor = Color.clear;
		}

		private void Update()
		{
			if (! this.isFading) return;

			if (this.guiTexture.color.a > 0.9)
			{
				this.guiTexture.color = this.targetColor;
				this.isFading = false;
				return;
			}

			float lerpTime = Time.deltaTime * this.fadeTime;

			this.guiTexture.color = Color.Lerp(this.guiTexture.color, this.targetColor, lerpTime);
		}

		public void FadeToTarget(Color targetColor)
		{
			this.targetColor = targetColor;
			this.isFading = true;
		}

		public bool IsFading()
		{
			return this.isFading;
		}

	}

}
