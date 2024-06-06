using System;
using UnityEngine;

namespace SRS.Utils
{
	public class Updater : MonoBehaviour
	{
		private static Updater instance;

		private Action<float> updateCallback;

		public static void AddUpdateCallback(Action<float> updateMethod)
		{
			if (instance == null)
			{
				instance = new GameObject("[Update Caller]").AddComponent<Updater>();
			}

			instance.updateCallback += updateMethod;
		}

		private void Update()
		{
			updateCallback(Time.deltaTime);
		}
	}
}