using System.Linq;
using UnityEngine;

namespace Sense.BehaviourTree
{
    /// <summary>
    /// 创建这个节点时，需要传入一个节点队列。当运行到这个节点时。他的子节点会一个接一个的运行。
    /// 如果他的子节点状态是SUCCESS，那么他会运行下一个；
    /// 如果他的子节点状态是RUNNING，那么他会将自身也标识成RUNNING，并且等待节点返回其他结果；
    /// 如果他的子节点状态是FAILED，那么他会把自己的状态标识为FAILED并且直接返回。
    /// 如果所有节点都返回结尾为SUCCESS，那么他会将自身标识成为SUCCESS并且返回。
    /// </summary>
    public class SequenceNode : BehaviourNode
    {
        protected System.Collections.Generic.List<BehaviourNode> nodes = new System.Collections.Generic.List<BehaviourNode>();
        private int currentNodeNumber = 0;

        #region OverrideMethod
        public override void Execute()
        {
            if (nodes.Count != 0)
            {
                nodes[currentNodeNumber].Execute();
            }
            base.Execute();
        }

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

            currentNodeNumber = 0;
            base.Reset();
        }
        
        public override void Abort(NodeState _state)
        {
            currentNodeNumber = 0;
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

        protected void Update()
        {
            if (State != NodeState.Running)
            {
                return;
            }

            // Node节点关闭的话就会跳过。
            if (!nodes[currentNodeNumber].isActiveAndEnabled)
            {
                nodes[currentNodeNumber].Abort(NodeState.Disable);
                NodeNumberPlusPlus();
                return;
            }

            // 检测Node的状态
            if (nodes[currentNodeNumber].State == NodeState.Succeed || nodes[currentNodeNumber].State == NodeState.Disable)
            {
                NodeNumberPlusPlus();
            }
            else if (nodes[currentNodeNumber].State == NodeState.Failed)
            {
                FinishNode(NodeState.Failed, NodeState.Disable);
            }

        }

        private void NodeNumberPlusPlus()
        {
            currentNodeNumber++;
            if (currentNodeNumber == nodes.Count)
            {
                FinishNode(NodeState.Succeed, NodeState.Succeed);
            }
            else
            {
                nodes[currentNodeNumber].Execute();
            }
        }

        private void FinishNode(NodeState _state, NodeState _otherNodeState)
        {
            currentNodeNumber = 0;
            State = _state;

            foreach (var v in nodes.Where(x => x.State == NodeState.Ready || x.State == NodeState.Running))
            {
                v.Abort(_otherNodeState);
            }
        }

    }

    #if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(SequenceNode))]
    public class SeuqenceNodeEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (Application.isPlaying)
            {
                var targetNode = target as SequenceNode;
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

