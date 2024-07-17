using Unity.Mathematics;
using UnityEngine;
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
			trackSplineContainer = GetComponent<SplineContainer>();
		}

		public void GenerateCheckpoints()
		{
			if(parent == null)
			{
				parent = new("Checkpoints");
			}

			while(parent.transform.childCount > 0)
			{
				Destroy(parent.transform.GetChild(0).gameObject);
			}

			for(int i = 0; i < numberOfCheckpoints; i++)
			{
				float t = i/numberOfCheckpoints;

				Spline spline = trackSplineContainer.Spline;

				spline.Evaluate(t, out float3 position, out float3 tangent, out float3 up);

				Instantiate(checkpointPrefab, position, Quaternion.LookRotation(tangent, up));
			}
		}

		public void RegenerateCheckpoints()
		{
			Destroy(parent);
			GenerateCheckpoints();
		}
	}
}