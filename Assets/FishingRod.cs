using UnityEngine;
using UnityEngine.InputSystem;

public class FishingRod : MonoBehaviour
{
    public Transform hook;
    public Transform rodTip;

    [Header("Cast")]
    public float castChargeTime = 2f;       // seconds of hold to reach max power
    public float castMaxSpeed = 8f;         // hook speed at full charge
    public Vector2 castAngle = new Vector2(1f, 0.5f); // launch direction (normalized in Awake)

    [Header("Arc & Water")]
    public float gravity = -18f;            // downward pull while hook is airborne
    public float waterSurfaceY = 0.3f;      // Y level where hook enters water
    public float bottomLimit = -3f;

    [Header("Underwater")]
    public float sinkSpeed = 0.5f;          // drift speed when hook is idle in water
    public float reelingSpeed = 3f;         // speed while X is held

    // ── state machine ──────────────────────────────────────────────────────────
    private enum State { AtRod, Charging, InAir, InWater, Reeling }
    private State state = State.AtRod;

    private float chargeTimer;
    private Vector2 hookVelocity;
    private Vector3 rodOrigin;              // position the hook was cast from
    private Rigidbody2D hookRb;

    void Awake()
    {
        castAngle = castAngle.normalized;
        hookRb = hook.GetComponent<Rigidbody2D>();
        if (hookRb != null) hookRb.isKinematic = true;
    }

    void Update()
    {
        switch (state)
        {
            case State.AtRod:
            case State.Charging:
                UpdateCharging();
                break;
            case State.InAir:
                UpdateInAir();
                break;
            case State.InWater:
                UpdateInWater();
                break;
            case State.Reeling:
                UpdateReeling();
                break;
        }
    }

    // ── charging ───────────────────────────────────────────────────────────────

    void UpdateCharging()
    {
        if (Keyboard.current.spaceKey.isPressed && state != State.Charging)
            state = State.Charging;

        if (state == State.Charging && Keyboard.current.spaceKey.isPressed)
            chargeTimer = Mathf.Min(chargeTimer + Time.deltaTime, castChargeTime);

        if (state == State.Charging && Keyboard.current.spaceKey.wasReleasedThisFrame)
            Cast();
    }

    void Cast()
    {
        float power = chargeTimer / castChargeTime;
        chargeTimer = 0f;

        rodOrigin = rodTip != null ? rodTip.position : transform.position;
        MoveHook(rodOrigin);

        hookVelocity = castAngle * (power * castMaxSpeed);
        state = State.InAir;
    }

    // ── airborne arc ───────────────────────────────────────────────────────────

    void UpdateInAir()
    {
        hookVelocity.y += gravity * Time.deltaTime;
        Vector3 next = hook.position + new Vector3(hookVelocity.x, hookVelocity.y, 0f) * Time.deltaTime;
        MoveHook(next);

        if (hook.position.y <= waterSurfaceY)
        {
            MoveHook(new Vector3(hook.position.x, waterSurfaceY, hook.position.z));
            hookVelocity = Vector2.zero;
            state = State.InWater;
        }
        else if (hook.position.y < bottomLimit)
        {
            MoveHook(new Vector3(hook.position.x, bottomLimit, hook.position.z));
            hookVelocity = Vector2.zero;
            state = State.InWater;
        }
    }

    // ── sinking in water ───────────────────────────────────────────────────────

    void UpdateInWater()
    {
        if (hook.position.y > bottomLimit)
        {
            float nextY = Mathf.Max(hook.position.y - sinkSpeed * Time.deltaTime, bottomLimit);
            MoveHook(new Vector3(hook.position.x, nextY, hook.position.z));
        }

        if (Keyboard.current.xKey.isPressed)
            state = State.Reeling;
    }

    // ── reeling (hold X) ───────────────────────────────────────────────────────

    void UpdateReeling()
    {
        Vector3 toRod = rodOrigin - hook.position;

        if (toRod.magnitude <= 0.05f)
        {
            MoveHook(rodOrigin);
            state = State.AtRod;
            return;
        }

        MoveHook(hook.position + toRod.normalized * reelingSpeed * Time.deltaTime);

        if (!Keyboard.current.xKey.isPressed)
        {
            // Stop reeling — sink again if still underwater, otherwise snap home
            state = hook.position.y <= waterSurfaceY ? State.InWater : State.AtRod;
        }
    }

    // ── helper ─────────────────────────────────────────────────────────────────

    void MoveHook(Vector3 pos)
    {
        hook.position = pos;
        if (hookRb != null) hookRb.position = pos;
    }
}
