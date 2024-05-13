using UnityEngine;

namespace Soap.Physics
{
	[CreateAssetMenu(fileName = "New Tire Profile", menuName = "Physics/Tire Profile")]
	[HelpURL("https://www.edy.es/dev/docs/pacejka-94-parameters-explained-a-comprehensive-guide/")]
	public class TireProfile : ScriptableObject
	{
		// Lateral
		[Header("Lateral parameters")]
		[Tooltip("Shape factor for lateral force. (defined as 1.3 in the Pacejka model)")]
		[SerializeField, Range(1.2f, 1.8f)] private float a0 = 1.3f;

		[Tooltip("Loads influence on lateral coefficient of friction. (x1000)")]
		[SerializeField, Range(-80, 80)] private float a1 = 0;

		[Tooltip("Lateral coefficient of friction. (x1000)")]
		[SerializeField, Range(900, 1700)] private float a2 = 1100;

		[Tooltip("Change of stiffness with slip.")]
		[SerializeField, Range(500, 2000)] private float a3 = 1100;

		[Tooltip("Change of progressivity of stiffness / load.")]
		[SerializeField, Range(0, 50)] private float a4 = 10;

		[Tooltip("Camber influence on stiffness.")]
		[SerializeField, Range(-0.1f, 0.1f)] private float a5 = 0;

		[Tooltip("Curvature change with load.")]
		[SerializeField, Range(-2, 2)] private float a6 = 0;

		[Tooltip("Curvature factor")]
		[SerializeField, Range(-20, 1)] private float a7 = -2;

		[Tooltip("Load influence on horizontal shift.")]
		[SerializeField, Range(-1, 1)] private float a8 = 0;

		[Tooltip("Horizontal shifft at load = 0 and camber = 0.")]
		[SerializeField, Range(-1, 1)] private float a9 = 0;

		[Tooltip("Camber influence on horizontal shift.")]
		[SerializeField, Range(-0.1f, 0.1f)] private float a10 = 0;

		[Tooltip("Vertical shift")]
		[SerializeField, Range(-200, 200)] private float a11 = 0;

		[Tooltip("Vertical shift at load = 0.")]
		[SerializeField, Range(-10, 10)] private float a12 = 0;

		[Tooltip("Camber influence on vertical shift, load dependent.")]
		[SerializeField, Range(-10, 10)] private float a13 = 0;

		[Tooltip("camber influence on vertical shift.")]
		[SerializeField, Range(-15, 15)] private float a14 = 0;

		[Tooltip("Camber influence on lateral coefficient of friction.")]
		[SerializeField, Range(-0.01f, 0.01f)] private float a15 = 0;

		[Tooltip("Curvature change with camber.")]
		[SerializeField, Range(-0.1f, 0.1f)] private float a16 = 0;

		[Tooltip("Curvature shift.")]
		[SerializeField, Range(-1, 1)] private float a17 = 0;

		// Longitudinal
		[Header("Longitudinal parameters")]
		[Tooltip("Shape factor for longitudinal force (defined as 1.65 in the Pacejka model)")]
		[SerializeField, Range(1.4f, 1.8f)] private float b0 = 1.65f;

		[Tooltip("Load influence on logitudinal coefficient of friction (x1000)")]
		[SerializeField, Range(-80, 80)] private float b1 = 0;

		[Tooltip("Longitudinal coefficient of friction. (x1000)")]
		[SerializeField, Range(900, 1700)] private float b2 = 1100;

		[Tooltip("Curvature factor of stiffness/load.")]
		[SerializeField, Range(-20, 20)] private float b3 = 0;

		[Tooltip("Change of stiffness with slip.")]
		[SerializeField, Range(100, 500)] private float b4 = 300;

		[Tooltip("Change of progressivity of stiffness/load.")]
		[SerializeField, Range(-1, 1)] private float b5 = 0;

		[Tooltip("Curvature change with load squared.")]
		[SerializeField, Range(-0.1f, 0.1f)] private float b6 = 0;

		[Tooltip("Curvature change with load.")]
		[SerializeField, Range(-1, 1)] private float b7 = 0;

		[Tooltip("Curvature factor")]
		[SerializeField, Range(-20, 1)] private float b8 = -2;

		[Tooltip("Load influence on horizontal shift.")]
		[SerializeField, Range(-1, 1)] private float b9 = 0;

		[Tooltip("Horizontal shift")]
		[SerializeField, Range(-5f, 5f)] private float b10 = 0;

		[Tooltip("Vertical shift")]
		[SerializeField, Range(-100, 100)] private float b11 = 0;

		[Tooltip("Vertical shift at load = 0.")]
		[SerializeField, Range(-10, 10)] private float b12 = 0;

		[Tooltip("Curvature shift")]
		[SerializeField, Range(-1, 1)] private float b13 = 0;

		public float[] LateralParameters
		{
			get
			{
				return new float[18] {a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17};
			}
		}
		public float[] LongitudinalParameters
		{
			get
			{
				return new float[14] {b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13};
			}
		}
	}
}
