using System;
using UnityEngine;

namespace SRS.Extensions.AnimationCurves
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
