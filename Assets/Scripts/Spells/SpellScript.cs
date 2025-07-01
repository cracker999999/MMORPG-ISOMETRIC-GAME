using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    /// <summary>
    /// El objetivo de los hechizos
    /// </summary>
    public Transform MyTarget { get; private set; }

    /// <summary>
    /// Una referencia al cuerpo rígido del hechizo.
    /// </summary>
    private Rigidbody2D myRigidBody;

    private Transform source;

    private int damage;




    // Start is called before the first frame update
    void Start()
    {
        //Crea una referencia al cuerpo rígido del hechizo.
        myRigidBody = GetComponent<Rigidbody2D>();

        
    }

    public void Initialize(Transform target, int damage, Transform source)
    {
        this.MyTarget = target;
        this.damage = damage;
        this.source = source;
    }

    private void FixedUpdate()
    {
        if (MyTarget != null)
        {
            //Pone la posicion de la animacion del hechizo encima del objetivo que le hemos asignado
            transform.position = MyTarget.position;
          

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "HitBox" && collision.transform == MyTarget)
        {
            Character c = collision.GetComponentInParent<Character>();
            c.TakeDamage(damage, source);

            MyTarget = null;
        }
    }

}
