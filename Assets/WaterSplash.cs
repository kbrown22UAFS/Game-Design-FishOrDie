using UnityEngine;

public class WaterSplash : MonoBehaviour
{
    public AudioSource splashSound;
    private bool hasSplashed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hook") && !hasSplashed)
        { 
            splashSound.Play();
            hasSplashed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Hook"))
        {
            hasSplashed = false;
        }
    }
}
