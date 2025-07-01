using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour{

    /// <summary>
    /// Imagen que estamos cambiando
    /// </summary>
    private Image content;

    /// <summary>
    /// A reference to the value text on the bar
    /// </summary>
    [SerializeField]
    private Text statValue;

    [SerializeField]
    private Text nameValue;


    /// <summary>
    /// Velocidad
    /// </summary>
    private float speed = 1;

    /// <summary>
    /// Relleno actual
    /// </summary>
    private float currentFill;

    /// <summary>
    /// El valor máximo del estado, por ejemplo, salud máxima o pa
    /// </summary>
    public float MyMaxValue { get; set; }

    /// <summary>
    /// El valor actual, por ejemplo, la salud o maná pa
    /// </summary>
    private float currentValue;

    /// <summary>
    /// Propiedad para establecer el valor actual, esto debe usarse cada vez que cambiamos el valor actual, para que todo se actualice correctamente
    /// </summary>
    public float MyCurrentValue {
        get {

           return currentValue;
        }
        set {

            if(value > MyMaxValue) {  //Se asegura de que no tengamos demasiada salud.

                 currentValue = MyMaxValue;
            }
            else if(value < 0) //Se asegura de que no tengamos salud por debajo de 0
            {
                currentValue = 0;
            }
            else
            { //Se asegura de establecer el valor actual dentro de los límites de 0 para la salud máxima
                currentValue = value;
            }
            //Calcula el archivo actual, para que podamos leer
            currentFill = currentValue / MyMaxValue;
            if(statValue != null) {
                //Se asegura de que actualizamos el texto del valor
                statValue.text = currentValue + "";
            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {

        //Crea una referencia al contenido.
        content = GetComponent<Image>();

    }

    // Update is called once per frame
    void Update(){

        //Se asegura de que actualizamos la barra
        HandleBar();
    }

    /// <summary>
    /// Inicializa la barra.
    /// </summary>
    /// <param name="currentValue">El valor actual de la barra.</param>
    /// <param name="maxValue">El valor máximo de la barra.</param>
    public void Initialize(float currentValue, float MyMaxValue)
    {
        this.MyMaxValue = MyMaxValue;
        MyCurrentValue = currentValue;
    }


    /// <summary>
    /// Se asegura de que la barra se actualice
    /// </summary>
    private void HandleBar()
    {
        if (currentFill != content.fillAmount)  //Si tenemos una nueva cantidad de relleno, entonces sabemos que necesitamos actualizar la barra
        {
            ////Sube cantidad de relleno para que tengamos un movimiento suave
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * speed);
        }

    }

}
