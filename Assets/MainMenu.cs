using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public AudioSource clickSound;
    public float delayBeforeLoad = 1f; // seconds

    public void PlayGame()
    {
        StartCoroutine(LoadSceneAfterSound());
    }

    IEnumerator LoadSceneAfterSound()
    {
        clickSound.Play();
        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene("FishOrDieScene");
    }
}