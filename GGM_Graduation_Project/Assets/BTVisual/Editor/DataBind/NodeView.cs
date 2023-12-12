using System;
using System.Collections;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BTVisual
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Node node;

        public Port input; // 입력노드
        public Port output;// 출력노드

        public Action<NodeView> OnNodeSelected;


        public NodeView(Node node) : base("Assets/BTVisual/Editor/DataBind/NodeView.uxml")
        {
            this.node = node;
            this.title = node.name;

            this.viewDataKey = node.guid;

            style.left = node.position.x;
            style.top = node.position.y;

            CreateInputPorts();
            CreateOutputPorts();
            SetupClasses();

            Label descLabel = this.Q<Label>("description");
            descLabel.bindingPath = "description";
            descLabel.Bind(new SerializedObject(node));
        }

        private void CreateInputPorts()
        {
            if (node is ActionNode)
            {
                input = InstantiatePort(
                    Orientation.Vertical, Direction.Input,
                    Port.Capacity.Single, typeof(bool));
            }
            else if (node is CompositeNode)
            {
                input = InstantiatePort(
                    Orientation.Vertical, Direction.Input,
                    Port.Capacity.Single, typeof(bool));

            }
            else if (node is DecoratorNode)
            {
                input = InstantiatePort(
                   Orientation.Vertical, Direction.Input,
                   Port.Capacity.Single, typeof(bool));
            }

            if (input != null)
            {
                input.portName = "";
                inputContainer.Add(input);
            }
        }

        private void CreateOutputPorts()
        {
            if (node is ActionNode)
            {
                // 액션은 아무것도 수행 안한다
            }
            else if (node is CompositeNode)
            {
                output = InstantiatePort(
                    Orientation.Vertical, Direction.Output,
                    Port.Capacity.Multi, typeof(bool));

            }
            else if (node is DecoratorNode)
            {
                output = InstantiatePort(
                    Orientation.Vertical, Direction.Output,
                    Port.Capacity.Single, typeof(bool));
            }
            else if (node is RootNode)
            {
                output = InstantiatePort(
                    Orientation.Vertical, Direction.Output,
                    Port.Capacity.Single, typeof(bool));
            }

            if (output != null)
            {
                output.portName = "";
                output.style.marginLeft = new StyleLength(-15);
                outputContainer.Add(output);
            }
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            Undo.RecordObject(node, "BT(SetPosition");

            node.position.x = newPos.xMin;
            node.position.y = newPos.yMin;

            EditorUtility.SetDirty(node); // 안해도 되긴 하는거임
        }

        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }

        public void SetupClasses()
        {
            if (node is ActionNode)
            {
                AddToClassList("action");
            }
            else if (node is CompositeNode)
            {
                AddToClassList("composite");
            }
            else if (node is DecoratorNode)
            {
                AddToClassList("decorator");
            }
            else if (node is RootNode)
            {
                AddToClassList("root");
            }
        }

        // 자식들을 정렬하는 기준
        public void SortChildren()
        {
            var composite = node as CompositeNode;
            if (composite != null)
            {
                composite.children.Sort(
                    (left, right) => left.position.x < right.position.x ? -1 : 1);
            }
        }

        // 구동중일때 노드의 뷰 상태를 변경하는거
        public void UpdateState()
        {
            if (Application.isPlaying)
            {
                RemoveFromClassList("running");
                RemoveFromClassList("failure");
                RemoveFromClassList("success");
                switch (node.state)
                {
                    case Node.State.RUNNING:
                        if (node.started)
                        {
                            AddToClassList("running");
                        }
                        break;
                    case Node.State.FAILURE:
                        AddToClassList("failure");
                        break;
                    case Node.State.SUCCESS:
                        AddToClassList("success");
                        break;
                }
            }
        }
    }
}
