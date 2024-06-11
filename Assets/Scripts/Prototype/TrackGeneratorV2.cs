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

		private SplineContainer splineContainer;

        private Mesh mesh;

		private float curvatureNormalization;

        private void OnValidate()
		{
			splineContainer = GetComponent<SplineContainer>();
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

		private void GenerateMesh()
		{
			mesh.Clear();

			float length = splineContainer.Spline.GetLength();
			int segments = Mathf.CeilToInt(resolution*length);
			int steps = segments + 1;
			float stepSize = 1f/steps;

			List<Vector3> verts = new();
			List<int> triangles = new();

			bool foldCatch = false;

			List<int> foldIndices = new();
			List<Vector3> foldPositions = new();

			for(int i = 0, index = 0; i <= steps; i++, index += 2)
			{
				float t = stepSize * i;

				splineContainer.Spline.Evaluate(t, out float3 pos, out float3 direction, out float3 normal);
				Vector3 position = pos;

				float curvature = splineContainer.Spline.EvaluateCurvature(t);
				Vector3 curveCenter = splineContainer.Spline.EvaluateCurvatureCenter(t);

				Vector3 right = math.normalizesafe(math.cross(normal, direction));

				int inside = (int)Mathf.Sign(Vector3.Dot(right, curveCenter - position));

				verts.Add(position  -right * width / 2);
				verts.Add(position + right * width / 2);

				if(1/curvature < width)
				{
					// On fold start
					if(foldCatch == false)
					{
						foldCatch = true;
						foldIndices.Clear();
						foldPositions.Clear();
					}

					foldPositions.Add(pos);

					if(inside < 0)
					{
						foldIndices.Add(index);
					}
					else
					{
						foldIndices.Add(index + 1);
					}
				}
				// On fold end
				else if(foldCatch == true)
				{
					foldCatch = false;
					
					Vector3 startPosition = verts[foldIndices[0]];
					Vector3 endPosition = verts[foldIndices.Last()];

					float startHeight = verts[foldIndices[0]].y;
					float endHeight = verts[foldIndices.Last()].y;

					for(int j = 0; j < foldIndices.Count; j++)
					{
						Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, j*1.0f/foldIndices.Count);
						// Debug.DrawLine(verts[foldIndices[j]], newPosition, Color.red, 10);
						verts[foldIndices[j]] = newPosition;
					}
				}

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
		}
	}
}