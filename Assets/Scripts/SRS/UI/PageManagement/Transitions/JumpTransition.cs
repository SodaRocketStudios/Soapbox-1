using System.Collections;
using UnityEngine;

namespace SRS.UI.PageManagement
{
	[CreateAssetMenu(menuName = "Page Transitions/Jump Transition", fileName = "New Jump Transition")]
    public class JumpTransition : PageTransition
    {
        public override IEnumerator Animate(GameObject page)
        {
			yield break;
        }
    }
}