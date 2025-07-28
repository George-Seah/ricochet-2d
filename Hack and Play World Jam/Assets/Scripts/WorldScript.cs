using UnityEngine;
using System.Linq;

public class WorldScript : MonoBehaviour
{
    public bool world1;
    public SpriteRenderer spriteRenderer;
    Vector2 triggerDirectionVector;
    string enterDirection;
    string exitDirection;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetWorldLayer();
    }

    public void SetWorldLayer()
    {
        if (world1)
        {
            gameObject.layer = LayerMask.NameToLayer("World 1");
            if (spriteRenderer) spriteRenderer.color = Color.red;
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("World 2");
            if (spriteRenderer) spriteRenderer.color = Color.green;
        }
        /*
        SpriteRenderer[] childSprites = GetComponentsInChildren<SpriteRenderer>();
        Transform[] childObjects = GetComponentsInChildren<Transform>();
        */

        GetComponentsInChildren<SpriteRenderer>()
            .ToList()
            .ForEach(sr => sr.color = this.spriteRenderer.color);

        GetComponentsInChildren<WorldScript>()
            .ToList()
            .ForEach(ws =>
            {
                ws.world1 = this.world1;
                ws.gameObject.layer = this.gameObject.layer;
            });
        
    }
    public void SwapWorlds()
    {
        world1 = !world1;
        SetWorldLayer();
    }
    public void StoreTriggerDirection(Transform worldChanger)
    {
        
        triggerDirectionVector = (Vector2)(worldChanger.position - transform.position).normalized;
        if (string.IsNullOrEmpty(enterDirection))
        {
            if (Mathf.Abs(triggerDirectionVector.x) > Mathf.Abs(triggerDirectionVector.y))
                enterDirection = (triggerDirectionVector.x > 0) ? "left" : "right";
            else
                enterDirection = (triggerDirectionVector.x > 0) ? "up" : "down";
        }
        else
        {
            if (Mathf.Abs(triggerDirectionVector.x) > Mathf.Abs(triggerDirectionVector.y))
                exitDirection = (triggerDirectionVector.x > 0) ? "left" : "right";
            else
                exitDirection = (triggerDirectionVector.x > 0) ? "up" : "down";

            if (enterDirection != exitDirection)
            {
                SwapWorlds();
            }
            enterDirection = null;
            exitDirection = null;
        }
    }

}
