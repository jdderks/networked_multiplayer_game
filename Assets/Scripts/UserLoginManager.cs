using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class UserLoginManager : MonoBehaviour
{

    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField passwordField;

    [SerializeField] private UserDataScriptableObject dataObject;

    [SerializeField] private UIManager uimanager;
    [SerializeField] private NetworkingManager networkingManager;

    public void GetRecentPlayers()
    {
        StartCoroutine(RecentPlayerCheck());
    }

    public void WebLogin()
    {
        StartCoroutine(Login(usernameField.text, passwordField.text));
    }

    private IEnumerator RecentPlayerCheck()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest www = UnityWebRequest.Post("https://studentdav.hku.nl/~joris.derks/networking/get_recently_logged_in.php", form);
        yield return www.SendWebRequest();
        uimanager.SetRecentPlayersText("Recent players: " + www.downloadHandler.text);

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
    }


    private IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post("https://studentdav.hku.nl/~joris.derks/networking/user_login.php", form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (!www.downloadHandler.text.Contains("Incorrect"))
            {
                UserData data = UserData.CreateFromJSON(www.downloadHandler.text);
                dataObject.id = data.id;
                dataObject.username = data.name;
                dataObject.mailadress = data.mailadress;
                dataObject.regdate = data.regdate;
            }

            if (www.downloadHandler.text.Contains("Incorrect"))
            {
                uimanager.SetUserUIOnLogin(false, "Incorrect username/password.", Color.red);
            }
            else
            {
                WWWForm logform = new WWWForm();
                logform.AddField("player_id",dataObject.id); //player ID
                logform.AddField("logtype", 0); //LogType

                UnityWebRequest logwww = UnityWebRequest.Post("https://studentdav.hku.nl/~joris.derks/networking/insert_log.php", logform);
                yield return logwww.SendWebRequest();

                uimanager.SetUserUIOnLogin(true, "Login succesful", Color.green);
                uimanager.SetRawJSONText(www.downloadHandler.text);
                networkingManager.LoggedInUser = username;
                networkingManager.LoggedIn = true;
                networkingManager.SetUserName(username);
            }
        }
        yield return null;
    }



}



