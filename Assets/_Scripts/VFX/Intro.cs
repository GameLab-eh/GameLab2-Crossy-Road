using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public float waitTime;
    private void Start()
    {
        StartCoroutine(WaitIntro());
    }

    IEnumerator WaitIntro()
    {
        yield return new WaitForSeconds(waitTime);
#if UNITY_EDITOR
        SceneManager.LoadScene("Game1");
#else
        SceneManager.LoadScene(1);
#endif
    }
}
