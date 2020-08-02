using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void enterGame()
    {
        SceneManager.LoadScene("Lvl1");
    }
    public void changePlayerName(string param)
    {
        ApplicationModel.nombreDeJugador = param;
    }
    public void changePlaySolo()
    {
        ApplicationModel.multijugador = false;
    }
    public void changePlayMultiplayer()
    {
        ApplicationModel.multijugador = true;
    }
}
