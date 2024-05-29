using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using System;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Splines;
#endif

namespace Soap.Prototype
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(SplineContainer), typeof(MeshFilter), typeof(MeshRenderer))]
	public class TrackGeneratorTest : MonoBehaviour
	{
		[SerializeField] private SplineContainer splineContainer;

		[Tooltip("The slope of the track in degrees")]
		[SerializeField] private float slopeDeg;

		[SerializeField] private float resolution;

		[SerializeField] private float width = 1;

		private Mesh mesh;

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
			float slope = Mathf.Sin(slopeDeg*Mathf.Deg2Rad);

            float lengthSum = 0;

			float[] length = new float[splineContainer.Spline.GetCurveCount()];

			for (int i = 0; i < length.Length; i++)
			{
				lengthSum += splineContainer.Spline.GetCurveLength(i);
				length[i] = lengthSum;
			}

            for (int i = 0; i < splineContainer.Spline.GetCurveCount(); i++)
            {
                BezierKnot knot = splineContainer.Spline[i + 1];
				knot.Position.y = 0;
                knot.Position += new float3(0, -slope * length[i], 0);
				knot.Rotation *= Quaternion.Euler(slope, 0, 0);

				Quaternion rotation = knot.Rotation;

				Vector3 direction = rotation*Vector3.forward;
				direction.y = -slope;

				knot.Rotation = Quaternion.LookRotation(direction);

                splineContainer.Spline.SetKnot(i + 1, knot);
            }
        }

		private void GenerateMesh()
		{
			int segments = Mathf.CeilToInt(resolution*splineContainer.Spline.GetLength());
			int steps = segments + 1;
			float stepSize = 1f/segments;
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
