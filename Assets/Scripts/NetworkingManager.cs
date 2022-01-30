using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using TMPro;

public class NetworkingManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loggedInAsText;

    [SerializeField] private TextMeshProUGUI serverSessionIDText;

    [SerializeField] private UserDataScriptableObject dataObject;

    private string loggedInUser = "";
    private string loggedInUserID = "";
    private bool loggedIn = false;

    private string sessionID = "000000";

    public string LoggedInUser { get => loggedInUser; set => loggedInUser = value; }
    public string LoggedInUserID { get => loggedInUserID; set => loggedInUserID = value; }
    public bool LoggedIn { get => loggedIn; set => loggedIn = value; }

    public void Logout()
    {
        loggedInUser   = string.Empty;
        loggedInUserID = string.Empty;
        loggedIn       = false;
        StartCoroutine(InsertLogoutLog());
    }

    public IEnumerator InsertLogoutLog()
    {
        Debug.Log("Starting LOGOUT LOG");
        WWWForm logform = new WWWForm();
        logform.AddField("player_id", dataObject.id); //player ID
        logform.AddField("logtype", 1); //LogType

        UnityWebRequest logwww = UnityWebRequest.Post("https://studentdav.hku.nl/~joris.derks/networking/insert_log.php", logform);
        yield return logwww.SendWebRequest();
        Debug.Log("Succes?: " + logwww.downloadHandler.text);
    }

    public string GetSessionID()
    {
        return sessionID.ToString();
    }

    public void SetUserName(string username)
    {
        loggedInAsText.text = "Currently logged in as: " + username;
    }

    public IEnumerator GetSessionIDFromDataBase()
    {
        WWWForm form = new WWWForm();

        UnityWebRequest www = UnityWebRequest.Post("https://studentdav.hku.nl/~joris.derks/networking/get_session.php", form);
        yield return www.SendWebRequest();
        serverSessionIDText.text = www.downloadHandler.text;
    }
}
