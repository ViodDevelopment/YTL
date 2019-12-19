using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    private bool started = false;

    private void Update()
    {
        if(GameManager.loadingScene != -1 && !started)
            StartCoroutine(LoadNewScene());
    }

    IEnumerator LoadNewScene()
    {
        started = true;
        yield return new WaitForSeconds(0.5f);
        
        AsyncOperation async = SceneManager.LoadSceneAsync(GameManager.loadingScene);

        while (!async.isDone)
        {

            yield return null;

        }
        GameManager.loadingScene = -1;
        Destroy(gameObject);


    }

}
