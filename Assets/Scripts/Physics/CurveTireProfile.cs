using System;
using UnityEditor;
using UnityEngine;
using SRS.Extensions.EditorExtensions;

namespace Soap.Physics
{
	[CreateAssetMenu(fileName = "New Tire Profile", menuName = "Physics/Curve Tire Profile")]
	public class CurveTireProfile : ScriptableObject
	{
		[Header("Longitudinal Parameters")]
		[SerializeField, Range(1, 3)] private float peakLongitudinalFriction = 1.75f;
		[SerializeField, Range(0.01f, 0.99f)] private float peakSlipRatio = 0.1f;
		[SerializeField, Range(1, 3)] private float longitudinalSlipFriction = 1.5f;
		[SerializeField] private AnimationCurve longitudinalCurve = new AnimationCurve();

		[SerializeField, Vector2Range(0.01f, 0.99f, 0, 1)] Vector2 longitudinalShapingKey;

		[Header("Lateral Parameters")]
		[SerializeField, Range(1, 3)] private float peakLateralFriction = 1.65f;
		[SerializeField, Range(0.1f, 40)] private float peakSlipAngle = 7.5f;
		[SerializeField, Range(1, 3)] private float lateralSlipFriction = 1.35f;
		[SerializeField, Range(0, 45)] private float maxSlipAngle = 25;
		[SerializeField] private AnimationCurve lateralCurve = new AnimationCurve();
		[SerializeField, Vector2Range(0.01f, 0.99f, 0, 1)] Vector2 lateralShapingKey;

		private void Awake()
		{
			OnValidate();
		}

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

		public float EvaluateLongitudinal(float slipRatio)
		{
			slipRatio = Mathf.Clamp(slipRatio, -1, 1);
			return longitudinalCurve.Evaluate(slipRatio);
		}

		public float EvaluateLateral(float slipAngle)
		{
			slipAngle = Mathf.Clamp(slipAngle, -maxSlipAngle, maxSlipAngle);
			return lateralCurve.Evaluate(slipAngle);
		}
	}
}
