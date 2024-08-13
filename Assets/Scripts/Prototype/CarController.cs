using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Soap.Physics;
using Soap.GameManagement;
using UnityEngine.Events;
using System;
using SRS.UI;

namespace Soap.Prototype
{
	public class CarController : MonoBehaviour
	{
		private const float TOP_SPEED = 300;

		[Header("Physics Settings")]
		[SerializeField, Range(0.5f, 1f)] private float brakeBias;

		[SerializeField] private float preloadTorque;
		[SerializeField, Range(1, 5)] float torqueBiasRatio;

		[Header("UI")]
		[SerializeField] private ProgressBar throttleBar;
		[SerializeField] private ProgressBar brakeBar;
		[SerializeField] private NumberDisplay speedDisplay;
		[SerializeField] private ProgressBar speedBar;
		[SerializeField] private NumberDisplay ERSPercent;
		[SerializeField] private ProgressBar ERSBar;

		public UnityEvent OnfalseStart;

		private Rigidbody carRigidBody;

		private DifferentialV1 diff;

		private Wheel[] wheels;
		private Wheel[] driveWheels = new Wheel[2];

		private DRSManager drsManager;

		private MGUK mguk;

		private bool isPhysicsEnabled;

		private float accelerationInput;

		private void Awake()
		{
			carRigidBody = GetComponent<Rigidbody>();
			wheels = GetComponentsInChildren<Wheel>();
			driveWheels = wheels.Where(wheel => wheel.IsDriveWheel).ToArray();
			drsManager = GetComponent<DRSManager>();
			mguk = GetComponent<MGUK>();
			diff = new(driveWheels, preloadTorque, torqueBiasRatio);
		}

		private void Start()
		{
			PhysicsEnabled(false);
		}

		private void Update()
		{
			float speed = Vector3.Dot(carRigidBody.velocity*3.6f, transform.forward);
			speedDisplay.SetValue(speed);
			speedBar.SetPercentage(speed/TOP_SPEED);
			ERSPercent.SetValue(mguk.ChargeAmount*100);
			ERSBar.SetPercentage(mguk.ChargeAmount);
		}

		private void FixedUpdate()
		{
			if(GameState.Instance.State == State.PreStart)
			{
				carRigidBody.Sleep();
				return;
			}

			if (isPhysicsEnabled == false)
			{
				if(accelerationInput > 0)
				{
					PhysicsEnabled(true);
					return;
				}
				
				carRigidBody.Sleep();
				return;
			}

			float torque;
			
			if(accelerationInput > 0)
			{
				torque = mguk.Deploy(accelerationInput);
			}
			else
			{
				torque = mguk.Recharge();
			}

			diff.Accelerate(torque);
		}

		public void PhysicsEnabled(bool enabled)
		{
			if(GameState.Instance.State == State.Countdown)
			{
				OnfalseStart?.Invoke();
			}

			isPhysicsEnabled = enabled;
			carRigidBody.useGravity = enabled;
		}

		public void Steer(InputAction.CallbackContext context)
		{
			float steerInput = context.ReadValue<float>();
			
			foreach (Wheel wheel in wheels)
			{
				wheel.Steer(steerInput);
			}
		}

		public void Brake(InputAction.CallbackContext context)
		{
			float brakeInput = context.ReadValue<float>();

			brakeBar.SetPercentage(brakeInput);

			if(brakeInput > 0)
			{
				drsManager.SetActive(false);
			}

			foreach(Wheel wheel in wheels)
			{
				wheel.Brake(brakeInput, brakeBias);
			}
		}

		public void DRS(InputAction.CallbackContext context)
		{
			if(context.performed)
			{
				drsManager.ToggleDRS();
			}
		}

		public void Accelerate(InputAction.CallbackContext context)
		{
			accelerationInput = context.ReadValue<float>();

			throttleBar.SetPercentage(accelerationInput);
		}
	}
}