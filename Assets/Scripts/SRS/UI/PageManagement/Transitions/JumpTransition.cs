using UnityEngine;

namespace SRS.UI.PageManagement
{
    public class JumpTransition : PageTransition
    {
        public override void Transition(GameObject currentPage, GameObject nextPage)
        {
			currentPage.SetActive(false);
			currentPage.SetActive(true);
        }
    }
}