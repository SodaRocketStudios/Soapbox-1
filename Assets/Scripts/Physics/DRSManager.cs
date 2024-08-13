using UnityEngine;
using SRS.UI;

namespace Soap.Physics
{
	public class DRSManager : MonoBehaviour
	{
		[SerializeField] private ImageSwapper imageSwapper;

		[SerializeField] private Sprite activeImage;
		[SerializeField] private Sprite availableImage;
		[SerializeField] private Sprite inactiveImage;

		private bool isActive;
		private bool isAvailable;

		private AeroSurface[] aeroSurfaces;

		private void Awake()
		{
			aeroSurfaces = GetComponentsInChildren<AeroSurface>();
		}

		public void ToggleDRS()
		{
			SetActive(!isActive);
		}

		public void SetActive(bool active)
		{
			isActive = active;
			if(isAvailable)
			{
				foreach(AeroSurface surface in aeroSurfaces)
				{
					surface.SetDRS(true);

					if(isActive)
					{
						imageSwapper.SetImage(activeImage);
					}
					else
					{
						imageSwapper.SetImage(availableImage);
					}
				}
			}
			else
			{
				imageSwapper.SetImage(inactiveImage);
			}
		}

		public void SetAvailable(bool available)
		{
			isAvailable = available;
			if(isAvailable)
			{
				imageSwapper.SetImage(availableImage);
			}
			else
			{
				SetActive(false);
				imageSwapper.SetImage(inactiveImage);
			}
		}
	}
}
