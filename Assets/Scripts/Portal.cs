using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject player;
    
    public GameObject kart;
    
    public GameObject PortalOut;

    void OnCollisionEnter(Collision other) {
       if(other.collider.tag == "Player")
        {
            //Para la velocidad y velocidad angular del jugador
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.transform.position = PortalOut.transform.position - new Vector3(10f,0f,0f);
        }
    }
}
