using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MathOperationController : MonoBehaviour
{
    public enum SIGN { PLUS, SUBSTRACT, MULTIPLY, DIVISION };

    //public SIGN sign;

    //public int result, a, b;

    string signStr;

    public InputField inputResult;

    public Text operationTxt;

    public Button confirmBtn;

    public GameObject sceneManagerGO;

    SceneManagement sceneManager;

    [System.Serializable]
    public struct operation
    {
        public int a, b;
        public SIGN sign;
    }

    int indexOper;

    int result;

    // public operation[] operations;

    public operation oper;

    void Start()
    {

        sceneManager = sceneManagerGO.GetComponent<SceneManagement>();

        //NewOperation();
        RandOperation();

        confirmBtn.onClick.AddListener(delegate { CompareResult(); });


    }

    void CompareResult()
    {
        if (result.ToString() == inputResult.text)
        {
            sceneManager.ConfScene();
        }
        else
        {
            RandOperation();
            //NewOperation();
        }
    }

    //void NewOperation()
    //{

    //    indexOper = Random.Range(0, operations.Length);

    //    switch (operations[indexOper].sign)
    //    {
    //        case SIGN.PLUS:
    //            signStr = "+";
    //            result = operations[indexOper].a + operations[indexOper].b;
    //            break;
    //        case SIGN.SUBSTRACT:
    //            signStr = "-";
    //            result = operations[indexOper].a - operations[indexOper].b;
    //            break;
    //        case SIGN.DIVISION:
    //            signStr = "/";
    //            result = operations[indexOper].a / operations[indexOper].b;
    //            break;
    //        case SIGN.MULTIPLY:
    //            signStr = "*";
    //            result = operations[indexOper].a * operations[indexOper].b;
    //            break;
    //    }

    //    operationTxt.text = operations[indexOper].a + " " + signStr + " " + operations[indexOper].b + " = ";
    //    inputResult.text = "\0";
    //}

    void RandOperation()
    {
        SIGN sign = (SIGN)Random.Range(0, 2);
        switch (sign)
        {
            case SIGN.PLUS:
                oper.a = Random.Range(5, 50);
                oper.b = Random.Range(5, 50);
                result = oper.a + oper.b;
                signStr = "+";
                break;
            case SIGN.MULTIPLY:
                oper.a = Random.Range(5, 25);
                oper.b = Random.Range(2, 4);
                result = oper.a * oper.b;
                signStr = "*";
                break;
            case SIGN.SUBSTRACT:
                oper.a = Random.Range(30, 50);
                oper.b = Random.Range(5, 29);
                result = oper.a - oper.b;
                signStr = "-";
                break;
            case SIGN.DIVISION:
                oper.a = Random.Range(30, 50);
                oper.b = Random.Range(2, 5);
                result = oper.a / oper.b;
                signStr = "/";
                break;
        }

        operationTxt.text = oper.a + " " + signStr + " " + oper.b + " = ";
        inputResult.text = "\0";
    }


}