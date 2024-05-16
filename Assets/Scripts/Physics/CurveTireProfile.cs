using UnityEditor;
using UnityEngine;

namespace Soap.Physics
{
	[CreateAssetMenu(fileName = "New Tire Profile", menuName = "Physics/Curve Tire Profile")]
	public class CurveTireProfile : ScriptableObject
	{
		[Header("Longitudinal Parameters")]
		[SerializeField, Range(1, 2)] private float peakFriction;
		[SerializeField, Range(0.01f, 0.99f)] private float peakSlip;
		[SerializeField, Range(1, 2)] private float slipFriction;
		[SerializeField] private AnimationCurve longitudinalCurve;

		[SerializeField] private AnimationCurve lateralCurve;

		private void OnValidate()
		{
			longitudinalCurve.ClearKeys();
			longitudinalCurve.AddKey(0, 0);
			longitudinalCurve.AddKey(peakSlip, peakFriction);
			longitudinalCurve.AddKey(1, slipFriction);
			AnimationUtility.SetKeyBroken(longitudinalCurve, 0, true);
			AnimationUtility.SetKeyRightTangentMode(longitudinalCurve, 0, AnimationUtility.TangentMode.Linear);
			AnimationUtility.SetKeyRightTangentMode(longitudinalCurve, 1, AnimationUtility.TangentMode.ClampedAuto);
		}
	}
}
