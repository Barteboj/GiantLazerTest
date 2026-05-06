using UnityEngine;
using UnityEditor;
using GiantLaserTest.Attributes;

namespace GiantLaserTest.Editor
{
    [CustomPropertyDrawer(typeof(RequireInterfaceAttribute))]
    public class RequireInterfaceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            RequireInterfaceAttribute requireInterfaceAttribute = (RequireInterfaceAttribute)attribute;

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                EditorGUI.BeginChangeCheck();

                Object assignedObject = EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(Object), true);

                if (EditorGUI.EndChangeCheck())
                {
                    UpdatePropertyValue(property, requireInterfaceAttribute, assignedObject);
                }
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use [RequireInterface] only with Object/MonoBehaviour");
            }
        }

        private void UpdatePropertyValue(SerializedProperty property, RequireInterfaceAttribute requireInterfaceAttribute, Object assignedObject)
        {
            if (assignedObject == null)
            {
                property.objectReferenceValue = null;
            }
            else
            {
                GameObject go = assignedObject as GameObject;
                Component component = go != null ? go.GetComponent(requireInterfaceAttribute.InterfaceType) : null;

                if (component != null)
                {
                    property.objectReferenceValue = component;
                }
                else if (requireInterfaceAttribute.InterfaceType.IsAssignableFrom(assignedObject.GetType()))
                {
                    property.objectReferenceValue = assignedObject;
                }
                else
                {
                    Debug.LogWarning($"Object {assignedObject.name} does not implement interface {requireInterfaceAttribute.InterfaceType.Name}");
                    property.objectReferenceValue = null;
                }
            }
        }
    }
}