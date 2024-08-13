using UnityEngine;
using UnityEngine.UI;

namespace SRS.UI
{
	public class ImageSwapper : MonoBehaviour
	{
		private Image image;

		private void Awake()
		{
			image = GetComponent<Image>();
		}

		public void SetImage(Sprite sprite)
		{
			image.sprite = sprite;
		}

		public void SetImage(Sprite sprite, Color color)
		{
			image.sprite = sprite;
			image.color = color;
		}
	}
}
