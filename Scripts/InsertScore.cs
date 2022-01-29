using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class InsertScore : MonoBehaviour
{
    [SerializeField] private ServerDataScriptableObject serverDataObject;

    [SerializeField] private TMP_InputField userIDInputField;
    [SerializeField] private TMP_InputField scoreInputField;

    public void SubmitScore()
    {
        StartCoroutine(SubmitScoreToDatabase(userIDInputField.text, scoreInputField.text));
    }


    public IEnumerator SubmitScoreToDatabase(string score, string player_id)
    {
        WWWForm form = new WWWForm();
        form.AddField("player_id", player_id);
        form.AddField("score", score);


        UnityWebRequest www = UnityWebRequest.Post("https://studentdav.hku.nl/~joris.derks/networking/insert_score_server.php", form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        } 
        else
        {
            Debug.Log(www.downloadHandler.text);
            Debug.Log("Succesfully submitted user score.");
        }
        yield return null;
    }
}
