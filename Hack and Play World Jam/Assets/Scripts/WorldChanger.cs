using UnityEngine;

public class WorldChanger : MonoBehaviour
{
    /*
    void HandleTrigger(Collider2D other)
    {
        //Debug.Log(other.name);
        WorldScript worldScript = other.GetComponent<WorldScript>();
        if (!worldScript)
            worldScript = other.GetComponentInParent<WorldScript>();
        
        if (!worldScript) return;

        Debug.Log(worldScript.name);
        worldScript.StoreTriggerDirection(this.transform);
    }
    */
    void OnTriggerEnter2D(Collider2D other)
    {
        WorldScript worldScript = other.GetComponent<WorldScript>();
        if (!worldScript)
            worldScript = other.GetComponentInParent<WorldScript>();

        if (!worldScript) return;

        Debug.Log(worldScript.name);
        worldScript.SwapWorlds();
    }
    /*
    void OnTriggerExit2D(Collider2D other)
    {
        HandleTrigger(other);
    }
    */
}
