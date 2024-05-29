using UnityEngine;

namespace Soap.Prototype
{
	public class ProtoERS : MonoBehaviour
	{
		[SerializeField] private float dischargeRate;

		private float charge = 100;

		private bool isActive = false;

		private void Update()
		{
			if(charge <= 0)
			{
				charge = Mathf.Max(charge, 0);
				isActive = false;
			}

			if(isActive)
			{
				charge -= dischargeRate*Time.deltaTime;
			}

			charge += 1*Time.deltaTime;
		}

		public bool UseERS()
		{
			isActive = true;
			
			if(charge <= 0)
			{
				return false;
			}

			return true;
		}
	}
}
