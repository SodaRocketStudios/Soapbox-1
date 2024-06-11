using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Splines;

namespace Soap.Prototype
{
	public class TrackGeneratorV2 : MonoBehaviour
	{
		[SerializeField, Min(0)] private float slope;
		[SerializeField, Min(0.1f)] private float width;
		[SerializeField, Min(0)] private float maxRoll;
		[SerializeField] private int resolution;
		[SerializeField] private SplineData<float> heightData;

		private SplineContainer splineContainer;

        private Mesh mesh;

		private float curvatureNormalization;

        private void OnValidate()
		{
			splineContainer = GetComponent<SplineContainer>();
		}

		[ContextMenu("Flatten")]
		private void Flatten()
		{
			foreach(Spline spline in splineContainer.Splines)
			{
				for (int i = 1; i < spline.Count; i++)
				{
					BezierKnot knot = spline[i];
					knot.Position.y = 0;
					Quaternion rotation = knot.Rotation;
					Vector3 eulerRotation = new Vector3(0, rotation.eulerAngles.y, 0);
					rotation.eulerAngles = eulerRotation;
					knot.Rotation = rotation;
					spline.SetKnot(i, knot);
				}
			}
		}

		[ContextMenu("Set Slope")]
		private void SetSlope()
        {
			int numberOfCurves = splineContainer.Spline.GetCurveCount();

			for (int i = 1; i < splineContainer.Spline.Count; i++)
			{
				BezierKnot knot = splineContainer.Spline[i];
				knot.Position.y = 0;
				Quaternion rotation = knot.Rotation;
				Vector3 eulerRotation = new Vector3(0, rotation.eulerAngles.y, 0);
				rotation.eulerAngles = eulerRotation;
				knot.Rotation = rotation;
				splineContainer.Spline.SetKnot(i, knot);
			}

			float length = splineContainer.Spline.GetLength();

			float[] lengths = new float[numberOfCurves];
			float[] curvatures = new float[numberOfCurves];
			Vector3[] curveCenters = new Vector3[numberOfCurves];

			for(int i = 0; i < numberOfCurves; i++)
			{

				if(i == 0)
				{
					lengths[i] = splineContainer.Spline.GetCurveLength(i);
				}
				else
				{
					lengths[i] = lengths[i-1] + splineContainer.Spline.GetCurveLength(i);
				}

				float t = lengths[i]/length;

				curvatures[i] = splineContainer.Spline.EvaluateCurvature(t);
				curveCenters[i] = (Vector3)splineContainer.Spline.EvaluateCurvatureCenter(t);

				curvatureNormalization = Mathf.Max(curvatures[i], curvatureNormalization);
			}

            for (int i = 0; i < numberOfCurves; i++)
            {
				float dropPerUnit = Mathf.Sin(-slope * Mathf.Deg2Rad);

                BezierKnot knot = splineContainer.Spline[i + 1];

				float t = lengths[i] / length;

				knot.Position.y = lengths[i] * dropPerUnit;

				curveCenters[i] += Vector3.up*knot.Position.y;

				Quaternion rotation = knot.Rotation;
				Vector3 eulerRotation = rotation.eulerAngles;

				eulerRotation.x = slope;

				Vector3 right = rotation*Vector3.right.normalized;

				float roll = -curvatures[i] * Mathf.Sign(Vector3.Dot(right, curveCenters[i] - (Vector3)knot.Position)) * maxRoll / curvatureNormalization;

				// eulerRotation.z = roll;

				rotation.eulerAngles = eulerRotation;
				knot.Rotation = rotation;

                splineContainer.Spline.SetKnot(i + 1, knot);
            }
        }

		[ContextMenu("Generate Mesh")]
		private void GenerateMesh()
		{
			mesh = new();
			mesh.Clear();

			Spline leftSpline = splineContainer.Splines[0];
			Spline rightSpline = splineContainer.Splines[1];

			float length = Mathf.Max(leftSpline.GetLength(), rightSpline.GetLength());
			int segments = Mathf.CeilToInt(resolution*length);
			int steps = segments + 1;
			float stepSize = 1f/steps;
			

			List<Vector3> verts = new();
			List<int> triangles = new();

			for(int i = 0, index = 0; i <= steps; i++, index += 2)
			{
				float t = stepSize * i;

				leftSpline.Evaluate(t, out float3 leftPosition, out float3 leftDirection, out float3 leftNormal);
				rightSpline.Evaluate(t, out float3 rightPosition, out float3 rightDirection, out float3 rightNormal);

				verts.Add(leftPosition);
				verts.Add(rightPosition);
				Debug.DrawLine(leftPosition, rightPosition, Color.green, 60);

				// Build triangles
				if(i < steps)
				{
					triangles.Add(index + 2);
					triangles.Add(index + 1);
					triangles.Add(index);

					triangles.Add(index + 1);
					triangles.Add(index + 2);
					triangles.Add(index + 3);
				}
			}

			mesh.SetVertices(verts);
			mesh.SetIndices(triangles, MeshTopology.Triangles, 0);

			GetComponent<MeshFilter>().sharedMesh = mesh;
			GetComponent<MeshCollider>().sharedMesh = mesh;
		}
	}
}