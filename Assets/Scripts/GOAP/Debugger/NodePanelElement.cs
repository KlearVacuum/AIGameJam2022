using System;
using UnityEngine;
using TMPro;

namespace GOAP
{
    public class NodePanelElement : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_Content;

        public void SetContent(string text)
        {
            m_Content.text = text;
        }

        public void SetTextColor(Color color)
        {
            m_Content.color = color;
        }
    }
}
