using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Splines;
#endif

namespace Soap.Prototype
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshCollider))]
	[RequireComponent(typeof(SplineContainer), typeof(MeshFilter), typeof(MeshRenderer))]
	public class TrackGeneratorTest : MonoBehaviour
	{
		[SerializeField] private SplineContainer splineContainer;

		[Tooltip("The slope of the track in degrees")]
		[SerializeField] private float slope;

		[SerializeField] private float maxRoll = 0;

		[SerializeField] private float resolution;

		[SerializeField] private float width = 1;

		private Mesh mesh;

		private float curvatureNormaization;

		private void OnValidate()
		{
			if(mesh == null)
			{
				mesh = new Mesh();
			}

			UpdateSpline();
		}

		private void OnEnable()
		{
			mesh = new();

#if UNITY_EDITOR
			EditorSplineUtility.AfterSplineWasModified += OnSplineModified;
			Undo.undoRedoPerformed += UpdateSpline;
#endif

			UpdateSpline();
		}

		private void OnDisable()
		{
			if (mesh != null)
#if  UNITY_EDITOR
                DestroyImmediate(mesh);
#else
                Destroy(mesh);
#endif

#if UNITY_EDITOR
			EditorSplineUtility.AfterSplineWasModified -= OnSplineModified;
			Undo.undoRedoPerformed -= UpdateSpline;
#endif
		}

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

				curvatureNormaization = Mathf.Max(curvatures[i], curvatureNormaization);
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

				float roll = -curvatures[i] * Mathf.Sign(Vector3.Dot(right, curveCenters[i] - (Vector3)knot.Position)) * maxRoll / curvatureNormaization;

				eulerRotation.z = roll;

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

			int index = 0;

			int side = 1;

			Vector3 startPosition = new();

			for(int i = 0; i <= steps; i++)
			{
				float t = stepSize * i;

				splineContainer.Spline.Evaluate(t, out float3 pos, out float3 direction, out float3 normal);
				Vector3 position = pos;

				float curvature = splineContainer.Spline.EvaluateCurvature(t);
				Vector3 curveCenter = splineContainer.Spline.EvaluateCurvatureCenter(t);

				Vector3 right = math.normalizesafe(math.cross(normal, direction));

				if(1/curvature < width)
				{
					if(foldCatch == false)
					{
						// fold start
						Debug.Log(right);
						Debug.DrawRay(position, right*50, Color.green, 20);
						foldCatch = true;
						foldIndices.Clear();
						side = (int)Mathf.Sign(Vector3.Dot(right, curveCenter - position));
						startPosition = position + side * right * width / 2;
					}

					verts.Add(position - side * right * width / 2);
					foldIndices.Add(index);
					index++;

					// Debug.DrawLine(position, position + side*right*width, Color.magenta, 20);
					// Debug.DrawLine(position, position - side*right*width, Color.green, 20);

					continue;
				}

				if(foldCatch == true)
				{
					// fold end
					foldCatch = false;
					Vector3 averagePosition = (position + side * right * width / 2 + startPosition)/2;

					verts.Add(averagePosition);
					foldIndices.Add(index);
					triangles[triangles.Count - 1] = index;
					if(side < 0)
					{
						triangles[triangles.Count - 1] = foldIndices[0];
						triangles[triangles.Count - 2] = index;
						triangles[triangles.Count - 6] = index;
					}
					index++;	

					for(int j = 0; j < foldIndices.Count - 2; j++)
					{
						if(side > 0)
						{
							triangles.Add(foldIndices[j]);
							triangles.Add(foldIndices[j+1]);
							triangles.Add(foldIndices.Last());
						}
						else
						{
							triangles.Add(foldIndices[j]);
							triangles.Add(foldIndices.Last());
							triangles.Add(foldIndices[j+1]);
						}

						// Debug.DrawLine(verts[triangles[triangles.Count - 1]], verts[triangles[triangles.Count-2]], Color.red, 20);
					}

					if(side > 0)
					{
						triangles.Add(index);
						triangles.Add(index - 1);
						triangles.Add(index - 2);
					}
					else
					{
						triangles.Add(index - 2);
						triangles.Add(index - 1);
						triangles.Add(index + 1);
					}

					triangles.Add(index - 1);
					triangles.Add(index);
					triangles.Add(index + 1);
				}

				verts.Add(position - right*width/2);
				verts.Add(position + right*width/2);
				// Debug.DrawLine(verts.Last(), verts[verts.Count - 2], Color.green, 20);
				if(verts.Count > 2)
				{
					// Debug.DrawLine(verts[verts.Count - 2], verts[verts.Count - 3], Color.green, 20);
				}
				index += 2;

				if(i < steps)
				{
					triangles.Add(index);
					triangles.Add(index - 1);
					triangles.Add(index - 2);

					triangles.Add(index - 1);
					triangles.Add(index);
					triangles.Add(index + 1);
				}

				// Debug.DrawLine(position, position - right*width/2, Color.blue, 20);
				// Debug.DrawLine(position, position + right*width/2, Color.red, 20);
			}

			// for (int i = 0, n = 0; i < triangleCount; i += 6, n += 2)
			// {
			// 	triangles.Add(n);
			// 	triangles.Add(n+2);
			// 	triangles.Add(n+1);
			// 	triangles.Add(n+2);
			// 	triangles.Add(n+3);
			// 	triangles.Add(n+1);
			// }

			mesh.SetVertices(verts);
			mesh.SetIndices(triangles, MeshTopology.Triangles, 0);

			GetComponent<MeshFilter>().sharedMesh = mesh;
			GetComponent<MeshCollider>().sharedMesh = mesh;
		}

		private void OnSplineModified(Spline spline)
		{
			UpdateSpline();
		}

		[ContextMenu("Update Mesh")]
		private void UpdateSpline()
		{
			SetSlope();

			GenerateMesh();
		}
    }
}