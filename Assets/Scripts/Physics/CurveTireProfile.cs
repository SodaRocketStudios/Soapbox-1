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

		[SerializeField, Vector2Range(0.01f, 0.99f, 0, 1)] Vector2 shapingKeyLocation;

		[Header("Lateral Parameters")]
		[SerializeField, Range(1, 3)] private float peakLateralFriction = 1.65f;
		[SerializeField, Range(0.1f, 40)] private float peakSlipAngle = 7.5f;
		[SerializeField, Range(1, 3)] private float lateralSlipFriction = 1.35f;
		[SerializeField] private AnimationCurve lateralCurve = new AnimationCurve();

		private void Awake()
		{
			OnValidate();
		}

		private void OnValidate()
		{
			Keyframe tempKey;
			
			// Longitudinal Curve -----------------------------------------------------------------
			longitudinalCurve.ClearKeys();

			// Origin
			longitudinalCurve.AddKey(0, 0);

            // Peak
           	int peakKeyIndex = longitudinalCurve.AddKey(peakSlipRatio, peakLongitudinalFriction);

			// Shaping key
			int shapingKeyIndex = longitudinalCurve.AddKey(peakSlipRatio + (1-peakSlipRatio)*shapingKeyLocation.x, longitudinalSlipFriction + (peakLongitudinalFriction - longitudinalSlipFriction)*shapingKeyLocation.y);

			// Max slip ratio
			longitudinalCurve.AddKey(1, longitudinalSlipFriction);

			if(shapingKeyIndex > 0 && shapingKeyIndex < longitudinalCurve.length)
			{
				AnimationUtility.SetKeyLeftTangentMode(longitudinalCurve, shapingKeyIndex, AnimationUtility.TangentMode.ClampedAuto);
			}
			else
			{
				Debug.LogError($"Shaping key index {shapingKeyIndex}, was out of range. Expected range is 0 to {longitudinalCurve.length}");
			}

			
			// Adjust shape
			tempKey = longitudinalCurve[peakKeyIndex];
			tempKey.inTangent = 0;
			tempKey.outTangent = 0;
			// tempKey.outWeight = shapingKeyLocation*2;
			tempKey.weightedMode = WeightedMode.Both;
			longitudinalCurve.MoveKey(peakKeyIndex, tempKey);
			// Keyframe tempKey = longitudinalCurve[shapingKeyIndex];
			// tempKey.inTangent = 0;
			// tempKey.outTangent = 0;
			// tempKey.outWeight = shape;
			// tempKey.inWeight = shape;
			// tempKey.weightedMode = WeightedMode.Both;
			// longitudinalCurve.MoveKey(shapingKeyIndex, tempKey);


			// Lateral Curve ----------------------------------------------------------------------
			lateralCurve.ClearKeys();
			lateralCurve.AddKey(0, 0);
			lateralCurve.AddKey(peakSlipAngle, peakLateralFriction);
			lateralCurve.AddKey(1, lateralSlipFriction);
			AnimationUtility.SetKeyRightTangentMode(lateralCurve, 0, AnimationUtility.TangentMode.Linear);
			AnimationUtility.SetKeyRightTangentMode(lateralCurve, 1, AnimationUtility.TangentMode.ClampedAuto);
		}
	}
}
