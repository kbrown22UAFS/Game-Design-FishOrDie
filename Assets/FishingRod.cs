using UnityEngine;
using UnityEngine.InputSystem;

public class FishingRod : MonoBehaviour
{
    public Transform hook;
    public float speed = 2f;

    public float bottomLimit = -3f; //lowest point the hook can reach

    private bool isCasting = false;
    private bool isReeling = false;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            //If hook is at player, then cast
            if (!isCasting && !isReeling && hook.position.y >= transform.position.y - 0.1f)
            {
                isCasting = true;
            }
            else
            {
                //reel in
                isCasting = false;
                isReeling = true;
            }
        }

        //Casting downward
        if (isCasting)
        {
            hook.position += Vector3.down * speed * Time.deltaTime;

            if (hook.position.y <= bottomLimit)
            {
                hook.position = new Vector3(hook.position.x, bottomLimit, hook.position.z);
                isCasting = false;
            }
        }

        //Reeling upward
        if (isReeling)
        {
            hook.position += Vector3.up * speed * Time.deltaTime;

            if (hook.position.y >= transform.position.y)
            {
                hook.position = new Vector3(hook.position.x, transform.position.y, hook.position.z);
                isReeling = false;
            }
        }
    }
}