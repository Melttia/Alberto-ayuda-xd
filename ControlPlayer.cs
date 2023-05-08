using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPlayer : MonoBehaviour
{
    [Header("Atributos Player")]    //organizar el inspector 
    public float velocidad = 2.0f; //inicializo la variable tipo float (con f)
    public float velocidadCorrer = 6.0f;
    private float velocidadBase;    //interno del script, para poder volver a la velocidad base del personaje
    public float impulso = 3.0f; //fuerza del salto
    public bool permitirSalto;
    public int vida = 3; //número de vidas iniciales


    [Header("Componentes")]
    public Rigidbody2D rb; //siempre que declaremos una variable de tipo componente tenemos que vincilarla en el INSPECTOR de unity
    public SpriteRenderer sr; 
    public Animator anim;   //variable de tipo componente para poder acceder a ese componente. Hay que vincularlo
    public bool recibiendoGolpe;    //por defecto inicializa en falso

    private void Start() 
    {
    velocidadBase = velocidad;


    }


    private void Update ()  //se ejecuta en cada frame
    {
        if (!recibiendoGolpe)
        {
            Movimiento();       
            Salto();
            Correr();

        }
        
    }

    

    private void Movimiento () 
    {
        rb.velocity = new Vector2(Input.GetAxis ("Horizontal") * velocidad, rb.velocity.y); //permite mover el personaje

        if (Input.GetAxisRaw ("Horizontal") != 0)  //si nos estamos moviendo
        {
        anim.SetBool("isRunning", true); //si está corriendo se activa


        } else {    //si estamos quietos
            anim.SetBool("isRunning", false);

        }


        if (Input.GetAxis("Horizontal") > 0)    //detectamos si miramos a la derecha
        {
            sr.flipX = false;   //miramos a la derecha

        } else if (Input.GetAxis("Horizontal") < 0) //para que se quede mirando hacia el lado de la última vez que pulsemos la tecla
        {
            sr.flipX = true;    //miramos a la izquierda

        }
    
    }

    private void Salto () 
    {
        if (Input.GetButtonDown("Jump") && permitirSalto) //condición compruebo que se pulse la barra espaciadora. Para saltar una vez debe estar tocando el suelo
        {
            
            rb.AddForce(transform.up * impulso, ForceMode2D.Impulse ); //coge el eje vertical del espacio. 
            

        }
        
    }

    private void Correr () //la velocidad es menor o mayor en función de si pulso la tecla SHIFT o no 
    {
        if (Input.GetButton ("Fire3"))  //compruebo cuando pulso la tecla SHIFT izquierdo
        {
            velocidad = velocidadCorrer;    //si pulso SHIFT corre
        }
        if (Input.GetButtonUp ("Fire3"))  //compruebo si suelto la tecla SHIFT
        {
            velocidad = velocidadBase;  //si dejo de pulsar SHIFT vuelve a su velocidad original

        }

    }



    private void ResetearMovimiento() 
    {
        recibiendoGolpe = false;

    }


    //EVENTOS PARA COLISIÓN

    //si colisiono con el SUELO
    private void OnCollisionEnter2D(Collision2D collision)  //detecta un elemento collider en 2D, del objeto con el que choco
    {

        if (collision.gameObject.tag == "Suelo")    //detecta si el objeto suelo es el que está tocando (accediendo a la etiqueta suelo)
        {
            permitirSalto=true;

        } 
    }

    private void OnCollisionExit2D(Collision2D collision) //cuando salga de la colisión
    {
        if (collision.gameObject.tag == "Suelo")    //detecta si el objeto suelo es el que está tocando (accediendo a la etiqueta suelo)
        {
            permitirSalto=false;
 
        } 

    }

    //si colisiono con un ENEMIGO
    //private void OnCollisionEnter2D(Collision2D collision)  //detecta un elemento collider en 2D, del objeto con el que choco
    //{

        if (collision.gameObject.tag == "Enemy")    //detecta si el objeto suelo es el que está tocando (accediendo a la etiqueta suelo)
        {
            if (collision.gameObject.GetComponent<DinoEnemy>().direccion && !recibiendoGolpe) 
            {
                //si es positivo (mira a la derecha) nos empuja hacia la derecha
                recibiendoGolpe=true;
                rb.AddForce(transform.right * impulso, ForceMode2D.Impulse);
                Invoke("ResetearMovimiento", 0.5f);

            }
            if (!collision.gameObject.GetComponent<DinoEnemy>().direccion && !recibiendoGolpe)
            {
                //si es negativo (mira a la izquierda) nos empuja a la izquierda
                recibiendoGolpe=true;
                rb.AddForce(-transform.right * impulso, ForceMode2D.Impulse);
                Invoke("ResetearMovimiento", 0.5f);
            }

        } 
    //}
}