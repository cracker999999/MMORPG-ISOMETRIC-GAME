using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeybindManager : MonoBehaviour
{
    /// <summary>
    /// A reference to the singleton instance
    /// </summary>
    private static KeybindManager instance;

    /// <summary>
    /// Property for accessing the singleton instance
    /// </summary>
    public static KeybindManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<KeybindManager>();
            }

            return instance;
        }
    }

    /// <summary>
    /// A dictionary for all actionKeybinds
    /// </summary>
    public Dictionary<string, KeyCode> ActionBinds { get; private set; }

    /// <summary>
    /// The name of the keybind we are trying to set or change
    /// </summary>
    private string bindName;

    // Use this for initialization
    void Start()
    {

        ActionBinds = new Dictionary<string, KeyCode>();

        BindKey("ACT1", KeyCode.Alpha1);
        BindKey("ACT2", KeyCode.Alpha2);
        BindKey("ACT3", KeyCode.Alpha3);
    }

    /// <summary>
    /// Binds a specific key
    /// </summary>
    /// <param name="key">Key to bind</param>
    /// <param name="keyBind">Keybind to set</param>
    public void BindKey(string key, KeyCode keyBind)
    {
        Dictionary<string, KeyCode> currentDictionary = null;

        if (key.Contains("ACT"))
        {
            currentDictionary = ActionBinds;
        }
        if (!currentDictionary.ContainsKey(key))
        {
            currentDictionary.Add(key, keyBind);
            UIManager.MyInstance.UpdateKeyText(key, keyBind);
        }
        else if (currentDictionary.ContainsValue(keyBind))
        {
            string myKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key;

            currentDictionary[myKey] = KeyCode.None;
            UIManager.MyInstance.UpdateKeyText(key, KeyCode.None);
        }

        currentDictionary[key] = keyBind;
        UIManager.MyInstance.UpdateKeyText(key, keyBind);
        bindName = string.Empty;
    }

    /// <summary>
    /// Function for setting a keybind, this is called whenever a keybind button is clicked on the keybind menu
    /// </summary>
    /// <param name="bindName"></param>
    public void KeyBindOnClick(string bindName)
    {
        this.bindName = bindName;
    }


    private void OnGUI()
    {
        if (bindName != string.Empty)//Checks if we are going to save a keybind
        {
            Event e = Event.current; //Listens for an event

            if (e.isKey) //If the event is a key, then we change the keybind
            {
                BindKey(bindName, e.keyCode);
            }
        }
    }

}
