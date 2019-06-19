using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sense.BehaviourTree
{
    public class CommandWithDealScript : MonoBehaviour
    {
        private ConsoleBehaviour consoleBehaviour;
        private GameObject intoCanvas;
        private Text commandText;
        void Awake()
        {
            consoleBehaviour = GetComponent<ConsoleBehaviour>();
            intoCanvas = consoleBehaviour.intoCanvas;
            commandText = intoCanvas.transform.Find("IntoCommandImage/CommandText").GetComponent<Text>();
        }
        
        public void CommandAnalysis(string _str)
        {
            if (_str.Equals("Next"))
            {
                Type type = Type.GetType("CJBehaviourTree.TaskCompositeNode");
                type.GetMethod("FinishNode")
                    .Invoke(consoleBehaviour.transform.parent.Find("TaskNodes").GetChild(0).GetComponent<ConsoleBehaviour>(), new object[] { });

            }
            commandText.text += _str + "\n";
        }
    }
}