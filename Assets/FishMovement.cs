using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float speed = 1f;
    public float distance = 2f;
    public AudioSource swimSound;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;

        if (swimSound != null)
        { 
            swimSound.Play();
        }
    }

    void Update()
    {
        float movement = Mathf.Sin(Time.time * speed) * distance;
        transform.position = new Vector3(startPos.x + movement, startPos.y, startPos.z);
    }
}
