using UnityEngine;

namespace Soap.Physics
{
	public class DRSManager : MonoBehaviour
	{
		private bool isActive;
		private bool isAvailable;

		private AeroSurface[] aeroSurfaces;

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
						// Set active sprite
					}
					else
					{
						// Set available sprite
					}
				}
				// Set UI
			}
			else
			{
				// set inactive sprite.
			}
		}

		public void SetAvailable(bool available)
		{
			isAvailable = available;
			if(isAvailable)
			{
				// Set available sprite
			}
			else
			{
				SetActive(false);
				// Set inactive sprite
			}
		}
	}
}
