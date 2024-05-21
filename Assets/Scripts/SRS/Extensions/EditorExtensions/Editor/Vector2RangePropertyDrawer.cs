using UnityEditor;
using UnityEngine;

namespace SRS.Extensions.EditorExtensions
{
	[CustomPropertyDrawer(typeof(Vector2RangeAttribute))]
	public class Vector2RangePropertyDrawer : PropertyDrawer
	{
		private const float LINE_SPACE = 2;

		private float lineHeight
		{
			get
			{
				return EditorGUIUtility.singleLineHeight + LINE_SPACE;
			}
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Vector2RangeAttribute range = attribute as Vector2RangeAttribute;

			EditorGUI.BeginProperty(position, label, property);

			EditorGUI.indentLevel = 0;

			EditorGUI.PrefixLabel(position, label);

			position.y += lineHeight;

			position.height = EditorGUIUtility.singleLineHeight;

			EditorGUI.indentLevel++;

			EditorGUI.BeginChangeCheck();

			Vector2 value = property.vector2Value;

			position.width -= 15;

			EditorGUI.LabelField(position, "X");
			position.x += 15;
			value.x = EditorGUI.Slider(position, value.x, range.XMin, range.XMax);

			position.x -= 15;
			position.y += EditorGUIUtility.singleLineHeight + LINE_SPACE;

			EditorGUI.LabelField(position, "Y");
			position.x += 15;
			value.y = EditorGUI.Slider(position, value.y, range.YMin, range.YMax);

			if(EditorGUI.EndChangeCheck())
			{
				property.vector2Value = value;
			}

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight * 3 + LINE_SPACE;
		}
	}
}