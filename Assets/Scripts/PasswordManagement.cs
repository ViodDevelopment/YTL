using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PasswordManagement : MonoBehaviour
{
    List<char> password = new List<char>();
    List<char> checkPassword = new List<char>();
    List<char> oldPassword = new List<char>();

   
    string stars = "";
    [SerializeField] GameObject newPasLayout, changePasLayout, logInLayout;

    [Header("Debug")]
    [SerializeField] InputField oldPasswordInput;
    [SerializeField] InputField newPasswordInput;
    [SerializeField] InputField confirmPasswordInput;
    [SerializeField] Button ConfirmarBtn;


    private void Start()
    {
        if(password.Count == 0)
        {
            NewPassword();
        }
    }

    void NewPassword()
    {
        newPasLayout.SetActive(true);
        changePasLayout.SetActive(false);
        logInLayout.SetActive(false);

        newPasswordInput = newPasLayout.transform.Find("NewPassword").gameObject.GetComponent<InputField>();
        confirmPasswordInput = newPasLayout.transform.Find("ConfirmNewPassword").gameObject.GetComponent<InputField>();
        ConfirmarBtn = newPasLayout.transform.Find("ConfirmarBtn").gameObject.GetComponent<Button>();

        newPasswordInput.onValueChanged.RemoveAllListeners();
        confirmPasswordInput.onValueChanged.RemoveAllListeners();

        newPasswordInput.onValueChanged.AddListener(delegate { AddPassword(password, newPasswordInput); });       
        confirmPasswordInput.onValueChanged.AddListener(delegate { AddPassword(checkPassword, confirmPasswordInput); });


    }

    void ChangePassword()
    {
        newPasLayout.SetActive(false);
        changePasLayout.SetActive(true);
        logInLayout.SetActive(false);

        oldPasswordInput = changePasLayout.transform.Find("OldPassword").gameObject.GetComponent<InputField>();
        newPasswordInput = changePasLayout.transform.Find("NewPassword").gameObject.GetComponent<InputField>();
        confirmPasswordInput = changePasLayout.transform.Find("ConfirmNewPassword").gameObject.GetComponent<InputField>();
        ConfirmarBtn = changePasLayout.transform.Find("ConfirmarBtn").gameObject.GetComponent<Button>();

        newPasswordInput.onValueChanged.RemoveAllListeners();
        confirmPasswordInput.onValueChanged.RemoveAllListeners();
        newPasswordInput.onValueChanged.RemoveAllListeners();

        newPasswordInput.onValueChanged.AddListener(delegate { AddPassword(password, newPasswordInput); });   
        confirmPasswordInput.onValueChanged.AddListener(delegate { AddPassword(checkPassword, confirmPasswordInput); });       
        oldPasswordInput.onValueChanged.AddListener(delegate { AddPassword(oldPassword, oldPasswordInput); });

    }

    void LogIn()
    {
        newPasLayout.SetActive(false);
        changePasLayout.SetActive(false);
        logInLayout.SetActive(true);

        newPasswordInput = logInLayout.transform.Find("NewPassword").gameObject.GetComponent<InputField>();
        confirmPasswordInput = logInLayout.transform.Find("ConfirmNewPassword").gameObject.GetComponent<InputField>();
        ConfirmarBtn = logInLayout.transform.Find("ConfirmarBtn").gameObject.GetComponent<Button>();

        confirmPasswordInput.onValueChanged.RemoveAllListeners();
        confirmPasswordInput.onValueChanged.AddListener(delegate { AddPassword(password, newPasswordInput); });
    }
   

    void AddPassword(List<char> str, InputField inputField)
    {
        try
        {
            if (inputField.text[inputField.text.Length - 1] == '*') return;
            if (inputField.text.Length >= str.Count)
            {
                str.Add(inputField.text[inputField.text.Length - 1]);
            }
            else if (str.Count > 0)
            {
                str.RemoveAt(str.Count - 1);
            }

            stars = "";
            for (int i = 0; i < inputField.text.Length; i++)
            {
                stars += "*";
            }
            
            StartCoroutine(comprovar(inputField));
        }
        catch
        {
           
        }
    }

   

    public void SetPassword(string newPass)
    {
        password = new List<char>(newPass.Length);
        for (int i = 0; i < newPass.Length; i++)
        {
            password[i] = newPass[i];
        }

    }

    IEnumerator comprovar(InputField inputField)
    {
        yield return new WaitForSeconds(0.2f);
        inputField.text = stars;
    }
}
