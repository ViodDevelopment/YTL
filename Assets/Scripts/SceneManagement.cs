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

    public void InicioScene()
    {
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

    public void LoadParejas()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadPuzzle()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadBit()
    {
        SceneManager.LoadScene(1);
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

    public void RandomMinigame ()
    {
        int Rng = Random.Range(0, 2);

        switch (Rng)
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
    }
}
