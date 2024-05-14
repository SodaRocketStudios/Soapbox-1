using UnityEditor;
using UnityEngine;

namespace Soap.Telemetry
{
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	public class ArrowGenerator : MonoBehaviour
	{
		[SerializeField, Min(0)] private float width = 0.1f;
		[SerializeField, Range(1, 5)] private float widthRatio = 2f;
		[SerializeField, Min(0)] private float length = 1;
		[SerializeField, Min(0)] private float tipShape = 1f;

		private Vector3[] verts = new Vector3[7];
		private int[] triangles = new int[9];

		private Mesh mesh;

		private void Start()
		{
			mesh = new Mesh();
			GetComponent<MeshFilter>().mesh = mesh;
		}

		private void Update()
		{
			GenerateArrow();
		}

		public void Draw(Vector3 origin, Vector3 direction)
		{

		}

		private void GenerateArrow()
		{
			float stemHalfWidth = width/2;
			float tipHalfWidth = width*widthRatio/2;

			float tipLength = tipHalfWidth*tipShape*2;
			float stemLength = length - tipLength;

			Vector3 stemOrigin = Vector3.zero;
			Vector3 tipOrigin = stemOrigin + stemLength*Vector3.right;

			// Create stem verts
			verts[0] = stemOrigin + (stemHalfWidth*Vector3.down);
			verts[1] = stemOrigin + (stemHalfWidth*Vector3.up);
			verts[2] = verts[0] + (stemLength*Vector3.right);
			verts[3] = verts[1] + (stemLength*Vector3.right);

			// Create stem triangles
			triangles[0] = 0;
			triangles[1] = 1;
			triangles[2] = 3;
			triangles[3] = 0;
			triangles[4] = 3;
			triangles[5] = 2;

			// Create tip verts
			verts[4] = tipOrigin + tipHalfWidth*Vector3.down;
			verts[5] = tipOrigin + tipHalfWidth*Vector3.up;
			verts[6] = tipOrigin + tipLength*Vector3.right;

			// Create tip triangles
			triangles[6] = 4;
			triangles[7] = 5;
			triangles[8] = 6;

			mesh.vertices = verts;
			mesh.triangles = triangles;
		}
	}
}
