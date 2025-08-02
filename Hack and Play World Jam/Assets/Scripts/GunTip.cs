using UnityEngine;

public class GunTip : MonoBehaviour
{
    PlayerBehaviour playerBehaviour;
    void Awake()
    {
        playerBehaviour = GetComponentInParent<PlayerBehaviour>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
            playerBehaviour.SetGunTipTouchingGround(true);
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
            playerBehaviour.SetGunTipTouchingGround(true);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
            playerBehaviour.SetGunTipTouchingGround(false);
    }
}
