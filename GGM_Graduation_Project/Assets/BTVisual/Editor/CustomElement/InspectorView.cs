using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace BTVisual
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits { }

        private Editor _editor;
        public InspectorView()
        {

        }

        internal void UpdateSelection(NodeView nv)
        {
            Clear(); // 인스펙터뷰에 있는 모든 UI툴킷을 전부 삭제

            UnityEngine.Object.DestroyImmediate(_editor);
            _editor = Editor.CreateEditor(nv.node); // 유니티 기본 인스펙터뷰를 만들어준다

            var container = new IMGUIContainer(() => {
                if (_editor.target)
                {
                    _editor.OnInspectorGUI();
                }
            });

            Add(container);
        }
    }
}
