using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    private bool started = false;
    public Image carga;

    private void Update()
    {
        if(GameManager.loadingScene != -1 && !started)
            StartCoroutine(LoadNewScene());
        carga.fillAmount += Time.deltaTime * 1.5f;

    }

    IEnumerator LoadNewScene()
    {
        started = true;
        yield return new WaitForSeconds(0.5f);
        
        AsyncOperation async = SceneManager.LoadSceneAsync(GameManager.loadingScene);
        GameManager.loadingScene = -1;


        while (!async.isDone)
        {
            yield return null;
        }
        carga.fillAmount = 1;

        Destroy(gameObject);


    }

}
