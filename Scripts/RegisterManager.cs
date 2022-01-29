using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour
{
    [SerializeField] private Button continueButton;

    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TMP_InputField emailField;

    [SerializeField] private TextMeshProUGUI registerStatusText;

    public void RegisterUser()
    {
        StartCoroutine(RegisterUser(usernameField.text, passwordField.text, emailField.text));
    }

    private IEnumerator RegisterUser(string username, string password, string email)
    {
        if (username.Contains(" ") || !CheckForNotLatin(username))
        {
            UpdateStatus("Please don't use any illegal characters in your username.");
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("email", email);

        UnityWebRequest www = UnityWebRequest.Post("https://studentdav.hku.nl/~joris.derks/networking/register_user.php", form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            continueButton.interactable = false;
            Debug.Log(www.error);
        }
        else
        {
            //Succesfully created account
            continueButton.interactable = true;
            UpdateStatus(www.downloadHandler.text);
        }
        yield return null;
    }

    private void UpdateStatus(string status)
    {
        registerStatusText.text = status;
    }

    //char can be casted to int to acquire letter's code
    //returns true if letters are latin
    bool CheckForNotLatin(string stringToCheck)
    {
        bool boolToReturn = false;
        foreach (char c in stringToCheck)
        {
            int code = (int)c;
            // for lower and upper cases respectively
            if ((code > 96 && code < 123) || (code > 64 && code < 91))
                boolToReturn = true;
            // visit http://www.dotnetperls.com/ascii-table for more codes
        }
        return boolToReturn;
    }
}
