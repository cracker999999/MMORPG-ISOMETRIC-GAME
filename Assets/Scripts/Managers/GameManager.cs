using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Una referencia al objeto jugador
    /// </summary>
    [SerializeField]
    private Player player;

    private NPC currentTarget;

    private Camera mainCamera;

    private static GameManager instance;

    private HashSet<Vector3Int> blocked = new HashSet<Vector3Int>();

    [SerializeField]
    private LayerMask clickableLayer, groundLayer;


    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }

    }

    public HashSet<Vector3Int> Blocked
    {
        get
        {
            return blocked;
        }

        set
        {
            blocked = value;
        }
    }


    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //Ejecuta objetivo del clic
        ClickTarget();
        DisplayHealth();
    }


    private void DisplayHealth()
    {
        if (!EventSystem.current.IsPointerOverGameObject())//
        {

            //Makes a raycast from the mouse position into the game world
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Clickable"));
            if (hit.collider != null)//If we hit something
            {
                if (hit.collider.tag == "Enemy") {
                    currentTarget = hit.collider.GetComponent<NPC>(); //Selects the new target

                    currentTarget.Select(); //Gives the player the new target


                }
            } else
            {
                if (currentTarget != null) //If we have a current target
                {
                    currentTarget.DeSelect();
                }
            }
        
         }
        
    }


    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) //Si hacemos clic con el botón izquierdo del mouse
        {
            //Hace una emisión de rayos desde la posición del mouse al mundo del juego
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Clickable"));

            if (hit.collider != null)//Si golpeamos algo
            {
                if (hit.collider.tag == "Enemy")
                {
                    currentTarget = hit.collider.GetComponent<NPC>(); //Selects the new target
                    //Si golpeamos a un enemigo, lo establecemos como nuestro objetivo.
                    player.MyTarget = currentTarget.Select();

                    UIManager.MyInstance.ShowTargetFrame(currentTarget);
                }
            }
            else
            {

                hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, groundLayer);

                if (hit.collider != null)
                {
                    //player.GetPath(mainCamera.ScreenToWorldPoint(Input.mousePosition));leen-
                }

                UIManager.MyInstance.HideTargetFrame();
                //Destraba el objetivo
                currentTarget = null;
                player.MyTarget = null;

            }


        }

    }



}
