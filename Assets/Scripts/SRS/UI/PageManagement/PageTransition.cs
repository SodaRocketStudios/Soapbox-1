using System.Collections;
using UnityEngine;

namespace SRS.UI.PageManagement
{
	public abstract class PageTransition : ScriptableObject
	{
		public abstract IEnumerator Animate(GameObject page);
	}
}