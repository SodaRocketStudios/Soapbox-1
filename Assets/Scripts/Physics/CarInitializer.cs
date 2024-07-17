using System.Collections.Generic;
using UnityEngine;

namespace Soap.Physics
{
	public class CarInitializer : MonoBehaviour
	{
		private Suspension[] suspensions;

		private Rigidbody carRigidbody;

		private void Awake()
		{
			suspensions = GetComponentsInChildren<Suspension>();
			carRigidbody = GetComponent<Rigidbody>();
		}

		private void Start()
		{
			GroundCar();
		}

		public void GroundCar()
		{
			RaycastHit hit;

			List<int> frontIndices = new();
			List<int> rearIndices = new();

			int i = 0;

			float[] distances = new float[suspensions.Length];

			foreach(Suspension suspension in suspensions)
			{
				float zPosition = suspension.transform.localPosition.z;

				if(UnityEngine.Physics.Raycast(suspension.transform.position, Vector3.down, out hit))
				{
					distances[i] = hit.distance;
				}

				if(zPosition < 0)
				{
					rearIndices.Add(i);
				}
				else if(zPosition > 0)
				{
					frontIndices.Add(i);
				}

				i++;
			}

			float wheelBase = suspensions[frontIndices[0]].transform.localPosition.z - suspensions[rearIndices[0]].transform.localPosition.z;
			float heightDelta = distances[frontIndices[0]] - distances[rearIndices[0]];

			float angle = Mathf.Atan2(heightDelta, wheelBase)*180/Mathf.PI;

			// transform.rotation *= Quaternion.AngleAxis(angle, transform.right);
			transform.Rotate(Vector3.right*angle);

			if(UnityEngine.Physics.Raycast(transform.position, Vector3.down, out hit))
			{
				float distance = hit.distance - suspensions[0].GroundedLength;
				transform.Translate(-transform.up*distance);
			}

			carRigidbody.angularVelocity = Vector3.zero;
			carRigidbody.velocity = Vector3.zero;
		}
	}
}