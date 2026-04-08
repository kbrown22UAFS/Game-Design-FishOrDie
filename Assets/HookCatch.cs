using UnityEngine;

public class HookCatch : MonoBehaviour
{
    public ScoreManager scoreManager;

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
        }
    }

    void Update()
    {
        //checks if fish reached fisherman!!
        if (caughtFish != null && transform.position.y >= 2.7f)
        {
            scoreManager.AddScore(1);
            Destroy(caughtFish);
            caughtFish = null;
        }
    }
}
