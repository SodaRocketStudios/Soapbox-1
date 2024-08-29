using System.Collections;
using UnityEngine;
using SRS.Extensions.Vector;
using SRS.Utils.Curves;

namespace SRS.UI.PageManagement
{
	[CreateAssetMenu(menuName = "Page Transitions/Slide Transition")]
    public class SlideTransition : PageTransition
    {
		[SerializeField, Min(0.1f)] private float slideTime;

		[SerializeField] private Vector2 startPosition;
		[SerializeField] private Vector2 endPosition;

		private SigmoidCurve animationCurve = new SigmoidCurve(1, 0.5f, 0, 10);

        public override IEnumerator Animate(GameObject page)
        {
			RectTransform transform = page.GetComponent<RectTransform>();

			float t = transform.anchoredPosition.InverseLerp(startPosition, endPosition);

			if(t >= 1)
			{
				t = 0;
			}

            while(t <= 1)
			{
				float step = animationCurve.Evaluate(t);
				transform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, step);
				t += Time.deltaTime/slideTime;
				yield return null;
			}
        }
    }
}