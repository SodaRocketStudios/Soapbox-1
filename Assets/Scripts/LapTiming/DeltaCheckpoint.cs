using System;
using UnityEngine;

namespace Soap.LapTiming
{
	public class DeltaCheckpoint : MonoBehaviour
	{
		public Action<TimingLine> OnDeltaLogged;

		private void Awake()
		{
			GetComponent<TimingLine>().OnTriggerAction += LogDelta;
		}

		public void LogDelta(TimingLine checkpoint)
		{
			OnDeltaLogged?.Invoke(checkpoint);
		}
	}
}
