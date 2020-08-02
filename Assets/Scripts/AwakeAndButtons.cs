using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AwakeAndButtons : MonoBehaviour
{
    public GameObject player2;
    public GameObject soloUI;
    public Camera cam;
    // Start is called before the first frame update
    void Awake()
    {
        if(ApplicationModel.multijugador){
            cam.GetComponent<Camera>().rect = new Rect (0, 0, 0.5f, 1);
            player2.SetActive(true);
            soloUI.SetActive(false);
        }
        else{  
            cam.GetComponent<Camera>().rect = new Rect (0, 0, 1, 1);
            player2.SetActive(false);
            soloUI.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void irAPantallaPrincipal()
    {
        print("hola");
        SceneManager.LoadScene("StartScreen");
    }
}
