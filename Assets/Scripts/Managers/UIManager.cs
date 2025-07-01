using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    private static UIManager instance;

    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    [SerializeField]
    private GameObject tooltip;

    /// <summary>
    /// Una referencia a todos los botones de acción.
    /// </summary>
    [SerializeField]
    private ActionButton[] actionButtons;

    private Text tooltipText;

    [SerializeField]
    private GameObject targetFrame;

    private Stat healthStat;
    private Stat nameStat;

    /// <summary>
    /// A reference to the keybind menu
    /// </summary>
    [SerializeField]
    private CanvasGroup keybindMenu;

    /// <summary>
    /// A reference to all the kibind buttons on the menu
    /// </summary>
    private GameObject[] keybindButtons;


    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
        tooltipText = tooltip.GetComponentInChildren<Text>();
    }


    // Use this for initialization
    void Start()
    {

   
        healthStat = targetFrame.GetComponent<Stat>();
        nameStat = targetFrame.GetComponent<Stat>();


    }

    // Update is called once per frame
    void Update()
    {
      
      

    }

    
    public void ShowTargetFrame(NPC target)
    {
        targetFrame.SetActive(true);
        healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);

        target.healthChanged += new HealthChanged(UpdateTargetFrame);
    }

    public void HideTargetFrame()
    {
        targetFrame.SetActive(false);
    }

    public void UpdateTargetFrame(float health)
    {
        healthStat.MyCurrentValue = health;
    }


    /// <summary>
    /// Updates the text on a keybindbutton after the key has been changed
    /// </summary>
    /// <param name="key"></param>
    /// <param name="code"></param>
    public void UpdateKeyText(string key, KeyCode code)
    {
        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();
    }

    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }


 

    public void OpenClose(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    /// <summary>
    /// Updates the stacksize on a clickable slot
    /// </summary>
    /// <param name="clickable"></param>
    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.MyCount > 1) //If our slot has more than one item on it
        {
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.color = Color.white;
            clickable.MyIcon.color = Color.white;
        }
        else //If it only has 1 item on it
        {
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
            clickable.MyIcon.color = Color.white;
        }
        if (clickable.MyCount == 0) //If the slot is empty, then we need to hide the icon
        {
            clickable.MyIcon.color = new Color(0, 0, 0, 0);
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }
    }



    /// <summary>
    /// Shows the tooltip
    /// </summary>
    public void ShowToolitip(Vector3 position, IDescribable description)
    {
        tooltip.SetActive(true);
        tooltip.transform.position = position;
        tooltipText.text = description.GetDescription();
    }

    /// <summary>
    /// Hides the tooltip
    /// </summary>
    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
