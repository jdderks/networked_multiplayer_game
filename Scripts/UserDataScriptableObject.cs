using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UserData", menuName = "ScriptableObjects/New User Data")]
public class UserDataScriptableObject : ScriptableObject
{
    public string id;
    public string username;
    public string mailadress;
    public string regdate;

    private void OnDestroy()
    {
        id =         string.Empty;
        username =   string.Empty;
        mailadress = string.Empty;
        regdate =    string.Empty;
    }
}
