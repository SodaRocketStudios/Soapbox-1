using System;
using UnityEngine;
using UnityEditor;
using SRS.Extensions.EditorExtensions;

namespace SRS.Soap.Physics
{
	[CreateAssetMenu(fileName = "New Tire Profile", menuName = "Physics/Curve Tire Profile")]
	public class CurveTireProfile : ScriptableObject
	{
		[Header("Longitudinal Parameters")]
		[SerializeField, Range(1, 3)] private float peakLongitudinalFriction = 1.75f;
		[SerializeField, Range(0.01f, 0.99f)] private float peakSlipRatio = 0.1f;
		[SerializeField, Range(1, 3)] private float longitudinalSlipFriction = 1.5f;
		#if UNITY_EDITOR
		[SerializeField, Vector2Range(0.01f, 0.99f, 0, 1)] Vector2 longitudinalShapingKey;
		#endif
		[SerializeField] private AnimationCurve longitudinalCurve = new AnimationCurve();


		[Header("Lateral Parameters")]
		[SerializeField, Range(1, 3)] private float peakLateralFriction = 1.65f;
		[SerializeField, Range(0.1f, 40)] private float peakSlipAngle = 7.5f;
		[SerializeField, Range(1, 3)] private float lateralSlipFriction = 1.35f;
		[SerializeField, Range(0, 45)] private float maxSlipAngle = 25;
		#if UNITY_EDITOR
		[SerializeField, Vector2Range(0.01f, 0.99f, 0, 1)] Vector2 lateralShapingKey;
		#endif
		[SerializeField] private AnimationCurve lateralCurve = new AnimationCurve();

		#if UNITY_EDITOR
		private void OnValidate()
		{
			Keyframe tempKey;
			int peakKeyIndex;
			int shapingKeyIndex;
			
			// Longitudinal Curve -----------------------------------------------------------------
			longitudinalCurve.ClearKeys();

			// Origin
			longitudinalCurve.AddKey(0, 0);

            // Peak
           	peakKeyIndex = longitudinalCurve.AddKey(peakSlipRatio, peakLongitudinalFriction);

			// Shaping key
			shapingKeyIndex = longitudinalCurve.AddKey(peakSlipRatio + (1-peakSlipRatio)*longitudinalShapingKey.x, longitudinalSlipFriction + (peakLongitudinalFriction - longitudinalSlipFriction)*longitudinalShapingKey.y);

			// Max slip ratio
			longitudinalCurve.AddKey(1, longitudinalSlipFriction);

			AnimationUtility.SetKeyLeftTangentMode(longitudinalCurve, shapingKeyIndex, AnimationUtility.TangentMode.ClampedAuto);

			tempKey = longitudinalCurve[peakKeyIndex];
			tempKey.inTangent = 0;
			tempKey.outTangent = 0;
			longitudinalCurve.MoveKey(peakKeyIndex, tempKey);

			// Lateral Curve ----------------------------------------------------------------------
			lateralCurve.ClearKeys();

			// Origin
			lateralCurve.AddKey(0, 0);

            // Peak
           	peakKeyIndex = lateralCurve.AddKey(peakSlipAngle, peakLateralFriction);

			// Shaping key
			shapingKeyIndex = lateralCurve.AddKey(peakSlipAngle + (maxSlipAngle-peakSlipAngle)*lateralShapingKey.x, lateralSlipFriction + (peakLateralFriction - lateralSlipFriction)*lateralShapingKey.y);

			// Max slip angle
			lateralCurve.AddKey(maxSlipAngle, longitudinalSlipFriction);

			AnimationUtility.SetKeyLeftTangentMode(lateralCurve, shapingKeyIndex, AnimationUtility.TangentMode.ClampedAuto);

			tempKey = lateralCurve[peakKeyIndex];
			tempKey.inTangent = 0;
			tempKey.outTangent = 0;
			lateralCurve.MoveKey(peakKeyIndex, tempKey);
		}
		#endif

		public float EvaluateLongitudinal(float slipRatio)
		{
			float sign = Mathf.Sign(slipRatio);
			slipRatio = Mathf.Abs(slipRatio);
			slipRatio = Mathf.Clamp01(slipRatio);
			return sign*longitudinalCurve.Evaluate(slipRatio);
		}

		public float EvaluateLateral(float slipAngle)
		{
			float sign = Mathf.Sign(slipAngle);
			slipAngle = Mathf.Abs(slipAngle);
			slipAngle = Mathf.Clamp(slipAngle, 0, maxSlipAngle);
			return sign*lateralCurve.Evaluate(slipAngle);
		}
	}
}
