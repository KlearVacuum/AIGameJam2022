using System;
using UnityEngine;
using TMPro;

namespace GOAP
{
    class NodePanelElement : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_Content;

        public void SetContent(string text)
        {
            m_Content.text = text;
        }
    }
}
