using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private int id;

    public int GetID()
    {
        return id;
    }
}
