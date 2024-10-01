using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SRS.Utils.DataPersistence
{
	public class PersistentEntity : MonoBehaviour
	{
		[SerializeField] private string uniqueIdentifier = "";
		public string UniqueIdentifier
		{
			get => uniqueIdentifier;
		}

		private static Dictionary<string, PersistentEntity> EntityLookup = new();

		public object CaptureState()
		{
			Dictionary<string, object> state = new();

			foreach (IPersist persistable in GetComponents<IPersist>())
            {
                state[persistable.GetType().ToString()] = persistable.CaptureState();
            }

			return state;
		}

		public void RestoreState(object state)
		{
			Dictionary<string, object> stateDict = state.ToDictionary();
			
            foreach (IPersist persistable in GetComponents<IPersist>())
            {
                string typeString = persistable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                {
                    persistable.RestoreState(stateDict[typeString]);
                }
            }
		}

        private bool IsUnique(string candidate)
        {
            if (!EntityLookup.ContainsKey(candidate)) return true;

            if (EntityLookup[candidate] == this) return true;

            if (EntityLookup[candidate] == null)
            {
                EntityLookup.Remove(candidate);
                return true;
            }

            if (EntityLookup[candidate].UniqueIdentifier != candidate)
            {
                EntityLookup.Remove(candidate);
                return true;
            }

            return false;
        }

#if UNITY_EDITOR
        private void OnValidate()
		{
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");
            
            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            EntityLookup[property.stringValue] = this;
        }
#endif
	}
}