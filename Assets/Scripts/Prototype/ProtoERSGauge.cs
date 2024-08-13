using UnityEngine;
using UnityEngine.UI;
using Soap.Physics;

namespace Soap.Prototype
{
	public class ProtoERSGauge : MonoBehaviour
	{
		[SerializeField] private MGUK mguk;
		private Slider slider;

		private void Awake()
		{
			slider = GetComponent<Slider>();
		}

		private void Update()
		{
			slider.value = mguk.ChargeAmount;
		}
	}
}
