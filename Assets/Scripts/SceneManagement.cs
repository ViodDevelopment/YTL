using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public GameObject ConfCanvas;

    public GameObject AñadirPalabraCanvas;

    public GameObject EditarCanvas;

    public GameObject MainMenuCanvas;

    public GameObject AccesToConf;

    public GameObject CameraCanvas;

    public GameObject ConfiguracionPestaña;

    public GameObject[] AllCanvas;

    private int numOfGames = 0;

    public void InicioScene(bool _DesdeActividad)
    {
        if (_DesdeActividad)
        {
            GameManager.fallosParejas = 0;
            GameManager.fallosPuzzle = 0;
            GameManager.backFromActivity = true;
        }
        SceneManager.LoadScene(GameManager.Instance.InicioIndex);
    }
    public void PreparadosScene()
    {
        SceneManager.LoadScene(GameManager.Instance.PreparadosIndex);
    }
    public void ListosScene()
    {
        SceneManager.LoadScene(GameManager.Instance.ListosIndex);
    }
    public void YaScene()
    {
        SceneManager.LoadScene(GameManager.Instance.YaIndex);
    }
    public void ConfScene()
    {
        DisableAllCanvas();
        ConfCanvas.SetActive(true);
    }
    public void ConfiguarcionScene()
    {
        DisableAllCanvas();
        ConfiguracionPestaña.SetActive(true);
    }
    public void MainMenuScene()
    {
        DisableAllCanvas();
        ConfCanvas.SetActive(true);
    }
    public void AddWordScene()
    {
        DisableAllCanvas();
        ConfCanvas.SetActive(true);
    }

    public void AddWordCanvas()
    {
        DisableAllCanvas();
        ConfCanvas.SetActive(true);

        ConfCanvas.transform.Find("ConfPestaña").gameObject.SetActive(false);
        ConfCanvas.transform.Find("AñadirPalabra").gameObject.SetActive(true);
    }


    public void CanvasCamera()
    {
        DisableAllCanvas();
        CameraCanvas.SetActive(true);
    }

    public void AccesToConfiguration()
    {
        DisableAllCanvas();
        AccesToConf.SetActive(true);
    }

    public void MinijuegoGusanosScene()
    {
        SceneManager.LoadScene(4);
    }

    public void MinijuegoBurbujasScene()
    {
        SceneManager.LoadScene(5);
    }
    public void MinijuegoColorScene()
    {
        SceneManager.LoadScene(6);
    }

    public void CameraScene()
    {
        SceneManager.LoadScene(6);
    }

    public void LoadParejas(int _level)
    {
        //SceneManager.LoadScene("LoadingScene");
        if (!gameObject.GetComponent<LoadingScene>().started)
        {
            if (_level == 1)
                GameManager.loadingScene = SceneManager.GetSceneByName("Parejas").buildIndex;
            else if (_level == 2)
                GameManager.loadingScene = 8;
            else if (_level == 3)
                GameManager.loadingScene = 9;

            gameObject.GetComponent<LoadingScene>().started = true;
        }

    }

    public void LoadPuzzle(int _level)
    {
        //SceneManager.LoadScene("LoadingScene");
        if (!gameObject.GetComponent<LoadingScene>().started)
        {
            if (_level == 1)
                GameManager.loadingScene = SceneManager.GetSceneByName("Puzzle").buildIndex;
            else if (_level == 2)
                GameManager.loadingScene = 11;
            else if (_level == 3)
                GameManager.loadingScene = 12;

            gameObject.GetComponent<LoadingScene>().started = true;
        }
    }

    public void LoadBit(int _level)
    {
        //SceneManager.LoadScene("LoadingScene");
        if (!gameObject.GetComponent<LoadingScene>().started)
        {
            if (_level == 1)
                GameManager.loadingScene = SceneManager.GetSceneByName("Bit").buildIndex;
            else if (_level == 2)
                GameManager.loadingScene = 6;
            else if (_level == 3)
                GameManager.loadingScene = 7;

            gameObject.GetComponent<LoadingScene>().started = true;
        }

    }

    public void WebButton()
    {
        Application.OpenURL("http://yotambienleo.com/recomendaciones/");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void DisableAllCanvas()
    {
        for (int i = 0; i < AllCanvas.Length; i++)
        {
            AllCanvas[i].SetActive(false);
        }
    }

    public void NextGame()
    {

        switch (GameManager.currentMiniGame)
        {
            case 0:
                MinijuegoBurbujasScene();
                break;
            case 1:
                MinijuegoGusanosScene();
                break;
            case 2:
                MinijuegoColorScene();
                break;

        }
        GameManager.currentMiniGame++;
        if (GameManager.currentMiniGame >= 2)
            GameManager.currentMiniGame = 0;
        ManagamentFalseBD.management.SaveBolasMinijuegos();
    }

    public void GoBackFromActivity()
    {
        AllCanvas[2].SetActive(true);
        AllCanvas[4].SetActive(true);
        AllCanvas[0].SetActive(false);
        AllCanvas[3].SetActive(false);
    }

    private void Start()
    {
        if (GameManager.backFromActivity)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                GoBackFromActivity();
                GameManager.backFromActivity = false;
            }
        }
    }
}
