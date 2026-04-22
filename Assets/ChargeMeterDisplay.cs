using UnityEngine;

public class ChargeMeterDisplay : MonoBehaviour
{
    public FishingRod fishingRod;
    public SpriteRenderer fill;

    private float spriteHalfWidth;
    private SpriteRenderer background;

    void Start()
    {
        spriteHalfWidth = fill.sprite.bounds.extents.x;
        background = GetComponent<SpriteRenderer>();
        background.enabled = false;
        fill.enabled = false;
    }

    void Update()
    {
        bool charging = fishingRod.IsCharging;
        background.enabled = charging;
        fill.enabled = charging;

        if (!charging) return;

        float t = fishingRod.ChargeFraction;

        fill.color = Color.Lerp(Color.red, Color.green, t);

        Vector3 ls = fill.transform.localScale;
        ls.x = Mathf.Max(t, 0.001f);
        fill.transform.localScale = ls;

        Vector3 lp = fill.transform.localPosition;
        lp.x = spriteHalfWidth * (1f - t);
        fill.transform.localPosition = lp;
    }
}
