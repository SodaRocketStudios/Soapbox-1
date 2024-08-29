using UnityEngine;

namespace SRS.Utils.Curves
{
	public class SigmoidCurve
	{
		private float L;
		private float x0;
		private float y0;
		private float k;

		public SigmoidCurve(float asymptote = 1, float horizontalShift = 0, float verticalShift = 0, float growthRate = 1)
		{
			L = asymptote;
			x0 = horizontalShift;
			y0 = verticalShift;
			k = growthRate;
		}

		public float Evaluate(float x)
		{
			return L/(1 + Mathf.Exp(-k * (x - x0))) - y0;
		}
	}
}