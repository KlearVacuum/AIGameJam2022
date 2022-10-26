using UnityEngine;

[CreateAssetMenu(menuName = "Node/ActionNodePanelData")]
class ActionNodePanelData : ScriptableObject
{
    [SerializeField] ActionNode m_ActionNode;
    public ActionNode ActionNode => m_ActionNode;
}