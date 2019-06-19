using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Sense.BehaviourTree
{
    public class ConsoleBehaviour : MonoBehaviour
    {

        public GameObject intoCanvas;

        private Text intoText;
        private StringBuilder cacheStringBuilder;
        private bool isEnterDown = false;
        private List<string> tempCacheStrings = new List<string>();
        private int cursorNumber = 0;
        private CommandWithDealScript commander;

        private void Awake()
        {
            commander = GetComponent<CommandWithDealScript>();
            intoText = intoCanvas.transform.Find("IntoBackageImage").Find("IntoText").GetComponent<Text>();

            if (!intoCanvas || !intoText)
            {
                enabled = false;
                return;
            }

            cacheStringBuilder = new StringBuilder("");
            intoText.text = cacheStringBuilder.ToString();
        }
        
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                intoCanvas.SetActive(false);
                isEnterDown = false;
                ClearInputText();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (isEnterDown)
                {
                    string temp = cacheStringBuilder.ToString();
                    if (temp.Equals(""))
                    {
                        return;
                    }

                    tempCacheStrings.Add(temp);
                    cursorNumber = tempCacheStrings.Count;
                    commander.CommandAnalysis(tempCacheStrings[tempCacheStrings.Count - 1]);
                }

                ClearInputText();

                if (!intoCanvas.activeSelf)
                {
                    intoCanvas.SetActive(true);
                    isEnterDown = true;
                }
            }

            if (isEnterDown)
            {
                string temp = Input.inputString;
                WaitInputText(temp);
                BackOrNextOneInputCommand();
            }
        }

        private void WaitInputText(string _inputString)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || _inputString.Equals(""))
            {
                return;
            }

            if (_inputString.Equals("\b"))
            {
                if (cacheStringBuilder.Length != 0)
                    cacheStringBuilder.Remove(cacheStringBuilder.Length - 1, 1);
            }
            else
            {
                cacheStringBuilder.Append(_inputString);
            }
            intoText.text = cacheStringBuilder.ToString();
            
        }

        private void ClearInputText()
        {
            cacheStringBuilder.Remove(0, cacheStringBuilder.Length);
            intoText.text = cacheStringBuilder.ToString();
        }

        private void BackOrNextOneInputCommand()
        {
            bool upInput = Input.GetKeyDown(KeyCode.UpArrow), downInput = Input.GetKeyDown(KeyCode.DownArrow);
            if ((!upInput || !downInput) && (!upInput && !downInput))
            {
                return;
            }

            if (upInput)
            {
                cursorNumber = cursorNumber - 1 == -1 ? 0 : cursorNumber - 1;
            }

            if (downInput)
            {
                cursorNumber = cursorNumber + 1 == tempCacheStrings.Count ? cursorNumber : cursorNumber + 1;
            }

            ClearInputText();

            cacheStringBuilder.Append(tempCacheStrings[cursorNumber]);
            intoText.text = cacheStringBuilder.ToString();
        }
    }

}
