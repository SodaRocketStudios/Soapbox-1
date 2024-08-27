using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using SRS.UI;
using Soap.Input;

namespace Soap.Physics
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

		private CarManager carManager;

		private Rigidbody carRigidBody;

		private Differential diff;

		private Wheel[] wheels;
		private Wheel[] driveWheels = new Wheel[2];

		private DRSManager drsManager;

		private MGUK mguk;

		private float accelerationInput;

		private bool clutchInput = true;

		private void Awake()
		{
			carManager = GetComponent<CarManager>();
			carRigidBody = GetComponent<Rigidbody>();
			wheels = GetComponentsInChildren<Wheel>();
			driveWheels = wheels.Where(wheel => wheel.IsDriveWheel).ToArray();
			drsManager = GetComponent<DRSManager>();
			mguk = GetComponent<MGUK>();
			diff = new(driveWheels, preloadTorque, torqueBiasRatio);
		}

		private void OnEnable()
		{
			InputHandler.OnAccelerateInput += Accelerate;
			InputHandler.OnBrakeInput += Brake;
			InputHandler.OnSteerInput += Steer;
			InputHandler.OnDRSToggleInput += DRS;
			InputHandler.OnClutchInput += Clutch;

			clutchInput = true;
		}

		private void OnDisable()
		{
			InputHandler.OnAccelerateInput -= Accelerate;
			InputHandler.OnBrakeInput -= Brake;
			InputHandler.OnSteerInput -= Steer;
			InputHandler.OnDRSToggleInput -= DRS;
			InputHandler.OnClutchInput -= Clutch;
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
			float torque;

			if(clutchInput == true)
			{
				return;
			}
			
			if(carManager.IsPhysicsEnabled == false)
			{
				carManager.EnablePhysics();
			}
			
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

		public void Steer(float steer)
		{
			float steerInput = steer;
			
			foreach (Wheel wheel in wheels)
			{
				wheel.Steer(steerInput);
			}
		}

		public void Brake(float brake)
		{
			float brakeInput = brake;

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

		public void DRS()
		{
			drsManager.ToggleDRS();
		}

		public void Accelerate(float acceleration)
		{
			accelerationInput = acceleration;

			throttleBar.SetPercentage(accelerationInput);
		}

		public void Clutch(bool input)
		{
			clutchInput = input;
		}
	}
}