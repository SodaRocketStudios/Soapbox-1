using UnityEditor;
using UnityEngine;

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
			// Longitudinal Curve
			longitudinalCurve.ClearKeys();
			longitudinalCurve.AddKey(0, 0);
			longitudinalCurve.AddKey(peakSlipRatio, peakLongitudinalFriction);
			longitudinalCurve.AddKey(1, longitudinalSlipFriction);
			AnimationUtility.SetKeyRightTangentMode(longitudinalCurve, 0, AnimationUtility.TangentMode.Linear);
			AnimationUtility.SetKeyRightTangentMode(longitudinalCurve, 1, AnimationUtility.TangentMode.ClampedAuto);

			// Lateral Curve
			lateralCurve.ClearKeys();
			lateralCurve.AddKey(0, 0);
			lateralCurve.AddKey(peakSlipAngle, peakLateralFriction);
			lateralCurve.AddKey(1, lateralSlipFriction);
			AnimationUtility.SetKeyRightTangentMode(lateralCurve, 0, AnimationUtility.TangentMode.Linear);
			AnimationUtility.SetKeyRightTangentMode(lateralCurve, 1, AnimationUtility.TangentMode.ClampedAuto);
		}
	}
}
