using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using SRS.Extensions.Vector;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Splines;
#endif

namespace Soap.DevTools
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshCollider))]
	[RequireComponent(typeof(SplineContainer), typeof(MeshFilter), typeof(MeshRenderer))]
	public class TrackGenerator : MonoBehaviour
	{

		[Tooltip("The slope of the track in degrees")]
		[SerializeField] private float slope;

		[SerializeField] private float maxRoll = 0;

		[SerializeField, Min(0.1f)] private float resolution;

		[SerializeField] private float width = 1;

		private SplineContainer splineContainer;

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

		private void GenerateMesh()
		{
			mesh.Clear();

			float length = splineContainer.Spline.GetLength();
			int segments = Mathf.CeilToInt(resolution*length);
			int steps = segments + 1;
			float stepSize = 1f/steps;

			List<Vector3> verts = new();
			List<int> triangles = new();
			
			float drop = -Mathf.Sin(slope*Mathf.Deg2Rad);

			bool foldCatch = false;
			
			int foldStartIndex = 0;
			int foldEndIndex = 0;

			bool correctingCorner = false;

			Spline spline = splineContainer.Spline;

			for(int i = 0, index = 0; i <= steps; i++, index += 2)
			{
				float t = stepSize * i;

				spline.Evaluate(t, out float3 pos, out float3 direction, out float3 normal);
				Vector3 position = pos;
				Vector3 right = math.normalizesafe(math.cross(normal, direction));

				float curvature = spline.EvaluateCurvature(t);
				Vector3 curveCenter = spline.EvaluateCurvatureCenter(t);

				Vector3 leftPosition = position - right * width / 2;
				Vector3 rightPosition = position + right * width / 2;

				int inside = (int)Mathf.Sign(Vector3.Dot(right, curveCenter - position));

				if(1/curvature <= width / 2)
				{
					// On fold start
					if(foldCatch == false)
					{
						foldCatch = true;
						foldStartIndex = inside < 0? index - 2: index - 1;
					}
				}
				// // On fold end
				else if(foldCatch == true)
				{
					foldCatch = false;
					foldEndIndex = inside < 0? index: index + 1;
					correctingCorner = true;
				}

				verts.Add(leftPosition);
				verts.Add(rightPosition);

				if(correctingCorner == true)
				{
					Vector3 stepDirection = verts[foldEndIndex] - verts[foldStartIndex];

					if(Vector3.Dot(stepDirection, direction) > 0)
					{
						correctingCorner = false;
						Vector3 startPosition = verts[foldStartIndex];
						Vector3 endPosition = verts[foldEndIndex];

						for(int j = foldStartIndex; j <= foldEndIndex; j += 2)
						{
							float step = (j - foldStartIndex)*1.0f / (foldEndIndex - foldStartIndex);
							verts[j] = Vector3.Lerp(startPosition, endPosition, step);
						}
					}

					foldStartIndex -= 2;
					foldEndIndex += 2;
				}

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

			verts[0] = verts[0].XZPlane();
			verts[1] = verts[1].XZPlane();

			float distance = 0;

			for(int i = 2; i < verts.Count; i += 2)
			{
				Vector3 leftPosition = verts[i];
				Vector3 rightPosition = verts[i+1];
				distance += Mathf.Min(Vector3.Distance(verts[i-2].XZPlane(), leftPosition.XZPlane()), Vector3.Distance(verts[i-1].XZPlane(), rightPosition.XZPlane()));

				leftPosition.y += distance*drop;
				rightPosition.y += distance*drop;

				verts[i] = leftPosition;
				verts[i + 1] = rightPosition;
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

		[ContextMenu("Update Mesh")]
		private void UpdateSpline()
		{
			if(splineContainer == null)
			{
				splineContainer = GetComponent<SplineContainer>();
			}
			GenerateMesh();
		}
    }
}