using UnityEngine;

namespace GOAP
{
    class GoToAction : Action
    {
        GameObject m_Target = null;

        public GoToAction(System.Type targetType)
        {
            GameObject targetObject = FindObjectOfType(targetType) as GameObject;

            if(targetObject != null)
            {
                m_Target = targetObject;
            }
        }

        public override bool CheckIfValid(Blackboard worldState)
        {
            throw new System.NotImplementedException();
        }

        public override void Execute(Agent agent)
        {
            throw new System.NotImplementedException();
        }

        public override string GetName()
        {
            throw new System.NotImplementedException();
        }
    }

    [CreateAssetMenu(menuName = "Planner/Actions/GoToAction")]
    class GoToActionData : ActionData
    {
        [SerializeField] System.Type m_TargetType;
        
        public override Action CreateAction()
        {
            return new GoToAction(m_TargetType);
        }
    }
}
