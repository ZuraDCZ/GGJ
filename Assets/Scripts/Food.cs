using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] LayerMask foodLayer;
    [SerializeField] int id;
    private Sprite sprite;
    private Transform spawnTracker;

    private void Awake()
    {
        sprite = GetComponent<Sprite>();
    }

    public int GetID()
    {
        return id;
    }

    public void UpdateFoodData(Sprite newSprite, int newID, Vector3 spawnRelocation)
    {
        if (newSprite != null)
        {
            sprite = newSprite;
        }
        id = newID;
        transform.position = spawnRelocation;
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
