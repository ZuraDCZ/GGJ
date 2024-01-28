using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] LayerMask foodLayer;
    [SerializeField] int id;
    private SpriteRenderer spriteRenderer;
    private Transform spawnTracker;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public int GetID()
    {
        return id;
    }

    public void UpdateFoodData(Sprite newSprite, int newID, Vector3 spawnRelocation)
    {
        if (newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
        }
        id = newID;
        transform.position = spawnRelocation;
        transform.localScale = new Vector3(0.6f,0.6f,0.6f); 
        gameObject.layer = LayerMask.NameToLayer("Food");
    }

    public void SetSpawnTracker(Transform spawnTransform)
    {
        spawnTracker = spawnTransform;
    }
    public Transform GetSpawn()
    {
        return spawnTracker;
    }
}
