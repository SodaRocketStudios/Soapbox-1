using System.Collections;
using UnityEngine;

namespace SRS.UI
{
	public abstract class UITransition : ScriptableObject
	{
		public abstract IEnumerator Animate(GameObject element);
	}
}