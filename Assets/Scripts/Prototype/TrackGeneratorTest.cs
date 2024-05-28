using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

namespace Soap.Prototype
{
	public class TrackGeneratorTest : MonoBehaviour
	{
		[SerializeField] private SplineContainer splineContainer;

		[Tooltip("The slope of the track in degrees")]
		[SerializeField] private float slope;

		private void Start()
        {
            SetSlope();
        }

        private void SetSlope()
        {
			float dropPerUnit = Mathf.Sin(slope*Mathf.Deg2Rad);

            float lengthSum = 0;

            for (int i = 0; i < splineContainer.Spline.GetCurveCount(); i++)
            {
                BezierKnot knot = splineContainer.Spline[i + 1];
                lengthSum += splineContainer.Spline.GetCurveLength(i);

                knot.Position += new float3(0, -dropPerUnit * lengthSum, 0);
                splineContainer.Spline.SetKnot(i + 1, knot);
            }
        }
    }
}
