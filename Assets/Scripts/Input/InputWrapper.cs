using UnityEngine;

namespace Soap.Input
{
	public class InputWrapper : MonoBehaviour
	{
		public void SetGameplayInput()
		{
			InputHandler.Instance.SetGameplayInput();
		}

		public void SetUIInput()
		{
			InputHandler.Instance.SetUIInput();
		}
	}
}