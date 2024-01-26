using UnityEngine;


public class Table : MonoBehaviour
{
    private bool occupied; // Tracks if the table is occupied
    [SerializeField] private Transform sit;

    public bool isOccupied() //Checks if the table is occupied
    {
        return occupied;
    }

    public void Occupy() //Occupies table
    {
        occupied = true;
    }

    public void Unoccupy()
    {
        occupied = false;
    }

    public Transform GetSit()
    {
        return sit;
    }
}
