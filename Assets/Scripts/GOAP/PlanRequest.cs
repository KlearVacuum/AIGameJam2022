using System.Collections.Generic;

namespace GOAP
{
    class PlanRequest
    {
        private Dictionary<string, IStateValue> m_DesiredState;
        public Dictionary<string, IStateValue> DesiredState => m_DesiredState;
   
        public PlanRequest(Dictionary<string, IStateValue> desiredState)
        {
            m_DesiredState = desiredState;
        }
    }
}