using UnityEngine;

public class HookCatch : MonoBehaviour
{
    public ScoreManager scoreManager;
    public AudioSource catchSound;
    public float currentPitch = 0.4f;

    private GameObject caughtFish = null;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fish") && caughtFish == null)
        {
            caughtFish = other.gameObject;
            caughtFish.transform.SetParent(transform);

            FishMovement movement = caughtFish.GetComponent<FishMovement>();
            if (movement != null)
            {
                movement.enabled = false;

            }

            if (catchSound != null)
            {
                catchSound.pitch = currentPitch;
                catchSound.Play();
            }
        }
    }

    void Update()
    {
        //checks if fish reached fisherman!!
        if (caughtFish != null && transform.position.y >= 2.7f)
        {
            scoreManager.AddScore(1);

            currentPitch -= 0.20f;
            currentPitch = Mathf.Clamp(currentPitch, 0.1f, 1.5f);

            Destroy(caughtFish);
            caughtFish = null;
        }
    }
}
