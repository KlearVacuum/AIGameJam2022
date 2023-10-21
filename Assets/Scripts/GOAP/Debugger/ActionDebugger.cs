using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    }
}
