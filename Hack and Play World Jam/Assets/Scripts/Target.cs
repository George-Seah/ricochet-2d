using UnityEngine;

public class Target : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (other.GetComponent<Bullet>()) Destroy(this.gameObject);
    }

}
