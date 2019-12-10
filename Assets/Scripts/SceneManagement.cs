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
        SceneManager.LoadScene(7);
    }

    public void LoadParejas(int _level)
    {
        if (_level == 1)
            SceneManager.LoadScene(2);
        else if(_level == 2)
            SceneManager.LoadScene(8);
        else if (_level == 3)
            SceneManager.LoadScene(9);
    }

    public void LoadPuzzle(int _level)
    {
        if (_level == 1)
            SceneManager.LoadScene(3);
    }

    public void LoadBit(int _level)
    {
        if (_level == 1)
            SceneManager.LoadScene(1);
        else if (_level == 2)
            SceneManager.LoadScene(6);
        else if(_level == 3)
            SceneManager.LoadScene(7);

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
