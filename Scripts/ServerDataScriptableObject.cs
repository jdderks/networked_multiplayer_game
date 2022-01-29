using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ServerData", menuName = "ScriptableObjects/New Server Data")]
public class ServerDataScriptableObject : ScriptableObject
{
    public string id;
    public string servername;

    private void OnDestroy()
    {
        id = string.Empty;
        servername = string.Empty;
    }
}
