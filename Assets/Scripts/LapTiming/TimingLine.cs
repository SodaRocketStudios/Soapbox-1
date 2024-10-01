using System;
using SRS.Utils.DataPersistence;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Soap.LapTiming
{
	public class TimingLine : MonoBehaviour, IPersist
	{
		public UnityEvent<TimingLine> OnTrigger;

		public TimedSegment TimedSegment = new();

        [SerializeField] private string id;
		public string ID {get => id;}

		private void OnValidate()
		{
			if(string.IsNullOrEmpty(id))
			{
				GenerateID();
			}
		}

        private void OnTriggerEnter(Collider other)
		{
			OnTrigger?.Invoke(this);
		}

        public object Save()
        {
			return new TimingData(this);
        }

        public void Load(object data)
        {
            TimingData timingData = data as TimingData;
			if(timingData != null)
			{
				id = timingData.ID;
				TimedSegment.SetBestTime(timingData.BestTime);
			}
        }

		[ContextMenu("Generate New ID")]
		private void GenerateID()
		{
			id = GUID.Generate().ToString();
		}
	}

	[Serializable]
	public class TimingData
	{
		public string ID;
		public float BestTime;

		public TimingData(TimingLine line)
		{
			ID = line.ID;
			BestTime = line.TimedSegment.BestTime;
		}
	}
}