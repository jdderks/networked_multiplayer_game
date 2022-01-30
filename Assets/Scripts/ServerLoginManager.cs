using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System;

public class ServerLoginManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField serverNameField;
    [SerializeField] private TMP_InputField passwordField;


    [SerializeField] private ServerDataScriptableObject serverDataObject;

    [SerializeField] private UIManager uimanager;
    [SerializeField] private NetworkingManager networkingManager;

    public void ServerLogin()
    {
        StartCoroutine(Login(serverNameField.text, passwordField.text));
        StartCoroutine(networkingManager.GetSessionIDFromDataBase());
    }


    private IEnumerator Login(string servername, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("servername", servername);
        form.AddField("serverpassword", password);

        UnityWebRequest www = UnityWebRequest.Post("https://studentdav.hku.nl/~joris.derks/networking/server_login.php", form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string test = www.downloadHandler.text;

            if (www.downloadHandler.text.Contains("Incorrect"))
            {
                Debug.Log(www.downloadHandler.text);
                uimanager.SetServerUIOnLogin(false, "Incorrect servername/password.", Color.red);
            }
            else
            {
                Debug.Log("not 0");
                uimanager.SetServerUIOnLogin(true, "Login succesful", Color.green);
                ServerData data = ServerData.CreateFromJSON(www.downloadHandler.text);

                serverDataObject.id = data.id;
                serverDataObject.servername = data.name;

                Debug.Log(data.id);
            }
        }
        yield return null;
    }
}



