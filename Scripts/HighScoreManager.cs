using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System;

public class HighScoreManager : MonoBehaviour
{
    private string[] topUsernames;
    private string[] topScores;

    public string[] TopUsernames { get => topUsernames; set => topUsernames = value; }
    public string[] TopScores { get => topScores; set => topScores = value; }

    private void OnEnable()
    {
        GetHighscores();
    }

    public void GetHighscores()
    {
        StartCoroutine(RetrieveHighScores(5));
        StartCoroutine(RetrieveTopUsers(5));
        Debug.Log(topUsernames);
        
    }

    public IEnumerator RetrieveHighScores(int limit)
    {
        WWWForm form = new WWWForm();
        form.AddField("limit", limit);

        UnityWebRequest www = UnityWebRequest.Post("https://studentdav.hku.nl/~joris.derks/networking/get_top_scores.php", form);
        yield return www.SendWebRequest();
        string handlerText = www.downloadHandler.text;
        handlerText = handlerText.Substring(0, handlerText.Length - 1);
        TopScores = handlerText.Split(',');

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
    }

    public IEnumerator RetrieveTopUsers(int limit)
    {
        WWWForm form = new WWWForm();
        form.AddField("limit", limit);

        UnityWebRequest www = UnityWebRequest.Post("https://studentdav.hku.nl/~joris.derks/networking/get_top_users.php", form);
        yield return www.SendWebRequest();

        string handlerText = www.downloadHandler.text;
        handlerText = handlerText.Substring(0, handlerText.Length - 1);
        TopUsernames = handlerText.Split(',');
        Debug.Log(handlerText);

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }

        for (int i = 0; i < topUsernames.Length; i++)
        {
            StartCoroutine(SetUserIDsToUsernames(topUsernames[i], i));
        }
    }

    public IEnumerator SetUserIDsToUsernames(string id, int index)
    {
        Debug.Log("ID: " + id + " " + "Index: " + index);
        WWWForm form = new WWWForm();
        form.AddField("id", id);

        UnityWebRequest www = UnityWebRequest.Post("https://studentdav.hku.nl/~joris.derks/networking/get_playername_by_id.php", form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            topUsernames[index] = www.downloadHandler.text;
        }
    }





}
