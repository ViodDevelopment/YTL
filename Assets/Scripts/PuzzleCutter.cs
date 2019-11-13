using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using System.Windows.Input;
using System.IO;
using UnityEditor;
using UnityEngine.UI;

public class PuzzleCutter : MonoBehaviour
{
    public Texture2D m_ImagePuzzle;
    public GameObject m_Renderer;
    public GameObject m_Canvas;

    int m_Difficulty = 4;
    
    void Start()
    {
        CutIntoPieces();
    }

    public void CutIntoPieces()
    {
        int l_NumPieces = 2;
        int l_WidhtPiece = m_ImagePuzzle.width / l_NumPieces;
        int l_HeightPeice = m_ImagePuzzle.height / l_NumPieces;

        for (int i = 0; i < l_NumPieces; i++)
        {
            for (int j = 0; j < l_NumPieces; j++)
            {
                
                Debug.Log(i);
                Debug.Log(j);
                print("new one");
                Sprite l_Sprite;
                Rect rect = new Rect(new Vector2(i * l_WidhtPiece, j * l_HeightPeice), new Vector2(l_WidhtPiece, l_HeightPeice));
                l_Sprite = Sprite.Create(m_ImagePuzzle, rect, new Vector2(0,0));

                GameObject l_NewOne = Instantiate(m_Renderer, m_Canvas.transform);
                l_NewOne.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * l_WidhtPiece*1.5f, j * l_HeightPeice*1.5f);
                l_NewOne.GetComponent<Image>().sprite = l_Sprite;
            }
        }
        
    }
}
