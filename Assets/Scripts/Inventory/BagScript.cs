using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{

    /// <summary>
    /// Prefab for creating slots
    /// </summary>
    [SerializeField]
    private GameObject slotPrefab;

    /// <summary>
    /// A list of all the slots the belongs to the bag
    /// </summary>
    private List<SlotScript> slots = new List<SlotScript>();

    public List<SlotScript> MySlots { get => slots; set => slots = value; }


    /// <summary>
    /// Creates slots for this bag
    /// </summary>
    /// <param name="slotCount">Amount of slots to create</param>
    public void AddSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
            MySlots.Add(slot);
        }
    }

    /// <summary>
    /// Adds an item to the bag
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddItem(Item item)
    {
        foreach (SlotScript slot in MySlots)//Checks all slots
        {
            if (slot.IsEmpty) //If the slot is empty then we add the item
            {
                slot.AddItem(item); //Adds the item

                return true; //Success
            }
        }
        //Failure
        return false;
    }



}
