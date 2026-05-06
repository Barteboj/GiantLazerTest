using UnityEngine;

namespace GiantLaserTest.Core.LayoutValidation
{
    [CreateAssetMenu(fileName = "LayoutTemplateSO", menuName = "ScriptableObjects/LayoutTemplateSO", order = 0)]
    public class LayoutTemplateSO : ScriptableObject
    {
        [field: SerializeField]
        public LayoutTemplateElement[] TemplateElements { get; private set; }
    }
}