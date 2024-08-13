using UnityEngine;

namespace Soap.Physics
{
	public class DRSZoneTrigger : MonoBehaviour
	{
		[SerializeField] private bool inZone;
		
		private void OnTriggerEnter(Collider other)
		{
			other.GetComponent<DRSManager>().SetAvailable(inZone);
		}
	}
}
