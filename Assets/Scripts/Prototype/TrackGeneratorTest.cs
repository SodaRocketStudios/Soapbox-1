using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using SRS.Extensions.Vector;




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
			float stepSize = 1f/(resolution*length);
			int triangleCount = segments*6;

			List<Vector3> verts = new();
			List<int> triangles = new();

			bool foldCatch = false;

			List<List<Vector3>> folds = new();
			List<List<int>> foldVerts = new();
			int foldCount = 0;

			for(int i = 0; i < steps; i++)
			{
				float t = Mathf.Min(1, stepSize*i);

				splineContainer.Spline.Evaluate(t, out var position, out var direction, out var normal);
				float curvature = splineContainer.Spline.EvaluateCurvature(t);
				Vector3 curveCenter = splineContainer.Spline.EvaluateCurvatureCenter(t);

				float3 perpendicular = math.normalizesafe(math.cross(normal, direction));


				if(1/curvature < width)
				{
					if(foldCatch == false)
					{
						// fold start
						foldCatch = true;
						folds.Add(new List<Vector3>());
						foldVerts.Add(new List<int>());
					}
					
					int side = (int)Mathf.Sign(Vector3.Dot(perpendicular, curveCenter - (Vector3)position));

					verts.Add(position - perpendicular*width/2*side);
					folds[foldCount].Add(position + perpendicular*width/2*side);

					foldVerts[foldCount].Add(i);

					Debug.DrawLine(position, position + perpendicular*width/2*side, Color.magenta, 20);
					// Debug.DrawLine(position, position - perpendicular*width, Color.magenta, 20);
					
				}
				else
				{
					if(foldCatch == true)
					{
						// fold end
						// Get the average position;
						Vector3 averagePosition = folds[foldCount].Average();

						verts.Add(averagePosition);
						i++;
						
						// remove all but one vert and set it to the average position
						foldCatch = false;
						foldCount++;
					}
					else
					{
						verts.Add(position - perpendicular*width/2);
						verts.Add(position + perpendicular*width/2);
						i++;
					}

				}

				// Debug.DrawLine(position, position - perpendicular*width/2, Color.blue, 20);
				// Debug.DrawLine(position, position + perpendicular*width/2, Color.red, 20);
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

		private void UpdateSpline()
		{
			SetSlope();

			GenerateMesh();
		}
    }
}