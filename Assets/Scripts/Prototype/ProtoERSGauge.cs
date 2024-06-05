using UnityEngine;
using UnityEngine.UI;

namespace Soap.Prototype
{
	public class ProtoERSGauge : MonoBehaviour
	{
		[SerializeField] private ProtoERS ers;
		private Slider slider;

		private void Awake()
		{
			slider = GetComponent<Slider>();
		}

		private void Update()
		{
			slider.value = ers.chargeAmount;
		}
	}
}
