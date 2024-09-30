using System;
using UnityEngine;

namespace Soap.LapTiming
{
	public class DeltaCheckpoint : MonoBehaviour
	{
		public Action<TimingLine> OnDeltaLogged;

		private void OnEnable()
		{
			GetComponent<TimingLine>().OnTrigger.AddListener(LogDelta);
		}

		private void OnDisable()
		{
			GetComponent<TimingLine>().OnTrigger.RemoveListener(LogDelta);
		}

		public void LogDelta(TimingLine checkpoint)
		{
			OnDeltaLogged?.Invoke(checkpoint);
		}
	}
}
