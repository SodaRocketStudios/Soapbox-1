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
			for (int i = 0; i < splineContainer.Spline.Count; i++)
			{
				BezierKnot knot = splineContainer.Spline[i];
				knot.Position.y = 0;
				Quaternion rotation = knot.Rotation;
				Vector3 eulerRotation = new Vector3(0, rotation.eulerAngles.y, 0);
				rotation.eulerAngles = eulerRotation;
				knot.Rotation = rotation;
				splineContainer.Spline.SetKnot(i, knot);
			}

			int numberOfCurves = splineContainer.Spline.GetCurveCount();

			float length = splineContainer.Spline.GetLength();

			float[] lengths = new float[numberOfCurves];
			float[] curvatures = new float[numberOfCurves];

			for(int i = 0; i < numberOfCurves; i++)
			{
				if(i == 0)
				{
					lengths[i] = splineContainer.Spline.GetCurveLength(i);
					curvatures[i] = splineContainer.Spline.EvaluateCurvature(lengths[i]/splineContainer.Spline.GetLength());
					curvatureNormaization = curvatures[i];
					continue;
				}

				lengths[i] = lengths[i-1] + splineContainer.Spline.GetCurveLength(i);
				curvatures[i] = splineContainer.Spline.EvaluateCurvature(lengths[i]/splineContainer.Spline.GetLength());

				curvatureNormaization = Mathf.Max(curvatures[i], curvatureNormaization);
			}

            for (int i = 0; i < numberOfCurves; i++)
            {
				float dropPerUnit = Mathf.Sin(-slope*Mathf.Deg2Rad);

                BezierKnot knot = splineContainer.Spline[i + 1];

				float t = lengths[i]/length;

				Vector3 curveCenter = splineContainer.Spline.EvaluateCurvatureCenter(t);

				knot.Position.y = lengths[i]*dropPerUnit;

				Quaternion rotation = knot.Rotation;
				Vector3 eulerRotation = rotation.eulerAngles;

				eulerRotation.x = slope;

				Vector3 right = rotation*Vector3.right.normalized;

				float roll = -curvatures[i]*Mathf.Sign(Vector3.Dot(right, curveCenter - (Vector3)knot.Position))*maxRoll/curvatureNormaization;
				Debug.Log($"{i+1}: {roll}, {right}");

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
			int vertexCount = steps*2;
			int triangleCount = segments*6;

			Vector3[] verts = new Vector3[vertexCount];
			int[] triangles = new int[triangleCount];

			for(int i = 0; i < steps; i++)
			{
				float t = Mathf.Min(1, stepSize*i);

				splineContainer.Spline.Evaluate(t, out var position, out var direction, out var normal);

				float3 perpendicular = math.normalizesafe(math.cross(normal, direction));

				verts[2*i] = position - perpendicular*width/2;
				verts[2*i + 1] = position + perpendicular*width/2;

				// Debug.DrawLine(position, position - perpendicular*width/2, Color.blue, 10);
				// Debug.DrawLine(position, position + perpendicular*width/2, Color.red, 10);
			}

			for (int i = 0, n = 0; i < triangleCount; i += 6, n += 2)
			{
				triangles[i] = n;
				triangles[i+1] = n+2;
				triangles[i+2] = n+1;
				triangles[i+3] = n+2;
				triangles[i+4] = n+3;
				triangles[i+5] = n+1;
			}

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