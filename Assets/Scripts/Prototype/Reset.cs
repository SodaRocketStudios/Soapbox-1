using UnityEngine;
using UnityEngine.InputSystem;

public class Reset : MonoBehaviour
	{
		[SerializeField] private Transform carTransform;
		private Rigidbody carRigidbody;

		private Vector3 startLocation;

		private void Start()
		{
			startLocation = carTransform.position;
			carRigidbody = carTransform.GetComponent<Rigidbody>();
		}

		public void ResetPosition(InputAction.CallbackContext context)
		{
			if(context.performed)
			{
				carTransform.position = startLocation;
				carTransform.rotation = Quaternion.identity;
				carRigidbody.velocity = Vector3.zero;
				carRigidbody.angularVelocity = Vector3.zero;
			}
		}

		public void ResetTimeScale(InputAction.CallbackContext context)
		{
			if(context.performed)
			{
				Time.timeScale = 1;
			}
		}
	}
