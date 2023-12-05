
/*
 상태 관리 (FSM)
1. FSM 클래스 (FSM<T>)
    멤버 변수:
        private Dictionary<string, T> states: 상태를 저장하는 딕셔너리.
        private T currentState: 현재 상태를 나타내는 변수.
    멤버 함수:
        public void AddState(string name, T state): 상태를 추가하는 함수.
        public void ChangeState(string name): 현재 상태를 변경하는 함수.
        public void Update(): 현재 상태를 업데이트하는 함수.

2. IState 인터페이스
    함수:
        void Enter(): 상태 진입 시 호출되는 함수.
        void Update(): 상태 업데이트 시 호출되는 함수.
        void Exit(): 상태 종료 시 호출되는 함수.

3. 각 상태 클래스 (예: MoveState)
    IState 인터페이스를 구현하여 각 상태의 특징에 맞게 Enter, Update, Exit 함수를 구현.


+------------------------+           +------------------+
|        FSM<T>          |           |      IState      |
+------------------------+           +------------------+
| - states: Dictionary   |           | + Enter()        |
| - currentState: T      |<--------->| + Update()       |
+------------------------+           | + Exit()         |
                                     +------------------+

FSM<T>는 상태를 저장하는 states 딕셔너리와 현재 상태를 나타내는 currentState 변수를 보유.
각 상태는 IState 인터페이스를 구현하며, Enter(), Update(), Exit() 함수를 보유.



행동 관리 (Behaviour Tree)
1. BehaviourTreeManager 클래스 (BehaviourTreeManager<T>)

    멤버 변수:
        private T root: 행동 트리의 루트 노드를 저장하는 변수.
    함수:
        public void SetRoot(T rootNode): 루트 노드를 설정하는 함수.
        public void Update(): 행동 트리를 업데이트하는 함수.

2. INode 인터페이스

    함수:
        NodeState Execute(): 노드를 실행하고 결과 상태를 반환하는 함수.

3. 각 노드 클래스 (예: SequenceNode, SelectorNode, ConditionNode, ActionNode)

    INode 인터페이스를 구현하여 각 노드의 특징에 맞게 Execute 함수를 구현.

+-------------------------------+           +-----------------+
|  BehaviourTreeManager<T>      |           |      INode      |
+-------------------------------+           +-----------------+
| - root: T                     |<--------->| + Execute()      |
+-------------------------------+           +-----------------+

BehaviourTreeManager<T>는 행동 트리의 루트를 나타내는 root 변수를 보유.
각 노드는 INode 인터페이스를 구현하며, Execute() 함수를 보유.


PlayerController
1. PlayerController 클래스
    멤버 변수:
        private FSM<YourStateType> stateManager: 플레이어 상태를 관리하는 FSM.
        private BehaviourTreeManager<INode> behaviourTreeManager: 플레이어 행동을 관리하는 행동 트리 매니저.
    함수:
        void Start(): 상태 관리자와 행동 트리 매니저를 초기화하는 함수.
        void Update(): 상태 관리자와 행동 트리 매니저를 업데이트하는 함수.
        다양한 조건 및 액션 함수들: 특정 상태 또는 노드에서 호출되는 함수들.

+--------------------------+
|     PlayerController     |
+--------------------------+
| - stateManager: FSM<T>   |
| - behaviourTreeManager:  |
|     BehaviourTreeManager |
+--------------------------+

PlayerController는 FSM<YourStateType>와 BehaviourTreeManager<INode>를 멤버 변수로 보유.
이 두 매니저를 초기화하고 업데이트하는 함수들 보유.

*/