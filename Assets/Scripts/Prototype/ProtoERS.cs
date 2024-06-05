using UnityEngine;
using UnityEditor;
using SRS.Extensions.Curves;

namespace Soap.Prototype
{
	public class ProtoERS : MonoBehaviour
	{
		[SerializeField] private float dischargeRate;

		[SerializeField] private float launchTorque;

		[SerializeField] private float peakTorque;
		[SerializeField] private float peakVelocity;

		[SerializeField] private float maxVelocity;
		[SerializeField] private float endTorque;

		[SerializeField, Curve(5)] private AnimationCurve torqueCurve;

		private Rigidbody carRigidbody;

		private float charge = 100;
		public float chargeAmount
		{
			get => charge/100;
		}

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

		public float UseERS(float inputValue)
		{
			if(inputValue == 0)
			{
				charge += 5*Time.deltaTime;
				charge = Mathf.Min(charge, 100);
				return 0;
			}

			if(charge <= 0)
			{
				charge = Mathf.Max(charge, 0);
				return 0;
			}

			float velocity = Vector3.Dot(carRigidbody.velocity, transform.forward);

			charge -= dischargeRate*Time.deltaTime*inputValue;
			Debug.Log($"{Time.realtimeSinceStartup}: {charge}");
			return torqueCurve.Evaluate(velocity)*inputValue;
		}
	}
}
