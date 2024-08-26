using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SRS.Input
{
    public class InputHandler : MonoBehaviour, InputActions.IGameplayActions, InputActions.IUIActions
    {
		public static InputHandler Instance;


		// Gameplay Actions
		public Action<float> OnSteerInput;
		public Action<float> OnAccelerateInput;
		public Action<float> OnBrakeInput;
		public Action<int> OnClutchInput;
		public Action OnDRSToggleInput;
		public Action OnPauseInput;

		// UI Actions
		public Action OnTabRightInput;
		public Action OnTabLeftInput;
		public Action OnReturnInput;
		public Action OnCancelInput;

		private InputActions inputActions;
		
		private void Awake()
		{
			if(Instance == null)
			{
				Instance = this;
			}
			else if(Instance != this)
			{
				Destroy(gameObject);
				return;
			}
		}

		private void OnEnable()
		{
			if (inputActions == null)
            {
                inputActions = new();
            }

            inputActions.Gameplay.SetCallbacks(this);
            inputActions.UI.SetCallbacks(this);

            SetUIInput();
		}

		public void SetGameplayInput()
		{
			inputActions.Gameplay.Enable();
			inputActions.UI.Disable();
		}

		public void SetUIInput()
		{
			inputActions.UI.Enable();
			inputActions.Gameplay.Disable();
		}

        public void OnAccelerate(InputAction.CallbackContext context)
        {
            OnAccelerateInput?.Invoke(context.ReadValue<float>());
        }

        public void OnBrake(InputAction.CallbackContext context)
        {
            OnBrakeInput?.Invoke(context.ReadValue<float>());
        }

		public void OnSteering(InputAction.CallbackContext context)
        {
			OnSteerInput?.Invoke(context.ReadValue<float>());
        }

		public void OnClutch(InputAction.CallbackContext context)
        {
			if(context.performed)
			{
				OnClutchInput?.Invoke(1);
			}
			else if(context.canceled)
			{
				OnClutchInput?.Invoke(0);
			}
        }

        public void OnDRSToggle(InputAction.CallbackContext context)
        {
			if(context.performed)
			{
            	OnDRSToggleInput?.Invoke();
			}
        }
		
        public void OnPause(InputAction.CallbackContext context)
        {
			if(context.performed)
			{
            	OnPauseInput?.Invoke();
			}
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if(context.performed)
			{
				OnCancelInput?.Invoke();
			}
        }

        public void OnClick(InputAction.CallbackContext context)
        {
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
        }

        public void OnReturn(InputAction.CallbackContext context)
        {
			if(context.performed)
			{
            	OnReturnInput?.Invoke();
			}
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
        }

        public void OnTabLeft(InputAction.CallbackContext context)
        {
            if(context.performed)
			{
            	OnTabLeftInput?.Invoke();
			}
        }

        public void OnTabRight(InputAction.CallbackContext context)
        {
            if(context.performed)
			{
            	OnTabRightInput?.Invoke();
			}
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
        }
    }
}