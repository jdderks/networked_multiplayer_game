using UnityEngine;

public class UserData
{
    public string id;
    public string name;
    public string mailadress;
    public string regdate;


    public static UserData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<UserData>(jsonString);
    }
}