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
            Clear(); // �ν����ͺ信 �ִ� ��� UI��Ŷ�� ���� ����

            UnityEngine.Object.DestroyImmediate(_editor);
            _editor = Editor.CreateEditor(nv.node); // ����Ƽ �⺻ �ν����ͺ並 ������ش�

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
