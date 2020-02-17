using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    private bool started = false;
    public bool doing = false;
    public GirarImagenEternamente myimage;
    private int numScene = 0;
    private void Update()
    {
        if (myimage != null)
        {
            if (started)
                StartCoroutine(LoadNewScene());
        }
    }

    IEnumerator LoadNewScene()
    {
        started = false;
        yield return new WaitForSeconds(0.75f);
        myimage.parar = true;
        float l_time = 360 / myimage.speed;
        print(l_time);
        yield return new WaitForSeconds(l_time);
        AsyncOperation async = SceneManager.LoadSceneAsync(numScene);

        while (!async.isDone)
        {
            yield return null;
        }

    }

    public void ActiveLoading()
    {
        numScene = GameManager.loadingScene;
        GameManager.loadingScene = -1;
        started = true;
        doing = true;
    }

}
