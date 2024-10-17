using UnityEngine;

namespace Soap.GameManagement
{
	public class ResetWrapper : MonoBehaviour
	{
		public void Reset()
		{
			ResetManager.Reset();
		}
	}
}