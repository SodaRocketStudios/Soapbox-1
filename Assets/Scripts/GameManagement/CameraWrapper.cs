using UnityEngine;
using Cinemachine;

namespace Soap.GameManagement
{
	public class CameraWrapper : MonoBehaviour
	{
		[SerializeField] private Transform target;

		private void OnEnable()
		{
			SetCameraTarget();
		}

		public void SetCameraTarget()
		{
			Camera.main.GetComponent<CinemachineVirtualCamera>().Follow = target;
		}
	}
}