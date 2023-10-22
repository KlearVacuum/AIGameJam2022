using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToggleSpeedUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_Text;
    private bool m_Toggle = false;

    public void Toggle()
    {
        m_Toggle = !m_Toggle;
        m_Text.text = (m_Toggle? ">>" : ">");
        Time.timeScale = (m_Toggle ? 2 : 1);
        // Debug.Log("Toggle : " + m_Toggle);
    }

}
