using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// A stack for all items on this slot
    /// </summary>
    private ObservableStack<Item> items = new ObservableStack<Item>();

    public ObservableStack<Item> MyItems
    {
        get
        {
            return items;
        }
    }

    //A a reference to the slot's icon
    [SerializeField]
    private Image icon;

    /// <summary>
    /// Checks if the item is empty
    /// </summary>
    public bool IsEmpty
    {
        get
        {
            return MyItems.Count == 0;
        }
    }

    /// <summary>
    /// Indicates if the slot is full
    /// </summary>
    public bool IsFull
    {
        get
        {
            if (IsEmpty || MyCount < MyItem.MyStackSize)
            {
                return false;
            }

            return true;
        }
    }




    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return MyItems.Peek();
            }

            return null;
        }
    }

    public Image MyIcon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }

    public int MyCount
    {
        get { return MyItems.Count; }
    }

    public Text MyStackText
    {
        get
        {
            return stackSize;
        }
    }

    [SerializeField]
    private Text stackSize;


    private void Awake()
    {
        //Assigns all the event on our observable stack to the updateSlot function
        MyItems.OnPop += new UpdateStackEvent(UpdateSlot);
        MyItems.OnPush += new UpdateStackEvent(UpdateSlot);
        MyItems.OnClear += new UpdateStackEvent(UpdateSlot);
    }


    /// <summary>
    /// Adds an item to the slot
    /// </summary>
    /// <param name="item">the item to add</param>
    /// <returns>returns true if the item was added</returns>
    public bool AddItem(Item item)
    {
        MyItems.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        item.MySlot = this;
        return true;
    }

    /// <summary>
    /// Removes the item from the slot
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(Item item)
    {
        if (!IsEmpty)
        {
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
        }
    }

    public void Clear()
    {
        if (MyItems.Count > 0)
        {
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
            MyItems.Clear();
        }
    }

    /// <summary>
    /// Whem the slot is clicked
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.MyInstance.FromSlot == null && !IsEmpty) //If we don't have something to move
            {
                HandScript.MyInstance.TakeMoveable(MyItem as IMoveable);
                InventoryScript.MyInstance.FromSlot = this;
            }
            else if (InventoryScript.MyInstance.FromSlot != null)//If we have something to move
            {
                //We will try to do diffrent things to place the item back into the inventory
                if (PutItemBack() || MergeItems(InventoryScript.MyInstance.FromSlot) || SwapItems(InventoryScript.MyInstance.FromSlot) || AddItems(InventoryScript.MyInstance.FromSlot.MyItems))
                {
                    HandScript.MyInstance.Drop();
                    InventoryScript.MyInstance.FromSlot = null;
                }
            }

        }
        if (eventData.button == PointerEventData.InputButton.Right)//If we rightclick on the slot
        {
            UseItem();
        }
    }


    /// <summary>
    /// Uses the item if it is useable
    /// </summary>
    public void UseItem()
    {
        if (MyItem is IUseable)
        {
            (MyItem as IUseable).Use();
        }

    }

    /// <summary>
    /// Compara los nombre y ve la cantidad que se puede stakear
    /// </summary>
    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && MyItems.Count < MyItem.MyStackSize)
        {
            MyItems.Push(item);
            item.MySlot = this;
            return true;
        }

        return false;
    }


    /// <summary>
    /// Puts the item back in the inventory
    /// </summary>
    /// <returns></returns>
    private bool PutItemBack()
    {
        if (InventoryScript.MyInstance.FromSlot == this)
        {
            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
            return true;
        }

        return false;
    }


    /// <summary>
    /// Swaps two items in the inventory
    /// </summary>
    /// <param name="from"></param>
    /// <returns></returns>
    private bool SwapItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount + MyCount > MyItem.MyStackSize)
        {
            //Copy all the items we need to swap from A
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.MyItems);

            //Clear Slot a
            from.MyItems.Clear();
            //All items from slot b and copy them into A
            from.AddItems(MyItems);

            //Clear B
            MyItems.Clear();
            //Move the items from ACopy to B
            AddItems(tmpFrom);

            return true;
        }

        return false;
    }


    /// <summary>
    /// Adds a stack of items to the slot
    /// </summary>
    /// <param name="newItems">stack to add</param>
    /// <returns></returns>
    public bool AddItems(ObservableStack<Item> newItems)
    {
        if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
        {
            int count = newItems.Count;

            for (int i = 0; i < count; i++)
            {
                if (IsFull)
                {
                    return false;
                }

                AddItem(newItems.Pop());
            }

            return true;
        }

        return false;
    }


    /// <summary>
    /// Merges two identical stacks of items
    /// </summary>
    /// <param name="from">Slot to merge from</param>
    /// <returns></returns>
    private bool MergeItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        if (from.MyItem.GetType() == MyItem.GetType() && !IsFull)
        {
            //How many free slots do we have in the stack
            int free = MyItem.MyStackSize - MyCount;

            for (int i = 0; i < free; i++)
            {
                AddItem(from.MyItems.Pop());
            }

            return true;
        }

        return false;
    }
    private void UpdateSlot()
    {
        UIManager.MyInstance.UpdateStackSize(this);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        //We need to show tooltip
        if (!IsEmpty)
        {
            UIManager.MyInstance.ShowToolitip(transform.position, MyItem);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }
}
