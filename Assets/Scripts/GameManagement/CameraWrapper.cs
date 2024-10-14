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
			CinemachineVirtualCamera vCam = Camera.main.GetComponent<CinemachineVirtualCamera>();
			vCam.Follow = target;
			vCam.LookAt = target;
		}
	}
}