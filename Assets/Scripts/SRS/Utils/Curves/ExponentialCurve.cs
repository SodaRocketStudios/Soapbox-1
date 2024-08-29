using UnityEngine;

namespace SRS.Utils.Curves
{
	public class ExponentialCurve
	{
		private float exponent;
		private float x0;
		private float y0;

		public ExponentialCurve(float exponent = 2, float horizontalShift = 0, float verticalShift = 0)
		{
			this.exponent = exponent;
			x0 = horizontalShift;
			y0 = verticalShift;
		}

		public float Evaluate(float x)
		{
			return Mathf.Pow(x - x0, exponent) - y0;
		}
	}
}