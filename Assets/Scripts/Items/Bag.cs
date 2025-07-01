using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Bag",menuName ="Items/Bag",order =1)]
public class Bag : Item, IUseable
{
    /// <summary>
    /// The amount of slots this bag has
    /// </summary>
    private int slots;

    /// <summary>
    /// A reference to a bag prefab, so that we can instanitate a bag in the game
    /// </summary>
    [SerializeField]
    private GameObject bagPrefab;

    /// <summary>
    /// A reference to the bagScript, that this bag belongs to
    /// </summary>
    public static BagScript MyBagScript { get; set; }

    /// <summary>
    /// Property for getting the slots
    /// </summary>
    public int Slots
    {
        get
        {
            return slots;
        }
    }

    /// <summary>
    /// Initializes the bag with an amount of slots
    /// </summary>
    /// <param name="slots"></param>
    public void Initialize(int slots)
    {
        this.slots = slots;
    }

    /// <summary>
    /// Equipts the bag
    /// </summary>
    public void Use()
    {
        MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<BagScript>();
        MyBagScript.AddSlots(slots);
    }
}
