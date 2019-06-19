using System.Linq;
using UnityEngine;

namespace Sense.BehaviourTree
{

    /// <summary>
    /// 创建这个节点的时候需要传入一个节点队列。一个接一个的运行子节点。
    /// 如果子节点的状态是FAILED，那么它会将自己标识为FAILED并且直接返回；
    /// 如果子节点的状态是SUCCESS或者RUNNING，那么它会运行下一个节点。
    /// 只有所有的节点都标识为SUCCESS它会将自己的标识为SUCCESS并且返回，否则他会将自己标识为RUNNING。
    /// </summary>
    public class ParallelNode : BehaviourNode
    {
        protected System.Collections.Generic.List<BehaviourNode> nodes = new System.Collections.Generic.List<BehaviourNode>();

        #region OverrideMethod
        public override void Reset()
        {
            nodes = new System.Collections.Generic.List<BehaviourNode>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var tmp = transform.GetChild(i).GetComponent<BehaviourNode>();
                if (tmp && tmp.isActiveAndEnabled)
                {
                    nodes.Add(tmp);
                    tmp.Reset();
                }
            }
            State = NodeState.Ready;
        }

        public override void Execute()
        {
            foreach (var v in nodes)
            {
                if (v.isActiveAndEnabled)
                {
                    v.Execute();
                }
                else
                {
                    v.Abort(NodeState.Disable);
                }
            }
            State = NodeState.Running;
        }
        
        public override void Abort(NodeState _state)
        {
            foreach (var v in nodes.Where(x => x.State == NodeState.Ready || x.State == NodeState.Running))
            {
                v.Abort(_state);
            }
            base.Abort(_state);
        }
        #endregion
        
        #region TestMethod
        /// <summary>
        /// 初始化加执行方法
        /// </summary>
        [UnityEngine.ContextMenu("Rest And Execute")]
        public void Test01()
        {
            Reset();
            Execute();
        }
        /// <summary>
        /// 跳过该复合节点
        /// </summary>
        [UnityEngine.ContextMenu("Abort To Success")]
        public void Test02()
        {
            Abort(NodeState.Succeed);
        }
        /// <summary>
        /// 忽略该复合节点
        /// </summary>
        [UnityEngine.ContextMenu("Abort To Disable")]
        public void Test03()
        {
            Abort(NodeState.Disable);
        }

        #endregion

        private void FinishNode(NodeState _state, NodeState _otherNodeState)
        {
            State = _state;

            foreach (var v in nodes.Where(x => x.State == NodeState.Ready || x.State == NodeState.Running))
            {
                v.Abort(_otherNodeState);
            }
        }
        
        protected void Update()
        {
            if (State != NodeState.Running)
            {
                return;
            }

            bool isReturn = nodes.Count(x => x.State == NodeState.Succeed || x.State == NodeState.Disable) != nodes.Count;

            if (nodes.Any(x => x.State == NodeState.Failed))
            {
                FinishNode(NodeState.Failed,NodeState.Disable);
            }

            if (isReturn)
            {
                return;
            }

            FinishNode(NodeState.Succeed, NodeState.Succeed);
        }
    }


#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(ParallelNode))]
    public class ParallelNodeEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (Application.isPlaying)
            {
                var targetNode = target as ParallelNode;
                if (targetNode == null)
                {
                    return;
                }
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Rest And Execute"))
                {
                    targetNode.Test01();
                }
                if (GUILayout.Button("Abort To Success"))
                {
                    targetNode.Test02();
                }
                if (GUILayout.Button("Abort To Disable"))
                {
                    targetNode.Test03();
                }
                GUILayout.EndHorizontal();

            }
        }
    }
#endif
}