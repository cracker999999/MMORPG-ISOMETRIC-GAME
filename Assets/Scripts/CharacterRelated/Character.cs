using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{


    /// <summary>
    /// Velocidad de movimiento del jugador.
    /// </summary>
    [SerializeField]
    protected float speed;

    /// <summary>
    /// A reference to the character's animator
    /// </summary>
    public Animator MyAnimator { get; set; }

    /// <summary>
    /// Direccion del jugador
    /// </summary>
    protected Vector2 direction;

    /// <summary>
    /// El rigidbody del personaje
    /// </summary>
    private Rigidbody2D myRigidbody;

    /// <summary>
    /// indicates if the character is attacking or not
    /// </summary>
    public bool IsAttacking { get; set; }

    /// <summary>
    /// Referencia a la Coroutine.
    /// </summary>
    protected Coroutine attackRoutine;

    [SerializeField]
    protected Transform hitBox;

    [SerializeField]
    protected Stat health;


    public Stat MyHealth {
        get
        {
            return health;
        }
    }

   
    /// <summary>
    /// Vida inicial del caracter
    /// </summary>
    [SerializeField]
    private float initHealth;

    public Transform MyTarget {get; set;  }

    public Vector2 Direction
    {
        get
        {
            return direction;
        }

        set
        {
            direction = value;
        }
    }


    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }


    public bool IsAlive
    {
        get
        {
            return health.MyCurrentValue > 0;
        }
    }


    /// <summary>
    /// Indica si el personaje se mueve o no
    /// </summary>
    public bool IsMoving
    {
        get
        {
            return direction.x != 0 || direction.y != 0;
        }
    }
    protected virtual void Start()
    {
     

        //Inicializamos la vida y los PA
        health.Initialize(initHealth, initHealth);

        // Enlazamos con el componente
        myRigidbody = GetComponent<Rigidbody2D>();

        //Hace una referencia al animador de personajes.
        MyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
        //Elige que myAnimator verse en cada momento
        HandleLayers();

    }

    private void FixedUpdate()
    {
        Move();

    }

    /// <summary>
    /// Mueve al jugador
    /// </summary>
    public void Move()
    {
        if (!IsAttacking && IsAlive)
        {
            //Se asegura de que el jugador se mueva
            myRigidbody.velocity = direction.normalized * speed;
        }

    }



    /// <summary>
    /// Se asegura de que se está reproduciendo la capa de animación correcta
    /// </summary>
    public void HandleLayers()
    {
        if (IsAlive)
        {
            if (IsMoving && !IsAttacking)
            {

                //Si se está moviendo activa la animacion y setea la direccion para que se gire corectamente
                ActivateLayer("WalkLayer");
                MyAnimator.SetFloat("x", direction.x);
                MyAnimator.SetFloat("y", direction.y);

            }
            else if (IsAttacking)
            {
                //Si está atacando activa la animacion
                ActivateLayer("AttackLayer");


            }
            else
            {
                //Sino pone el personaje en idle
                ActivateLayer("IdleLayer");
            }
        }
        else
        {
            ActivateLayer("DeathLayer");
        }
    }

    /// <summary>
    /// Activa una capa de animación al pasarle el string y desactiva las demás
    /// </summary>
    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < MyAnimator.layerCount; i++)
        {
            MyAnimator.SetLayerWeight(i, 0);
        }
        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
    }



    public virtual void TakeDamage(float damage, Transform source)
    {

        if (MyTarget == null)
        {
            MyTarget = source;
        } 

        health.MyCurrentValue -= damage;

        if(health.MyCurrentValue <= 0)
        {
            //Makes sure that the character stops moving when its dead
            Direction = Vector2.zero;
            myRigidbody.velocity = Direction;
            MyAnimator.SetTrigger("die");
        }
    }

}
