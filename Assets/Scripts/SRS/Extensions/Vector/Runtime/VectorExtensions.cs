using System.Collections.Generic;
using UnityEngine;

namespace SRS.Extensions.Vector
{
	public static class VectorExtensions
	{
		public static float InverseLerp(this Vector3 point, Vector3 start, Vector3 end)
		{
			Vector3 temp = end - start;
			Vector3 temp2 = point - start;
			return Vector3.Dot(temp.normalized, temp2)/temp.magnitude;
		}

		public static float InverseLerp(this Vector2 point, Vector2 start, Vector2 end)
		{
			Vector2 temp = end - start;
			Vector2 temp2 = point - start;
			return Vector2.Dot(temp.normalized, temp2)/temp.magnitude;
		}

		public static Vector3 ElementwiseRound(this Vector3 vector)
		{
			vector.x = Mathf.Round(vector.x);
			vector.y = Mathf.Round(vector.y);
			vector.z = Mathf.Round(vector.z);
			return vector;
		}

		public static Vector2 ElementwiseRound(this Vector2 vector)
		{
			vector.x = Mathf.Round(vector.x);
			vector.y = Mathf.Round(vector.y);
			return vector;
		}

		public static float SquareDistance(Vector3 a, Vector3 b)
		{
			return Vector3.SqrMagnitude(b - a);
		}

		public static Vector3 Average(this Vector3[] vectors)
		{
			if(vectors.Length <= 0)
			{
				return Vector3.zero;
			}

			Vector3 meanVector = Vector3.zero;

			foreach(Vector3 vector in vectors)
			{
				meanVector += vector;
			}

			meanVector/= vectors.Length;

			return meanVector;
		}

		public static Vector3 Average(this List<Vector3> vectors)
		{
			if(vectors.Count <= 0)
			{
				return Vector3.zero;
			}

			Vector3 meanVector = Vector3.zero;

			foreach(Vector3 vector in vectors)
			{
				meanVector += vector;
			}

			meanVector /= vectors.Count;

			return meanVector;
		}

		public static Vector3 XZPlane(this Vector3 vector)
		{
			return new Vector3(vector.x, 0, vector.z);
		}
	}
}