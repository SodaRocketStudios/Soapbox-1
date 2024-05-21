using UnityEditor;
using UnityEngine;

namespace SRS.Soap.Prototype
{
	public class CurveTest : MonoBehaviour
	{
		[SerializeField] private Vector2 peak = new Vector2();
		[SerializeField] private float muSlip;
		[SerializeField] private Vector2 shapingKey;
		[SerializeField] private AnimationCurve curve;

		private void OnValidate()
		{
			curve.ClearKeys();
			curve.AddKey(0, 0);
			curve.AddKey(peak.x, peak.y);
			curve.AddKey(shapingKey.x, shapingKey.y);
			curve.AddKey(1, muSlip);
			AnimationUtility.SetKeyLeftTangentMode(curve, 0, AnimationUtility.TangentMode.Linear);
			AnimationUtility.SetKeyLeftTangentMode(curve, 1, AnimationUtility.TangentMode.ClampedAuto);
			AnimationUtility.SetKeyLeftTangentMode(curve, 2, AnimationUtility.TangentMode.Free);
			AnimationUtility.SetKeyLeftTangentMode(curve, 3, AnimationUtility.TangentMode.ClampedAuto);
		}
	}
}
