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

		private bool isActive = false;

		private void Start()
		{
			carRigidbody = GetComponent<Rigidbody>();
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			// Keyframe tempKey;

			int peakIndex;

			torqueCurve.ClearKeys();

			torqueCurve.AddKey(0, launchTorque);

			peakIndex = torqueCurve.AddKey(peakVelocity, peakTorque);

			torqueCurve.AddKey(maxVelocity, endTorque);

			AnimationUtility.SetKeyLeftTangentMode(torqueCurve, peakIndex, AnimationUtility.TangentMode.ClampedAuto);
		}
#endif

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
			float velocity = Vector3.Dot(carRigidbody.velocity, transform.forward);

			isActive = true;

			if(charge <= 0)
			{
				return 0;
			}

			Debug.Log(velocity);

			return torqueCurve.Evaluate(velocity)*inputValue;
		}
	}
}
