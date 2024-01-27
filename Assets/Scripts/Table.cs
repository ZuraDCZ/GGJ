using UnityEngine;


public class Table : MonoBehaviour
{
    private bool occupied; // Tracks if the table is occupied
    [SerializeField] Transform sit; // Tracks the place where the client sits
    [SerializeField] Transform platePlace; //Tracks the place where the plaet is placed on delivery

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

    public Transform GetPlatePlace()
    {
        return platePlace;
    }
}
