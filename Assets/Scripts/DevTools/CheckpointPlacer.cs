using UnityEngine;
using Unity.Mathematics;
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
		}

		private void Start()
		{
			UpdateCheckpoints();
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

				RaycastHit hit;

				if(Physics.Raycast(position, Vector3.down, out hit))
				{
					position.y = hit.point.y;
				}

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