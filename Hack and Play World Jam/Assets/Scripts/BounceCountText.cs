using UnityEngine;
using TMPro;

public class BounceCountText : MonoBehaviour
{
    TMP_Text bounceText;
    PlayerBehaviour playerBehaviour;

    void Awake()
    {
        bounceText = GetComponent<TMP_Text>();
        playerBehaviour = FindFirstObjectByType<PlayerBehaviour>();
        bounceText.text = $"{playerBehaviour.maxBulletBounces}";
    }

    public void UpdateBounceText(int durability)
    {
        bounceText.text = $"{durability}";
    }
}
