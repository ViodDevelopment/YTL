using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalabraFraseBit2 : MonoBehaviour
{
    public int numImage = 0;
    public AudioSource audioSource;
    public BitLvl2 bit;
    public float distance = 0;
    private float timer = 3;
    private bool doingAnimation = false;
    private Vector3 scaleOriginal;
    // Start is called before the first frame update
    void Start()
    {
        scaleOriginal = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (bit.currentWord == numImage)
        {
            if (numImage != 0)
            {
                if (!bit.rectanglesInScene[numImage - 1].GetComponent<PalabraFraseBit2>().audioSource.isPlaying)
                {
                    DoingCosas();
                }

            }
            else if (!bit.m_AS.isPlaying)
                DoingCosas();

        }
    }


    private void DoingCosas()
    {
        if (GameManager.configurartion.ayudaVisual)
            timer += Time.deltaTime;

        if (GameManager.configurartion.ayudaVisual)
        {
            if (!doingAnimation && timer >= Random.Range(0.75f, 2))
            {
                doingAnimation = true;
                timer = 0;
            }
            else if (doingAnimation && timer <= 1)
            {
                if (timer <= 0.5f)
                {
                    gameObject.transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) * 35;
                }
                else
                {
                    gameObject.transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) * 35;
                }
            }
            else if (doingAnimation && timer > 1)
            {
                timer = 0;
                doingAnimation = false;
                gameObject.transform.localScale = scaleOriginal;
            }
        }

        if (GameManager.GetInstance().InputRecieved())
        {
            Vector3 positionInput;
            if (Input.touchCount > 0)
            {
                bool clicada = false;
                for (int i = 0; i < Input.touchCount; i++)
                {
                    positionInput = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                    if ((new Vector2(positionInput.x, positionInput.y) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).magnitude <= distance && Mathf.Abs(positionInput.y - gameObject.transform.position.y) <= 1.2f)
                    {
                        audioSource.Play();
                        gameObject.transform.localScale = scaleOriginal;
                        timer = 0;
                        bit.currentWord++;
                        doingAnimation = false;
                        clicada = true;
                        break;
                    }

                }
            }
            else
            {
                positionInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if ((new Vector2(positionInput.x, positionInput.y) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).magnitude <= distance && Mathf.Abs(positionInput.y - gameObject.transform.position.y) <= 1.2f)
                {
                    audioSource.Play();
                    gameObject.transform.localScale = scaleOriginal;
                    timer = 0;
                    bit.currentWord++;
                    doingAnimation = false;
                }
            }
        }
        
    }
}
