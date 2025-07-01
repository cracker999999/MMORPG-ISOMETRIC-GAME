using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{


    private static Player instance;

    public static Player MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }

            return instance;
        }
    }

    /// <summary>
    /// Puntos de accion del jugador.
    /// </summary>
    [SerializeField]
    protected Stat pa;

    /// <summary>
    /// Puntos de accion iniciales del caracter
    /// </summary>
    [SerializeField]
    private float initPa;


    #region PATHFINDING

    private Stack<Vector3> path;

    private Vector3 destination;

    private Vector3 goal;

    [SerializeField]
    private AStar astar;

    #endregion


    protected override  void Start()    {
        //Inicializamos los PA
        pa.Initialize(initPa, initPa);

        base.Start();

    }

    // Update is called once per frame
    protected override void Update(){

        //Vemos que pulsamos y setea la direction
        GetInpu();
        ClickToMove();

        base.Update();

    }
    
    /// <summary>
    /// Escucha la entrada de teclas
    /// </summary>
    private void GetInpu(){


        //TESTEO SOLO 

        if (Input.GetKeyDown(KeyCode.V))
        {
            health.MyCurrentValue -= 2;
            pa.MyCurrentValue -= 1;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            health.MyCurrentValue += 2;
            pa.MyCurrentValue += 1;
        }



        //

        direction = Vector2.zero;

        if (Input.GetKey(KeyCode.W)){
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A)){
            direction += Vector2.left;
        }

        if (Input.GetKey(KeyCode.S)){
            direction += Vector2.down;
        }

        if (Input.GetKey(KeyCode.D)){
            direction += Vector2.right;
        }


        foreach (string action in KeybindManager.MyInstance.ActionBinds.Keys)
        {
            if (Input.GetKeyDown(KeybindManager.MyInstance.ActionBinds[action]))
            {
                UIManager.MyInstance.ClickActionButton(action);

            }
        }

    }

    /// <summary>
    /// Metodo para atacar
    /// </summary>
    /// <returns></returns>
    private IEnumerator Attack(string spellName)
    {
            Transform currentTarget = MyTarget;

            //Creates a new spell, so that we can use the information form it to cast it in the game
            Spell newSpell = SpellBook.MyInstance.CastSpell(spellName);

            IsAttacking = true; //Indica que estamos atacando

            pa.MyCurrentValue -= 1; //Bajamos 1 pa (Solo por probar)

            MyAnimator.SetBool("attack", IsAttacking); // Inicia la animacion del personaje
            yield return new WaitForSeconds(1.5f);  //Tiempo de lanzamiento, solo debugear
            
            //Comprueba si no es nulo para no cambiar de objetivo mientras tiramos el spell
            if(currentTarget != null){ 
                //Crea un objeti hechizo 
 
                SpellScript s = Instantiate(newSpell.MySpellPrefab, transform.position, Quaternion.identity).GetComponent<SpellScript>();
                s.Initialize(currentTarget, newSpell.MyDamage, transform); // asigna el objetivo donde tirar el hechizo

        }

            StopAttack(); //Termina el ataque

    }


  
    /// <summary>
    /// Lanza un hechizo
    /// </summary>
    public void CastSpell(string spellName)
    { 
        if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !IsAttacking && !IsMoving) //Comprueba si podemos atacar (si tenemos objetivo, no atacamos ya o no nos movemos)
        {
            attackRoutine = StartCoroutine(Attack(spellName));
        }

    }


    /// <summary>
    /// Detiene el ataque
    /// </summary>
    public void StopAttack()
    {
        
        StopCoroutine(attackRoutine);
        IsAttacking = false;
        MyAnimator.SetBool("attack", IsAttacking);
    }


    public void GetPath(Vector3 goal)
    {
        Debug.Log(goal);
        path = astar.Algorithm(transform.position, goal);
        destination = path.Pop();
        this.goal = goal;
    }

    public void ClickToMove()
    {
        if (path != null)
        {
            //Moves the enemy towards the target
            transform.parent.position = Vector2.MoveTowards(transform.parent.position, destination, 2 * Time.deltaTime);

            float distance = Vector2.Distance(destination, transform.parent.position);

            if (distance <= 0f)
            {
                if (path.Count > 0)
                {
                    destination = path.Pop();
                }
                else
                {
                    path = null;
                }
            }
        }

    }


}
