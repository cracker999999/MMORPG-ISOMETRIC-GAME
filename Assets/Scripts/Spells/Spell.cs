using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class Spell : IUseable, IMoveable, IDescribable
{


    /// <summary>
    /// The Spell's name
    /// </summary>
    [SerializeField]
    private string name;

    /// <summary>
    /// The spell's icon
    /// </summary>
    [SerializeField]
    private Sprite icon;


    /// <summary>
    /// The spell's damage
    /// </summary>
    [SerializeField]
    private int damage;

 
    /// <summary>
    /// The spell's prefab
    /// </summary>
    [SerializeField]
    private GameObject spellPrefab;


    [SerializeField]
    private string description;

    /// <summary>
    /// Property for reading the damage
    /// </summary>
    public int MyDamage
    {
        get
        {
            return damage;
        }

    }

    /// <summary>
    /// Property for reading the icon
    /// </summary>
    public Sprite MyIcon
    {
        get
        {
            return icon;
        }
    }



    /// <summary>
    /// Property for accessing the spell's name
    /// </summary>
    public string MyName
    {
        get
        {
            return name;
        }
    }

    /// <summary>
    /// Property for reading the spell's prefab
    /// </summary>
    public GameObject MySpellPrefab
    {
        get
        {
            return spellPrefab;
        }
    }

    public void Use()
    {
        Player.MyInstance.CastSpell(MyName);
    }

    public string GetDescription()
    {
        return string.Format("Titulo: {0} \nDescripcion: {1}\n<color=#ffd111> \nQue causa {2} daño</color>", name, description, damage);
    }


}
