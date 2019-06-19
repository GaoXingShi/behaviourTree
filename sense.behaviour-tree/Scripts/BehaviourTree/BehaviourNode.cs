using UnityEngine;

namespace Sense.BehaviourTree
{
    [System.Serializable]
    public enum NodeState {
        Disable,
        Ready,
        Running,
        Succeed,
        Failed,
    }

    [DisallowMultipleComponent]
    public class BehaviourNode : MonoBehaviour {
        /// <summary>
        /// 任务是否正在执行中
        /// </summary>
        private NodeState state;

        public NodeState State
        {
            get => state;
            protected set
            {
                NodeState temp = state;
                state = value;
                StateChanged(temp, state);
            }
        }

        // 当节点状态发生改变时
        public delegate void StateChange(NodeState _beforeState, NodeState _afterState);
        public event StateChange StateChanged = (_beforeState, _afterState) => { };

        /// <summary>
        /// 执行节点
        /// </summary>
        public virtual void Execute () {
            state = NodeState.Running;
        }

        /// <summary>
        /// 重置节点
        /// </summary>
        public virtual void Reset () {
            state = NodeState.Ready;
        }
        
        /// <summary>
        /// 中止节点
        /// </summary>
        public virtual void Abort(NodeState _state)
        {
            state = _state;
        }

        

    }
}