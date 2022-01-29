using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExitGame : MonoBehaviour
{


    public void CloseGame()
    {
#if UNITY_EDITOR
        if (Application.isEditor)
        {
            EditorApplication.isPlaying = false;
        }
#endif
        Application.Quit();
    }
}
