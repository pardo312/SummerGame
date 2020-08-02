using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text;
using SimpleJSON;

public class EndRace : MonoBehaviour
{
    public string lvlSiguiente = "Lvl2";
    public GameObject kart;
    public GameObject score;
    
    public GameObject scoreBoard;
    void OnCollisionEnter(Collision other) {
        if(other.collider.tag == "Player")
        {
            kart.GetComponent<MovementBall>().inicio = false;
            
            if(score.GetComponent<Score>().segundos < 10)
            {
                ApplicationModel.scoreJugadorSeg = "0" + score.GetComponent<Score>().segundos.ToString() ;
            }
            else{
                ApplicationModel.scoreJugadorSeg = score.GetComponent<Score>().segundos.ToString();
            }
            ApplicationModel.scoreJugadorMin = score.GetComponent<Score>().minutos.ToString() ;
        
        StartCoroutine(Upload());
        
        SceneManager.LoadScene(lvlSiguiente);
        }
    }
    IEnumerator Upload( )
    {
        bool mejorPuntaje = false;
        bool noExiste = true;
        ArrayList usuarios = scoreBoard.GetComponent<ShowScoreboard>().playerScores;
        foreach(string player in usuarios){
            string[] strArr= player.Split(';');
            if(strArr[0].Equals(ApplicationModel.nombreDeJugador))
            {
                noExiste = false;
                print("minutos: "+strArr[1]);
                if(int.Parse(strArr[1]) >= score.GetComponent<Score>().minutos)
                {
                print("segundos: "+int.Parse(strArr[2]));
                    if(int.Parse(strArr[2]) > score.GetComponent<Score>().segundos)
                    {
                        print("FINAL: "+(strArr[0]));
                        mejorPuntaje = true;
                    }  
                   
                }                
            }
        }
        if(mejorPuntaje || noExiste)
        {
            string addition = ('"' + "" );
            string bodyJsonString ="{"+addition+"nombreUsuario"+addition+":"+addition+ ApplicationModel.nombreDeJugador+addition+","+addition+"scoreMin"+addition+":"+score.GetComponent<Score>().minutos+","+addition+"scoreSeg"+addition+":"+score.GetComponent<Score>().segundos+"}";
            var request = new UnityWebRequest("https://summergame-117eb.firebaseio.com/"+ApplicationModel.nombreDeJugador+".json", "PUT");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
            request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
        }
        
    }
}
