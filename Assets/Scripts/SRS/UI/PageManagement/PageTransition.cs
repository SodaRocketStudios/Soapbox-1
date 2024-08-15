using UnityEngine;

namespace SRS.UI.PageManagement
{
	public abstract class PageTransition
	{
		public abstract void Transition(GameObject currentPage, GameObject nextPage);
	}
}