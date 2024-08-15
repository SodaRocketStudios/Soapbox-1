using UnityEngine;

namespace SRS.UI.PageManagement
{
	[CreateAssetMenu(fileName = "New Jump Transition", menuName = "Page Transitions/JumpTransition")]
    public class JumpTransition : PageTransition
    {
        protected override void Animate(GameObject currentPage, GameObject nextPage){}
    }
}