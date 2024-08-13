using UnityEngine;
using UnityEditor;
using SRS.Extensions.Curves;
using System;

namespace Soap.Physics
{
	public class MGUK : MonoBehaviour
	{
		[Header("Recharge Parameters")]
		[SerializeField] private float dischargeRate;
		[SerializeField] private float maxRechargeRate;
		[SerializeField, Min(1)] private float maxRechargeSpeed = 85;
		[SerializeField] private float rollingResistanceTorque;


		[Header("Deploy Parameters")]
		[SerializeField] private float launchTorque;

		[SerializeField] private float peakTorque;
		[SerializeField] private float peakVelocity;

		[SerializeField] private float maxVelocity;
		[SerializeField] private float endTorque;

		[SerializeField, Curve(5)] private AnimationCurve torqueCurve;

		private const float MAX_CHARGE = 100;

		private float charge = MAX_CHARGE;
		public float ChargeAmount
		{
			get => charge/MAX_CHARGE;
		}

		private bool depleted = false;

		private Rigidbody carRigidbody;

		private void Start()
		{
			carRigidbody = GetComponent<Rigidbody>();
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			int peakIndex;

			torqueCurve.ClearKeys();

			torqueCurve.AddKey(0, launchTorque);

			peakIndex = torqueCurve.AddKey(peakVelocity, peakTorque);

			torqueCurve.AddKey(maxVelocity, endTorque);

			AnimationUtility.SetKeyLeftTangentMode(torqueCurve, peakIndex, AnimationUtility.TangentMode.ClampedAuto);
		}
#endif

		public float Deploy(float inputValue)
		{
			if(depleted)
			{
				float torque = Recharge();
				depleted = true;
				return torque;
			}

			if(charge <= 0)
			{
				depleted = true;
				charge = Mathf.Max(charge, 0);
				return 0;
			}

			float speed = Mathf.Abs(Vector3.Dot(carRigidbody.velocity, transform.forward));

			charge -= dischargeRate*Time.deltaTime*inputValue;
			return torqueCurve.Evaluate(speed)*inputValue;
		}

		public float Recharge()
		{
			if(charge >= MAX_CHARGE)
			{
				return 0;
			}

			float speed = Mathf.Abs(Vector3.Dot(carRigidbody.velocity, transform.forward));
			
			float rechargeAmount = maxRechargeRate*speed/maxRechargeSpeed;
			rechargeAmount = Math.Min(rechargeAmount, maxRechargeRate);

			charge = Mathf.Min(charge + rechargeAmount*Time.deltaTime, MAX_CHARGE);

			depleted = false;

			return -rollingResistanceTorque*speed/maxVelocity;
		}

		public void Reset()
		{
			charge = MAX_CHARGE;
		}
	}
}
