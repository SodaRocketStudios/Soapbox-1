using UnityEngine;
using UnityEditor;
using Unity.Mathematics;
using UnityEditor.Splines;
using UnityEngine.Splines;

namespace Soap.Tools
{
	public class CheckpointPlacer : MonoBehaviour
	{
		[SerializeField] private GameObject checkpointPrefab;
		[SerializeField] private int numberOfCheckpoints;

		private SplineContainer trackSplineContainer;

		private GameObject parent;

		private void Awake()
		{
			if(trackSplineContainer == null)
			{
				trackSplineContainer = GetComponent<SplineContainer>();
			}
			trackSplineContainer = GetComponent<SplineContainer>();
		}

		private void OnEnable()
		{
#if UNITY_EDITOR
			EditorSplineUtility.AfterSplineWasModified += PlaceCheckpoints;
			Undo.undoRedoPerformed += UpdateCheckpoints;
#endif
		}

		private void OnDisable()
		{
#if UNITY_EDITOR
			EditorSplineUtility.AfterSplineWasModified -= PlaceCheckpoints;
			Undo.undoRedoPerformed -= UpdateCheckpoints;
#endif
		}

		public void PlaceCheckpoints(Spline spline)
		{
			if(parent != null)
			{
#if UNITY_EDITOR
				DestroyImmediate(parent);
#else
				Destroy(parent);
#endif
			}

			parent = new("Checkpoints");

			for(int i = 0; i < numberOfCheckpoints; i++)
			{
				float t = i*1.0f/numberOfCheckpoints;

				spline.Evaluate(t, out float3 position, out float3 tangent, out float3 up);

				Instantiate(checkpointPrefab, position, Quaternion.LookRotation(tangent, up), parent.transform);
			}
		}

		[ContextMenu("Update Checkpoints")]
		private void UpdateCheckpoints()
		{
			if(trackSplineContainer == null)
			{
				trackSplineContainer = GetComponent<SplineContainer>();
			}
			PlaceCheckpoints(trackSplineContainer.Spline);
		}
	}
}