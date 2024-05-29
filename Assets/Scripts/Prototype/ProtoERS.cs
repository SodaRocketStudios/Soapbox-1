using UnityEngine;

namespace Soap.Prototype
{
	public class ProtoERS : MonoBehaviour
	{
		[SerializeField] private float dischargeRate;

		[SerializeField] private float torque;

		private float charge = 100;

		private bool isActive = false;

		private void Update()
		{
			if(charge <= 0)
			{
				charge = Mathf.Max(charge, 0);
				return;
			}

			if(isActive)
			{
				charge -= dischargeRate*Time.deltaTime;
			}

			charge += 10*Time.deltaTime;
		}

		public float UseERS(float inputValue)
		{
			isActive = true;

			if(charge <= 0)
			{
				return 0;
			}

			return torque*inputValue;
		}
	}
}
