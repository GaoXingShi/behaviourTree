using System.Linq;

namespace Sense.BehaviourTree.MouseDemo
{
    public class TaskMouseClickNode : BehaviourNode
    {
        public MouseClickColliderScript mouseClick;

        void Update()
        {
            if (State != NodeState.Running)
            {
                return;
            }

            if (!mouseClick.IsRunning)
            {
                NodeToDisabledTrigger();
                if (mouseClick.isSuccessed)
                {
                    State = NodeState.Succeed;
                }
                else
                {
                    State = NodeState.Failed;
                }

            }
        }
        
        public override void Execute()
        {
            NodeToEnableTrigger();

            base.Execute();
        }

        public override void Reset()
        {
            NodeToDisabledTrigger();
            base.Reset();
        }
        /// <summary>
        /// 急停节点时
        /// </summary>
        public override void Abort(NodeState _state)
        {
            NodeToDisabledTrigger();
            base.Abort(_state);
        }

        private void NodeToDisabledTrigger()
        {
            mouseClick.DisableTrigger();
        }

        private void NodeToEnableTrigger()
        {
            mouseClick.EnableTrigger();
        }

    }



}
