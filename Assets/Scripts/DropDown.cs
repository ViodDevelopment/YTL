﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDown : MonoBehaviour
{
    public RectTransform container;
    public bool isOpen;
    private bool lastOpen;
    private bool doing;
    public BotonDropDown buttonSelected;
    public List<BotonDropDown> buttons = new List<BotonDropDown>();
    public Sprite selected, unselected;
    RemoveWord removeWord;
    // Start is called before the first frame update
    void Start()
    {
        removeWord = gameObject.GetComponent<RemoveWord>();
        isOpen = false;
        lastOpen = isOpen;
        doing = false;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i > GameManager.palabrasUserDisponibles.Count - 1)
                buttons[i].gameObject.SetActive(false);
            else
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].SetPalabra(GameManager.palabrasUserDisponibles[i]);
            }

        }

        if (doing)
        {
            Vector3 l_scale = container.transform.localScale;
            if(isOpen)
                l_scale.y += Time.deltaTime * 12;
            else
                l_scale.y += -Time.deltaTime * 12;

            if (l_scale.y <= 0)
                l_scale.y = 0;
            else if (l_scale.y >= 1)
                l_scale.y = 1;
            container.localScale = l_scale;
            if (container.localScale.y == 0 || container.localScale.y == 1)
                doing = false;
        }
        else if (lastOpen != isOpen)
        {
            lastOpen = isOpen;
            doing = true;
        }
    }

    public void Open()
    {
        if (!doing)
            isOpen = !isOpen;
    }

    public void Selected(BotonDropDown _button)
    {
        if (buttonSelected != null)
        {
            buttonSelected.myButton.image.sprite = unselected;
            buttonSelected.myText.color = Color.black;
        }

        buttonSelected = _button;
        buttonSelected.myButton.image.sprite = selected;
        buttonSelected.myText.color = Color.white;

        removeWord.palabraSeleccionada = buttonSelected.palabraBD;
    }

    public void Unselected()
    {
        if (buttonSelected != null)
        {
            buttonSelected.myButton.image.sprite = unselected;
            buttonSelected.myText.color = Color.black;
        }
    }
}
