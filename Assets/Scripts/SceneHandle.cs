using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandle : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(HideLoading());
    }

    IEnumerator HideLoading()
    {
        yield return new WaitForSeconds(3f);
        //SceneManager.UnloadSceneAsync("Loading");
    }
}
