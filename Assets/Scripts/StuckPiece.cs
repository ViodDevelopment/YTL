using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckPiece : MonoBehaviour
{ 
    bool m_PieceLocked = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.name == this.gameObject.name) && (Input.touchCount == 0 && Input.GetMouseButtonUp(0)) && !m_PieceLocked)
        {
            this.transform.position = collision.gameObject.transform.position;
            m_PieceLocked = true;
            GetComponentInParent<MoveTouch>().m_PieceLocked = true;
            GameObject.FindGameObjectWithTag("GameManagerPuzzle").GetComponent<GameManagerPuzzle>().m_Puntuacion++;
        }
    }
}
