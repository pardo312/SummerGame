using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowPlayerNameUI : MonoBehaviour
{
       private TMP_Text textMeshMin;
    // Start is called before the first frame update
    void Start()
    {
        textMeshMin = GetComponent<TMP_Text>();
        textMeshMin.text = ApplicationModel.nombreDeJugador;
    }

}
