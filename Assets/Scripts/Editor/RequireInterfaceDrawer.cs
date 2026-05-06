using UnityEngine;
using UnityEditor;
using System.Linq;
using GiantLaserTest.Attributes;

namespace GiantLaserTest.Editor
{
    [CustomPropertyDrawer(typeof(RequireInterfaceAttribute))]
    public class RequireInterfaceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            RequireInterfaceAttribute reqAttribute = (RequireInterfaceAttribute)attribute;

            // Sprawdzamy, czy pole jest obiektem
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                // Rysujemy standardowe pole wyboru obiektu
                EditorGUI.BeginChangeCheck();

                Object obj = EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(Object), true);

                if (EditorGUI.EndChangeCheck())
                {
                    if (obj == null)
                    {
                        property.objectReferenceValue = null;
                    }
                    else
                    {
                        // Próbujemy wyciągnąć interfejs z GameObjectu lub bezpośrednio z obiektu
                        GameObject go = obj as GameObject;
                        Component component = go != null ? go.GetComponent(reqAttribute.InterfaceType) : null;

                        if (component != null)
                        {
                            property.objectReferenceValue = component;
                        }
                        else if (reqAttribute.InterfaceType.IsAssignableFrom(obj.GetType()))
                        {
                            property.objectReferenceValue = obj;
                        }
                        else
                        {
                            Debug.LogWarning($"Obiekt {obj.name} nie implementuje interfejsu {reqAttribute.InterfaceType.Name}!");
                            property.objectReferenceValue = null;
                        }
                    }
                }
            }
            else
            {
                // Jeśli atrybut zostanie użyty nad czymś innym niż obiekt
                EditorGUI.LabelField(position, label.text, "Użyj [RequireInterface] tylko z Object/MonoBehaviour");
            }
        }
    }
}