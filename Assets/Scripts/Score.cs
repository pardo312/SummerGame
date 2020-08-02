using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public GameObject player;
    public GameObject textMeshObjectMinutos;
    private MovementBall movementScript;
    private TMP_Text textMeshMin;
    private TMP_Text textMeshSeg;
    public float segundos;
    public float minutos;
    private int wait = 0;

void Awake() {
    movementScript = player.GetComponent<MovementBall>();
    textMeshSeg = GetComponent<TMP_Text>();
    textMeshMin = textMeshObjectMinutos.GetComponent<TMP_Text>();
    segundos = 0;
}
    // Update is called once per frame
    void FixedUpdate()
    {
     
        if(movementScript.inicio){
            wait+=1;
            if(wait == 50)
            {
                segundos += 1;
                if(segundos == 60)
                {
                segundos = 0;
                minutos += 1;
                }
                textMeshMin.text = minutos.ToString();
                if(segundos < 10)
                {
                    textMeshSeg.text = "0"+segundos.ToString();
                }
                else{
                    textMeshSeg.text = segundos.ToString();
                }
                
                wait = 0;
            }
        }
        else{
            if(segundos!= 0 || minutos!=0)
            {
                segundos = 0;
                minutos = 0;
            }
        }
    }
}
