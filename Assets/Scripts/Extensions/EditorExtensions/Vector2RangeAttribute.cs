using System;
using UnityEngine;

namespace SRS.Extensions.EditorExtensions
{
	[AttributeUsage(AttributeTargets.Field)]
	public class Vector2RangeAttribute : PropertyAttribute
	{
		public float XMin;
		public float XMax;
		public float YMin;
		public float YMax;

		public Vector2RangeAttribute(float xMin, float xMax, float yMin, float yMax)
		{
			XMin = xMin;
			XMax = xMax;
			YMin = yMin;
			YMax = yMax;
		}
	}
}
