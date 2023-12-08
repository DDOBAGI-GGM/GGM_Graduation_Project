using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BTVisual
{
    public class BehaviourTreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<BehaviourTreeView, UxmlTraits> { }
        public new class UxmlTraits : GraphView.UxmlTraits { }

        private BehaviourTree _tree;

        public Action<NodeView> OnNodeSelected;

        public BehaviourTreeView()
        {
            Insert(0, new GridBackground());

            //메뉴퓰레이터 (드래그이벤트 -> 마우스다운 + 마우스 무브 + 마우스 업 이벤트)

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            Undo.undoRedoPerformed += OnUndoRedoHandle;
        }

        private void OnUndoRedoHandle()
        {
            PopulateView(_tree);
            AssetDatabase.SaveAssets();
        }

        public void PopulateView(BehaviourTree tree)
        {
            _tree = tree;
            graphViewChanged -= OnGraphViewChanged; // 이전 이벤트 제거후

            DeleteElements(graphElements); // 기존에 그려졌던 애들을 전부 삭제

            graphViewChanged += OnGraphViewChanged; // 다시 이벤트 구독

            if (_tree.rootNode == null)
            {
                _tree.rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
                EditorUtility.SetDirty(_tree);
                AssetDatabase.SaveAssets();
            }

            _tree.nodes.ForEach(n => CreateNodeView(n));

            // 엣지 그려주는것
            tree.nodes.ForEach(n =>
            {
                var children = tree.GetChildren(n); // n의 자식을 가져온다
                NodeView parent = FindNodeView(n);
                children.ForEach(c =>
                {
                    NodeView child = FindNodeView(c); // 해당 자식 가져와서 
                    // 연결시작
                    Edge edge = parent.output.ConnectTo(child.input);
                    AddElement(edge);
                });
            });
        }

        // guid로 노드 엘레멘드 가져오는 함수
        private NodeView FindNodeView(Node node)
        {
            return GetNodeByGuid(node.guid) as NodeView;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(elem =>
                {
                    var nv = elem as NodeView;
                    if (nv != null)
                    {
                        _tree.DeleteNode(nv.node); // 실제 SO에서도 삭제해라
                    }

                    var edge = elem as Edge;
                    if (edge != null) // 연결선이 삭제된거다
                    {
                        NodeView parent = edge.output.node as NodeView;
                        NodeView child = edge.input.node as NodeView;

                        _tree.RemoveChild(parent.node, child.node);
                    }
                });
            }

            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge =>
                {
                    NodeView parent = edge.output.node as NodeView;
                    NodeView child = edge.input.node as NodeView;

                    _tree.AddChild(parent.node, child.node);
                    parent.SortChildren();
                });
            }

            if (graphViewChange.movedElements != null)
            {
                nodes.ForEach(n =>
                {
                    var view = n as NodeView;
                    view?.SortChildren();
                });
            }

            return graphViewChange;
        }

        private void CreateNodeView(Node n)
        {
            NodeView nv = new NodeView(n);
            nv.OnNodeSelected = OnNodeSelected;
            AddElement(nv);
        }

        private void CreateNode(Type t, Vector2 position)
        {
            Node node = _tree.CreateNode(t);
            node.position = position;
            CreateNodeView(node);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if (_tree == null)
            {
                evt.StopPropagation(); // 이벤트 전파 금지를 걸고
                return;
            }

            Vector2 nodePosition = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
            {
                var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
                foreach (var t in types)
                {
                    evt.menu.AppendAction($"[{t.BaseType.Name}] / {t.Name}",
                                            (a) => { CreateNode(t, nodePosition); });
                }
            }

            {
                var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
                foreach (var t in types)
                {
                    evt.menu.AppendAction($"[{t.BaseType.Name}] / {t.Name}",
                                            (a) => { CreateNode(t, nodePosition); });
                }
            }

            {
                var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
                foreach (var t in types)
                {
                    evt.menu.AppendAction($"[{t.BaseType.Name}] / {t.Name}",
                                            (a) => { CreateNode(t, nodePosition); });
                }
            }
        }

        // 드래깅이 시작될 때 연결가능한 포트를 가져오는 함수
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList()
                .Where(x => x.direction != startPort.direction && x.node != startPort.node)
                .ToList();
        }

        public void UpdateNodeState()
        {
            nodes.ForEach(n =>
            {
                var nodeView = n as NodeView;
                nodeView?.UpdateState();
            });
        }
    }
}
