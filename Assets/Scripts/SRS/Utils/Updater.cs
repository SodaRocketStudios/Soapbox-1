using System;
using UnityEngine;

namespace SRS.Utils
{
	public class Updater : MonoBehaviour
	{
		private Updater instance;
		public Updater Instance
		{
			get
			{
				if(instance == null)
				{
					CreateUpdater();
				}

				return instance;
			}
		}

		public static event Action<float> OnUpdate;

		private void CreateUpdater()
		{
			instance = new GameObject("Updater").AddComponent<Updater>();
		}

		private void Update()
		{
			OnUpdate?.Invoke(Time.deltaTime);
		}
	}

	// public class UpdateCaller : MonoBehaviour {
    // private static UpdateCaller instance;
    // public static void AddUpdateCallback(Action updateMethod) {
    //     if (instance == null) {
    //         instance = new GameObject("[Update Caller]").AddComponent<UpdateCaller>();
    //     }
    //     instance.updateCallback += updateMethod;
    // }
 
    // private Action updateCallback;
 
    // private void Update() {
    //     updateCallback();
    // }
}