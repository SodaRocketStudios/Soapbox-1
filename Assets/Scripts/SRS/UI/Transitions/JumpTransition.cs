using System.Collections;
using UnityEngine;

namespace SRS.UI
{
	[CreateAssetMenu(menuName = "UI Transitions/Jump Transition", fileName = "New Jump Transition")]
    public class JumpTransition : UITransition
    {
        public override IEnumerator Animate(GameObject page)
        {
			yield break;
        }
    }
}