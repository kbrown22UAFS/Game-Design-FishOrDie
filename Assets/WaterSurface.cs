using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WaterSurface : MonoBehaviour
{
    [Header("Splash")]
    public int splashCount = 18;
    public float splashSpeed = 5f;
    public float splashLifetime = 0.5f;
    public Color splashColor = new Color(0.5f, 0.85f, 1f, 0.9f);

    [Header("Ripple")]
    public float rippleDuration = 0.9f;
    public float rippleMaxWidth = 2.0f;
    public Color rippleColor = new Color(1f, 1f, 1f, 0.55f);

    [Header("Cooldown")]
    public float splashCooldown = 0.15f;

    private ParticleSystem splashPS;
    private Sprite rippleSprite;
    private SpriteRenderer waterRenderer;
    private float lastSplashTime = -99f;

    void Awake()
    {
        waterRenderer = GetComponent<SpriteRenderer>();
        rippleSprite = BuildRingSprite(64, 0.7f);
        InitTrigger();
        InitParticles();
    }

    void InitTrigger()
    {
        var col = GetComponent<BoxCollider2D>();
        if (col == null) col = gameObject.AddComponent<BoxCollider2D>();

        Bounds spriteBounds = waterRenderer.sprite.bounds;
        col.size = new Vector2(spriteBounds.size.x, 0.25f);
        col.offset = new Vector2(0f, spriteBounds.extents.y - 0.1f);
        col.isTrigger = true;
    }

    void InitParticles()
    {
        GameObject go = new GameObject("SplashPS");
        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;

        splashPS = go.AddComponent<ParticleSystem>();

        var main = splashPS.main;
        main.loop = false;
        main.playOnAwake = false;
        main.startLifetime = new ParticleSystem.MinMaxCurve(splashLifetime * 0.6f, splashLifetime);
        main.startSpeed = new ParticleSystem.MinMaxCurve(splashSpeed * 0.4f, splashSpeed);
        main.startSize = new ParticleSystem.MinMaxCurve(0.04f, 0.11f);
        main.startColor = splashColor;
        main.gravityModifier = 1.5f;
        main.maxParticles = 200;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        var emission = splashPS.emission;
        emission.enabled = true;
        emission.rateOverTime = 0;

        var shape = splashPS.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 28f;
        shape.radius = 0.08f;

        var colorOverLife = splashPS.colorOverLifetime;
        colorOverLife.enabled = true;
        var grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
        );
        colorOverLife.color = grad;

        var psRenderer = splashPS.GetComponent<ParticleSystemRenderer>();
        psRenderer.renderMode = ParticleSystemRenderMode.Billboard;
        psRenderer.sortingLayerName = waterRenderer.sortingLayerName;
        psRenderer.sortingOrder = waterRenderer.sortingOrder + 2;
        psRenderer.material = new Material(Shader.Find("Sprites/Default"));
        psRenderer.material.color = splashColor;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        TriggerSplash(other.transform.position.x);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        TriggerSplash(other.transform.position.x);
    }

    void TriggerSplash(float x)
    {
        if (Time.time - lastSplashTime < splashCooldown) return;
        lastSplashTime = Time.time;

        float surfaceY = waterRenderer.bounds.max.y;
        Vector3 splashPos = new Vector3(x, surfaceY, 0f);

        splashPS.transform.position = splashPos;
        splashPS.Emit(splashCount);

        StartCoroutine(RippleEffect(splashPos));
    }

    IEnumerator RippleEffect(Vector3 pos)
    {
        GameObject ripple = new GameObject("Ripple");
        ripple.transform.position = pos;

        var sr = ripple.AddComponent<SpriteRenderer>();
        sr.sprite = rippleSprite;
        sr.color = rippleColor;
        sr.sortingLayerName = waterRenderer.sortingLayerName;
        sr.sortingOrder = waterRenderer.sortingOrder + 1;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / rippleDuration;
            float w = Mathf.Lerp(0.05f, rippleMaxWidth, Mathf.SmoothStep(0f, 1f, t));
            ripple.transform.localScale = new Vector3(w, w * 0.22f, 1f);
            sr.color = new Color(rippleColor.r, rippleColor.g, rippleColor.b, rippleColor.a * (1f - t));
            yield return null;
        }

        Destroy(ripple);
    }

    Sprite BuildRingSprite(int size, float innerRatio)
    {
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color[] pixels = new Color[size * size];
        Vector2 center = new Vector2(size * 0.5f, size * 0.5f);
        float outer = size * 0.5f;
        float inner = outer * innerRatio;

        for (int i = 0; i < size; i++)
        for (int j = 0; j < size; j++)
        {
            float d = Vector2.Distance(new Vector2(j, i), center);
            pixels[i * size + j] = (d <= outer && d >= inner) ? Color.white : Color.clear;
        }

        tex.SetPixels(pixels);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, size, size), Vector2.one * 0.5f, size);
    }
}
