using UnityEngine;

public class Highlight : MonoBehaviour
{
    Color m_MouseOverColor;
    Color m_OriginalColor;

    MeshRenderer m_Renderer;

    void Start()
    {
        m_Renderer = GetComponent<MeshRenderer>();
        m_OriginalColor = m_Renderer.material.color;

        m_MouseOverColor = m_OriginalColor * 1.5f;
        m_MouseOverColor.a = m_OriginalColor.a;
    }

    void OnMouseOver()
    {
        m_Renderer.material.color = m_MouseOverColor;
    }

    void OnMouseExit()
    {
        m_Renderer.material.color = m_OriginalColor;
    }
}
