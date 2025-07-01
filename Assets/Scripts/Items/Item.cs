using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Superclass for all items
/// </summary>
public abstract class Item : ScriptableObject, IMoveable, IDescribable
{
    /// <summary>
    /// Icon used when moving and placing the items
    /// </summary>
    [SerializeField]
    private Sprite icon;

    /// <summary>
    /// The size of the stack, less than 2 is not stackable
    /// </summary>
    [SerializeField]
    private int stackSize;

    /// <summary>
    /// A reference to the slot that this item is sitting on
    /// </summary>
    private SlotScript slot;

    /// <summary>
    /// Property for accessing the icon
    /// </summary>
    public Sprite MyIcon
    {
        get
        {
            return icon;
        }
    }

    /// <summary>
    /// Property for accessing the stacksize
    /// </summary>
    public int MyStackSize
    {
        get
        {
            return StackSize1;
        }
    }

    /// <summary>
    /// Proprty for accessing the slotscript
    /// </summary>
    public SlotScript MySlot
    {
        get
        {
            return slot;
        }

        set
        {
            slot = value;
        }
    }

    [SerializeField]
    private string title;


    /// <summary>
    /// Returns a description of this specific item
    /// </summary>
    /// <returns></returns>
    public string GetDescription()
    {
        return title;
    }

    public int StackSize1 { get => stackSize; set => stackSize = value; }

    /// <summary>
    /// Removes the item from the inventory
    /// </summary>
    public void Remove()
    {
        if (MySlot != null)
        {
            MySlot.RemoveItem(this);
        }
    }





 }

