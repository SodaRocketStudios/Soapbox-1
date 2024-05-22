using System;
using UnityEngine;

namespace SRS.Extensions.Curves
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class CurveAttribute : PropertyAttribute
	{
		public float Height;

		public CurveAttribute(float height)
		{
			Height = height;
		}
	}
}
