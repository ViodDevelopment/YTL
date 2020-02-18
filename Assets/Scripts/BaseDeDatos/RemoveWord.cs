using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveWord : MonoBehaviour
{
    public PalabraBD palabraSeleccionada;
    public Button button;
    ManagamentFalseBD managamentFalseBD;

    public DropDown dropdown;

    private void Start()
    {
        managamentFalseBD = GameObject.Find("ManagementFalsaBD").GetComponent<ManagamentFalseBD>();
    }

    private void Update()
    {
        if (palabraSeleccionada != null)
            button.interactable = true;
        else button.interactable = false;

    }


    public void RemovePalabraBD()
    {
        if (palabraSeleccionada != null)
        {
            managamentFalseBD.SaveWordUser(palabraSeleccionada, false);
            palabraSeleccionada = null;
            dropdown.Unselected();
        }
    }
}
