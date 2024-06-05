using UnityEngine;
using UnityEngine.Splines;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Splines;
#endif

namespace Soap.Prototype
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshCollider))]
	[RequireComponent(typeof(SplineContainer), typeof(MeshFilter), typeof(MeshRenderer))]
	public class EdgeSplineTrackGenerator : MonoBehaviour
	{
		[SerializeField] private float resolution;

		[SerializeField] private float width;

		private void GenerateEdges()
		{
			
		}
	}
}
