using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ItemCountChanged(Item item);


public class InventoryScript : MonoBehaviour
{

    public event ItemCountChanged itemCountChangedEvent;

    private static InventoryScript instance;

    public static InventoryScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }

            return instance;
        }
    }

    private SlotScript fromSlot;

    public SlotScript FromSlot
    {
        get
        {
            return fromSlot;
        }

        set
        {
            fromSlot = value;

            if (value != null)
            {
                fromSlot.MyIcon.color = Color.grey;
            }
        }
    }


    //For debugging
    [SerializeField]
    private Item[] items;

    private void Awake()
    {
        
        Bag bag = (Bag)Instantiate(items[0]);
        bag.Initialize(44);
        bag.Use();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            HealthPotion potion = (HealthPotion)Instantiate(items[1]);
            AddItem(potion);
        }

    }

    /// <summary>
    /// Adds an item to the inventory
    /// </summary>
    /// <param name="item">Item to add</param>
    public void AddItem(Item item)
    {
        if (item.MyStackSize > 0)
        {
            if (PlaceInStack(item))
            {
                return;
            }
        }

        PlaceInEmpty(item);

    }

    /// <summary>
    /// Places an item on an empty slot in the game
    /// </summary>
    /// <param name="item">Item we are trying to add</param>
    private void PlaceInEmpty(Item item)
    {
      
            if (Bag.MyBagScript.AddItem(item)) //Tries to add the item
            {
                OnItemCountChanged(item);
                return; //It was possible to add the item
            }
        
    }


    /// <summary>
    /// Tries to stack an item on anothe
    /// </summary>
    /// <param name="item">Item we try to stack</param>
    /// <returns></returns>
    private bool PlaceInStack(Item item)
    {
      
            foreach (SlotScript slots in Bag.MyBagScript.MySlots) //Checks all the slots on the current bag
            {
                if (slots.StackItem(item)) //Tries to stack the item
                {
                    OnItemCountChanged(item);
                    return true; //It was possible to stack the item
                }
            }
        

        return false; //It wasn't possible to stack the item
    }


    public Stack<IUseable> GetUseables(IUseable type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();

      
            foreach (SlotScript slot in Bag.MyBagScript.MySlots)
            {
                if (!slot.IsEmpty && slot.MyItem.GetType() == type.GetType())
                {
                    foreach (Item item in slot.MyItems)
                    {
                        useables.Push(item as IUseable);
                    }
                }
            
        }

        return useables;
    }


    public void OnItemCountChanged(Item item)
    {
        if (itemCountChangedEvent != null)
        {
            itemCountChangedEvent.Invoke(item);
        }
    }

}
