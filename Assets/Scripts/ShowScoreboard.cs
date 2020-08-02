using System.Collections;
using UnityEngine;
using TMPro;
using SimpleJSON;
using UnityEngine.Networking;

public class ShowScoreboard : MonoBehaviour
{
    public ArrayList playerScores = new ArrayList();
    void Awake()
    {
       StartCoroutine(GetText("https://summergame-117eb.firebaseio.com/.json"));
    }
    
    IEnumerator GetText(string uri)
    {
       using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                string top5 = "\n";
                JSONNode data = JSON.Parse(webRequest.downloadHandler.text);
                foreach(JSONNode player in data)
                {
                    
                    string temp =player["nombreUsuario"];
                    string userName = temp.Replace('"', ' ');
                    if(int.Parse($"{player["scoreSeg"]}") < 10){
                        playerScores.Add($"{player["scoreMin"]}:0{player["scoreSeg"]} --> "+userName + "\n"); 
                    }
                    else{
                        playerScores.Add($"{player["scoreMin"]}:{player["scoreSeg"]} --> "+userName + "\n"); 
                    }
                }
                playerScores.Sort();
                int i = 1;
                foreach(string score in playerScores)
                {
                    GetComponent<TMP_Text>().text += i +". " +score;
                    
                    i++;
                }
                
            }
        }
    }

   
}
