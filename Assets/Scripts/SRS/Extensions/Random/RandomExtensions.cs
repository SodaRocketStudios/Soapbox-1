using UnityEngine;

namespace SRS.Extensions.Random
{
	public static class RandomExtensions
	{
		public static float NextFloat(this System.Random random)
		{
			return (float)random.NextDouble();
		}

		public static float NextFloat(this System.Random random, float min, float max)
		{
			return random.NextFloat()*(max - min) + min;
		}

		public static Vector3 WithinUnitSphere(this System.Random random)
		{
			return new Vector3(random.NextFloat()*2 - 1, random.NextFloat()*2 - 1, random.NextFloat()*2 - 1);
		}

		public static Vector2 WithinUnitCircle(this System.Random random)
		{
			return new Vector2(random.NextFloat()*2 - 1, random.NextFloat()*2 - 1);
		}

		public static Vector2 WithinRect(this System.Random random, Rect bounds)
		{
			float randomX = random.NextFloat()*(bounds.xMax - bounds.xMin) + bounds.xMin;
			float randomY = random.NextFloat()*(bounds.yMax - bounds.yMin) + bounds.yMin;
			return new Vector2(randomX, randomY);
		}
	}
}