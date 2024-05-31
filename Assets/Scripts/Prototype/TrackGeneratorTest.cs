using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

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
			float[] lengths = new float[splineContainer.Spline.GetCurveCount()];

			for(int i = 0; i < numberOfCurves; i++)
			{
				if(i == 0)
				{
					lengths[i] = splineContainer.Spline.GetCurveLength(i);
					continue;
				}

				lengths[i] = lengths[i-1] + splineContainer.Spline.GetCurveLength(i);
			}

            for (int i = 0; i < numberOfCurves; i++)
            {
				float dropPerUnit = Mathf.Sin(-slope*Mathf.Deg2Rad);

                BezierKnot knot = splineContainer.Spline[i + 1];
				BezierKnot previousKnot = splineContainer.Spline[i];
				BezierKnot nextKnot = splineContainer.Spline[i + 1];

				if(i < numberOfCurves - 2)
				{
					nextKnot = splineContainer.Spline[i + 2];
				}

				knot.Position.y = lengths[i]*dropPerUnit;

				Quaternion rotation = knot.Rotation;
				Vector3 eulerRotation = Vector3.up*rotation.eulerAngles.y;
				eulerRotation.x = slope;

				float outAngle = 0;
				float inAngle = ((Quaternion)previousKnot.Rotation).eulerAngles.y - eulerRotation.y;

				if(i < numberOfCurves - 2)
				{
					outAngle = eulerRotation.y - ((Quaternion)nextKnot.Rotation).eulerAngles.y;
				}

				if(Mathf.Abs(outAngle) > 180)
				{
					outAngle -= 360*Mathf.Sign(outAngle);
				}

				if(Mathf.Abs(inAngle) > 180)
				{
					inAngle -= 360*Mathf.Sign(inAngle);
				}

				if(i < numberOfCurves - 1)
				{
					outAngle /= splineContainer.Spline.GetCurveLength(i+1);
				}

				inAngle /= splineContainer.Spline.GetCurveLength(i);

				eulerRotation.z = (outAngle+inAngle/2)*maxRoll;

				rotation.eulerAngles = eulerRotation;
				knot.Rotation = rotation;


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

				Debug.DrawLine(position, position - perpendicular*width/2, Color.blue, 10);
				Debug.DrawLine(position, position + perpendicular*width/2, Color.red, 10);
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