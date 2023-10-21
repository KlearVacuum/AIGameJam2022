using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace GOAP
{
    class ActionDebugger : NodeDebugger
    {
        Action m_Action;
        public Action Action => m_Action;

        public ActionDebugger(Action action)
        {
            m_Action = action;
        }

        public string Stringify()
        {
            string debugString = m_Action.GetName() + "\n";

            var conditions = m_Action.GetPreconditions().Conditions;
            if (conditions.Count > 0)
            {
                debugString += String.Format("Conditions : \n {0}", StringifyStateDatas(conditions));
            }

            var results = m_Action.GetEffect().Results;
            if (results.Count > 0)
            {
                debugString += String.Format("Effects : \n {0}", StringifyStateDatas(results));
            }

            return debugString;
        }

        public List<NodePanelElement> CreateNodePanelElements(NodePanelElement prefab)
        {
            List<NodePanelElement> elements = new List<NodePanelElement>();

            elements.Add(CreateElement(prefab, m_Action.GetName()));

            {
                var conditions = m_Action.GetPreconditions().Conditions;
                if (conditions.Count > 0)
                {
                    elements.Add(CreateElement(prefab, "Conditions"));

                    foreach (IStateData condition in conditions)
                    {
                        elements.Add(CreateElement(prefab, StringifyStateData(condition)));
                    }
                }
            }

            {
                var results = m_Action.GetEffect().Results;
                if (results.Count > 0)
                {
                    elements.Add(CreateElement(prefab, "Effects"));

                    foreach (IStateData result in results)
                    {
                        elements.Add(CreateElement(prefab, StringifyStateData(result)));
                    }
                }
            }
            
            return elements;
        }

        private NodePanelElement CreateElement(NodePanelElement prefab, string content)
        {
            NodePanelElement newElement = GameObject.Instantiate(prefab);
            newElement.SetContent(content);
            return newElement;
        }
    }
}
