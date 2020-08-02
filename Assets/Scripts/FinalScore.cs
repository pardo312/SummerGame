using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FinalScore : MonoBehaviour
{
    
    public GameObject textMeshObjectMinutos;
    private TMP_Text textMeshMin;
    private TMP_Text textMeshSeg;
    // Start is called before the first frame update
    void Start()
    {
        textMeshSeg = GetComponent<TMP_Text>(); 
        textMeshMin = textMeshObjectMinutos.GetComponent<TMP_Text>(); 
        textMeshSeg.text = ApplicationModel.scoreJugadorSeg;
        textMeshMin.text = ApplicationModel.scoreJugadorMin;
    }

}
