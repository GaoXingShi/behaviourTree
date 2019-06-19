using HighlightingSystem;
using UnityEngine;

namespace Sense.BehaviourTree.MouseDemo
{
    [RequireComponent(typeof(Highlighter))]
    public class MouseClickColliderScript : TriggerBehaviour
    {
        [HideInInspector] public bool isSuccessed = false;

        protected Highlighter high;
        protected Collider coll;
        protected bool isEnter = false;
        protected void Awake()
        {
            coll = GetComponent<Collider>();
            high = GetComponent<Highlighter>();
            if (!coll)
            {
                coll = gameObject.AddComponent<BoxCollider>();
            }
        }
        public override void EnableTrigger()
        {
            base.EnableTrigger();
            HighLightTrigger(true, Color.cyan);
        }

        public override void DisableTrigger()
        {
            base.DisableTrigger();
            isEnter = false;
            HighLightTrigger(false, Color.cyan);
        }
        

        private void Update()
        {
            if (!IsRunning || !isEnter)
            {
                return;
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                isSuccessed = true;
                DisableTrigger();
            }
            else if(Input.GetMouseButtonUp(1))
            {
                isSuccessed = false;
                DisableTrigger();
            }
        }

        private void OnMouseEnter()
        {
            if (!IsRunning)
            {
                return;
            }

            isEnter = true;
            HighLightTrigger(true, Color.green);
        }

        private void OnMouseExit()
        {
            if (!IsRunning)
            {
                return;
            }

            isEnter = false;
            HighLightTrigger(true, Color.cyan);
        }

        private void HighLightTrigger(bool _isConstant, Color _color)
        {
            high.constant = _isConstant;
            high.constantColor = _color;

        }

    }
}