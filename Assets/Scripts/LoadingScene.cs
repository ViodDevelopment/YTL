using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public bool started = false;

    private void Update()
    {
        if(started)
            StartCoroutine(LoadNewScene());
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

        Destroy(gameObject);


    }

}
