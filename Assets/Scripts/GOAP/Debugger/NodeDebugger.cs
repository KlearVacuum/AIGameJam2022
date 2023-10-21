using System.Collections.Generic;
using System;

namespace GOAP
{
    class NodeDebugger
    {
        protected string StringifyStateDatas(List<IStateData> stateDatas)
        {
            string debugString = "";

            foreach (IStateData stateData in stateDatas)
            {
                debugString += StringifyStateData(stateData);
                debugString += "\n";
            }

            return debugString;
        }

        protected string StringifyStateData(IStateData stateData)
        {
            string debugString = String.Format(
                " {0} ({1}) = {2}",
                stateData.Name,
                stateData.StringifyType(),
                stateData.GetStateValue().Stringify());

            return debugString;
        }
    }
}
