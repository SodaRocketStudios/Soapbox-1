using System.Collections.Generic;
using System.Linq;
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
	public class TrackGeneratorV3 : MonoBehaviour
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

			List<int> foldIndices = new();
			List<Vector3> foldPositions = new();

			for(int i = 0, index = 0; i <= steps; i++, index += 2)
			{
				float t = stepSize * i;

				splineContainer.Spline.Evaluate(t, out float3 pos, out float3 direction, out float3 normal);
				Vector3 position = pos;
				Vector3 right = math.normalizesafe(math.cross(normal, direction));

				float curvature = splineContainer.Spline.EvaluateCurvature(t);
				Vector3 curveCenter = splineContainer.Spline.EvaluateCurvatureCenter(t);

				Vector3 leftPosition = position - right * width / 2;
				Vector3 rightPosition = position + right * width / 2;

				int inside = (int)Mathf.Sign(Vector3.Dot(right, curveCenter - position));

				if(index >= 2)
				{
					leftPosition.y = verts[index - 2].y;
					rightPosition.y = verts[index - 1].y;
					float distance = Mathf.Min(Vector3.Distance(leftPosition, verts[index - 2]), Vector3.Distance(rightPosition, verts[index - 1]));
					leftPosition.y += distance*drop;
					rightPosition.y += distance*drop;
				}
				
				if(1/curvature < width)
				{
					// On fold start
					if(foldCatch == false)
					{
						foldCatch = true;
						foldIndices.Clear();
						foldPositions.Clear();
					}

					if(inside < 0)
					{
						foldIndices.Add(index);
						foldPositions.Add(leftPosition);
					}
					else
					{
						foldIndices.Add(index + 1);
						foldPositions.Add(rightPosition);
					}
				}
				// // On fold end
				else if(foldCatch == true)
				{
					foldCatch = false;
					
					Vector3 startPosition = foldPositions[0];
					Vector3 endPosition = foldPositions.Last();
					endPosition.y = startPosition.y;

					for(int j = 0; j < foldIndices.Count; j++)
					{
						Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, j*1.0f/foldIndices.Count);
						if(j > 0)
						{
							newPosition.y = foldPositions[j-1].y;
							newPosition.y += Vector3.Distance(foldPositions[j - 1], newPosition)*drop;
							Debug.DrawLine(newPosition, foldPositions[j - 1], Color.red, 10);
						}
						verts[foldIndices[j]] = newPosition;
					}
				}


				verts.Add(leftPosition);
				verts.Add(rightPosition);

				// Debug.DrawLine(verts[index + 1], verts[index], Color.green, 10);

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