using UnityEngine;
using UnityEditor;

namespace SRS.Extensions.AnimationCurves.Editor
{
	[CustomPropertyDrawer(typeof(CurveAttribute))]
	public class AnimationCurveDrawer: PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			float height = (attribute as CurveAttribute).Height;

			AnimationCurve curve = property.animationCurveValue;

			Rect range = new(0, 0, curve.keys[curve.length-1].time, curve.keys[curve.length-1].value*1.25f);

			EditorGUI.indentLevel = 0;

			position.height = height*EditorGUIUtility.singleLineHeight;

			EditorGUI.CurveField(position, property, Color.green, range);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return (attribute as CurveAttribute).Height*EditorGUIUtility.singleLineHeight;
		}
	}
}
