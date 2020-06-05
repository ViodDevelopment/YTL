using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CreateWord : MonoBehaviour
{
    public Image img;

    public Text word;

    public GameObject bloqueSilabas;

    public AudioSource audioClip;

    public string palabraSilabas;

    List<InputField> silabas = new List<InputField>();

    Button thisButton;
    public DropDownArticles articlesDet;
    public DropDownArticlesIndet articlesIndet;
    // Start is called before the first frame update
    void Start()
    {
        thisButton = GetComponent<Button>();
        for (int i = 0; i < bloqueSilabas.transform.childCount; i++)
        {
            GameObject l_temp = bloqueSilabas.transform.GetChild(i).gameObject;
            if (l_temp.name.Contains("LineaSilaba"))
                silabas.Add(l_temp.GetComponent<InputField>());
        }
    }

    private void Update()
    {
        palabraSilabas = null;
        foreach (InputField input in silabas)
            palabraSilabas += input.text;
        if (img.sprite != null && word.text != "" && audioClip.clip != null && word.text.ToLower() == palabraSilabas.ToLower())
        {
            if (articlesDet != null && articlesIndet != null)
            {
                if (articlesDet.buttonSelected != null && articlesIndet.buttonSelected != null)
                    thisButton.interactable = true;
                else thisButton.interactable = false;

            }
            else thisButton.interactable = false;

        }
        else thisButton.interactable = false;

    }

    public void Clear()
    {

    }

    void SaveWord()
    {

    }
}
