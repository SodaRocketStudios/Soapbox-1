using System;
using System.Collections;
using UnityEngine;

namespace SRS.UI
{
	public abstract class PanelTransition : ScriptableObject
	{
		public Action OnTransitionEnd;
		
		public abstract IEnumerator Animate(GameObject element);
	}
}