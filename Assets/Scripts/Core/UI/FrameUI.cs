using UnityEngine;
using UnityEngine.UI;

public class FrameUI : MonoBehaviour
{
    [SerializeField] private Image damageImage;
    [SerializeField] private Image healImage;
    [SerializeField] private float fadeSpeed = 2f;

    private float bloodAlpha = 0f;
    private float healAlpha = 0f;

    void Update()
    {
        if (bloodAlpha > 0f)
        {
            bloodAlpha = Mathf.Lerp(bloodAlpha, 0f, fadeSpeed * Time.deltaTime);
            UpdateBloodAlpha();
        }

        if (healAlpha > 0f)
        {
            healAlpha = Mathf.Lerp(healAlpha, 0f, fadeSpeed * Time.deltaTime);
            UpdateHealAlpha();
        }
    }

    public void ShowBlood()
    {
        bloodAlpha = 1f;
        UpdateBloodAlpha();
    }

    public void ShowHeal()
    {
        healAlpha = 1f;
        UpdateHealAlpha();
    }

    private void UpdateBloodAlpha()
    {
        Color color = damageImage.color;
        color.a = bloodAlpha;
        damageImage.color = color;
    }

    private void UpdateHealAlpha()
    {
        Color color = healImage.color;
        color.a = healAlpha;
        healImage.color = color;
    }
}
