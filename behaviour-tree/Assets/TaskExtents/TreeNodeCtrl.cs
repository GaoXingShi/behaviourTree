using System.Collections.Generic;
using UnityEngine;

namespace Sense.BehaviourTree
{
    public class TreeNodeCtrl : MonoBehaviour
    {
        private readonly List<BehaviourNode> rootNodes = new List<BehaviourNode>();
        private bool isClick = false;

        private void Start()
        {
            InitChildNodes();
        }

        private void InitChildNodes()
        {
            if (rootNodes.Count != 0)
            {
                return;
            }
            
            for (int i = 0; i < transform.childCount; i++)
            {
                BehaviourNode temp = transform.GetChild(i).GetComponent<BehaviourNode>();
                if (temp)
                {
                    rootNodes.Add(temp);
                }
            }
            
        }
        private void Update()
        {
            if (rootNodes.Count == 0)
            {
                return;
            }

            if (isClick && (rootNodes[0].State == NodeState.Succeed || rootNodes[0].State == NodeState.Failed))
            {
                isClick = false;
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Sense.BehaviourTree Demo");
            if (!isClick)
            {
                if (GUILayout.Button("Reset And Execute"))
                {
                    rootNodes[0].Reset();
                    rootNodes[0].Execute();
                    isClick = true;
                }
            }
            else
            {
                GUILayout.Label("鼠标左键点击方块为任务通过，鼠标右键点击方块表示任务失败");
            }

            GUILayout.EndVertical();
        }
    }
}