using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public AudioSource clickSound;
    public float delayBeforeLoad = 1f; //seconds

    public void PlayGame()
    {
        StartCoroutine(LoadSceneAfterSound("FishOrDieScene1"));
    }

    public void LoadTitleScreen()
    {
        StartCoroutine(LoadSceneAfterSound("TitleScene"));
    }

    IEnumerator LoadSceneAfterSound(string sceneName)
    {
        clickSound.Play();
        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene(sceneName);
    }
}