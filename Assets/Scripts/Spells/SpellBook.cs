using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour
{

    private static SpellBook instance;

    public static SpellBook MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpellBook>();
            }

            return instance;
        }
    }

    /// <summary>
    /// All spells in the spellbook
    /// </summary>
    [SerializeField]
    private Spell[] spells;

       
    /// <summary>
    /// Cast a spell at an enemy
    /// </summary>
    /// <param name="index">The index of the spell you would like to cast, the first spells is index 0</param>
    /// <returns></returns>
    public Spell CastSpell(string spellName)
    {
        Spell spell = Array.Find(spells, x => x.MyName == spellName);


        //Returns the spell that we just  cast.
        return spell;
    }

    public Spell GetSpell(string spellName)
    {
        Spell spell = Array.Find(spells, x => x.MyName == spellName);

        return spell;
    }

}
