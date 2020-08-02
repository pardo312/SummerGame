using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class death : MonoBehaviour
{
    private MovementBall m ;
    public GameObject kart;
    public string lvlActual = "Lvl1";
    // Start is called before the first frame update
    void Start()
    {
       m =  kart.GetComponent<MovementBall>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision) {
        if(collision.collider.tag == "Death")
        {
            SceneManager.LoadScene(lvlActual);
        }
        if(collision.collider.tag == "piso")
        {
            m.onFloor = true;
            m.timeOnAir= 0;
        }
        
    }
     void OnCollisionExit(Collision collision) {
        if(collision.collider.tag == "piso")
        {
            m.onFloor = false;
            m.tempPos = m.sphere.position ;
            m.tempRot = m.sphere.rotation ;
        }
     }
    
}
