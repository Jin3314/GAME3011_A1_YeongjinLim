using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Block_Type
{
    Minimal,
    Quater,
    Half,
    Max
}
public class Resource : MonoBehaviour, IPointerClickHandler
{
    public bool m_Open = false;
    public Block_Type m_BlockType = Block_Type.Minimal;
    public int IndexX;
    public int IndexY;

    void Update()
    {
        if (m_Open)
        {
            switch (m_BlockType)
            {
                case Block_Type.Minimal:
                    GetComponent<Image>().color = new Color32(210, 105, 30, 255);
                    break;
                case Block_Type.Quater:
                    GetComponent<Image>().color = new Color32(255, 140, 0, 255);
                    break;
                case Block_Type.Half:
                    GetComponent<Image>().color = new Color32(255, 215, 0, 255);
                    break;
                case Block_Type.Max:
                    GetComponent<Image>().color = new Color32(255, 255, 0, 255);
                    break;
            }
        }
        else
        {
            GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject Manager = GameObject.Find("GameManager");
        if (Manager.GetComponent<GameManagerScript>().state == Mode.SCAN && !m_Open && Manager.GetComponent<GameManagerScript>().scanCount > 0)
        {
            Manager.GetComponent<GameManagerScript>().OpenBlock(IndexX, IndexY);

            m_Open = true;
        }

        else if  (Manager.GetComponent<GameManagerScript>().state == Mode.EXTRACT && m_Open && Manager.GetComponent<GameManagerScript>().extractCount > 0)
        {
            Manager.GetComponent<GameManagerScript>().ExtractBlock(IndexX, IndexY);
        }
    }
}
