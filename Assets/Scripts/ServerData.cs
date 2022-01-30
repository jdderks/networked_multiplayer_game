using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerData
{
    public string id;
    public string name;

    public static ServerData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ServerData>(jsonString);
    }
}
