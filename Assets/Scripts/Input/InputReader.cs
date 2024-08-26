using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SRS.Input
{
	[CreateAssetMenu(menuName = "Input Reader")]
	public class InputReader : ScriptableObject, InputActions.IGameplayActions, InputActions.IUIActions
	{
		private InputActions inputActions;

		// Gameplay Actions
		public Action<float> OnSteerInput;
		public Action<float> OnAcceleratorInput;
		public Action<float> OnBrakeInput;
		public Action OnDRSToggleInput;
		public Action OnPauseInput;
		public Action OnResetInput;

		// UI Actions
		public Action OnTabRightInput;
		public Action OnTabLeftInput;
		public Action OnResumeInput;

		private void OnEnable()
        {
            // This only gets called when the SO is loaded. Would probably work fine in builds, but causes problems in the editor.
            Debug.Log("Input Reader Enabled.");
            Initialize();
        }

        public void Initialize()
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

        public void OnSteering(InputAction.CallbackContext context)
        {
            OnSteerInput?.Invoke(context.ReadValue<float>());
        }

        public void OnBrake(InputAction.CallbackContext context)
        {
            OnBrakeInput?.Invoke(context.ReadValue<float>());
        }

        public void OnAccelerate(InputAction.CallbackContext context)
        {
            OnAcceleratorInput?.Invoke(context.ReadValue<float>());
        }

        public void OnReset(InputAction.CallbackContext context)
        {
			if(context.performed)
			{
				OnResetInput?.Invoke();
			}
        }

        public void OnDRSToggle(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if(context.performed)
			{
				OnPauseInput?.Invoke();
			}
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
        }

        public void OnClick(InputAction.CallbackContext context)
        {
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
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

        public void OnReturn(InputAction.CallbackContext context)
        {
            if(context.performed)
			{
				OnResumeInput?.Invoke();
			}
        }

        public void OnClutch(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }
    }
}