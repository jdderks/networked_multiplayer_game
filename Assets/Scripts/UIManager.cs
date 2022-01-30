using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum UIState
{
    Homescreen = 1,
    Loginscreen = 2,
    Registerscreen = 3,
    UserInfoScreen = 4
}

public class UIManager : MonoBehaviour
{
    [SerializeField] private UserDataScriptableObject userDataContainer;
    [SerializeField] private ServerDataScriptableObject serverDataObject;
    [SerializeField] private NetworkingManager networkingManager;
    [SerializeField] private HighScoreManager highScoreManager;


    [SerializeField] private Button loginContinueButton;
    [SerializeField] private Button serverLoginButton;
    [SerializeField] private TextMeshProUGUI LoginStatusText;

    [SerializeField] private TextMeshProUGUI recentPlayersText;

    [SerializeField] private TextMeshProUGUI statsUsernameText;
    [SerializeField] private TextMeshProUGUI statsEmailText;
    [SerializeField] private TextMeshProUGUI statsRegdateText;
    [SerializeField] private TextMeshProUGUI rawJSONText;


    [SerializeField] private List<GameObject> screenObjects;

    [SerializeField] private Button serverLoginContinueButton;
    [SerializeField] private TextMeshProUGUI serverLoginStatusText;

    [SerializeField] private TextMeshProUGUI serverNameText;



    [SerializeField] private List<TextMeshProUGUI> leaderboardUserNameTexts = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> leaderboardScoreTexts = new List<TextMeshProUGUI>();



    UIState currentUIState = UIState.Loginscreen;

    public void ChangeUIState(int stateNmbr)
    {
        currentUIState = (UIState)stateNmbr;
        
        Debug.Log("UI State number: " + stateNmbr);

        for (int i = 0; i < screenObjects.Count; i++)
        {
            if (i == stateNmbr)
            {
                screenObjects[i].SetActive(true);
            }
            else
            {
                screenObjects[i].SetActive(false);
            }
        }
    }
    public void SetUserUIOnLogin(bool buttonsInteractable, string popupMessage, Color popupMessageColor)
    {
        loginContinueButton.interactable = buttonsInteractable;
        serverLoginButton.interactable = buttonsInteractable;
        LoginStatusText.color = popupMessageColor;
        LoginStatusText.text = popupMessage;

        statsUsernameText.text = userDataContainer.username;
        statsEmailText.text = userDataContainer.mailadress;
        statsRegdateText.text = userDataContainer.regdate;
    }

    public void SetServerUIOnLogin(bool buttonsInteractable, string popupMessage, Color popupMessageColor)
    {
        serverLoginContinueButton.interactable = buttonsInteractable;
        serverLoginButton.interactable = buttonsInteractable;
        serverLoginStatusText.color = popupMessageColor;
        serverLoginStatusText.text = popupMessage;
        serverNameText.text = serverDataObject.servername;
    }
    public void SetRawJSONText(string json)
    {
        rawJSONText.text = json;
    }

    public void SetRecentPlayersText(string text)
    {
        recentPlayersText.text = text;
    }
    public void FillLeaderboard(int limit)
    {
        for (int i = 0; i < limit; i++)
        {
            Debug.Log(leaderboardUserNameTexts[i].text);
            Debug.Log(highScoreManager.TopUsernames[i]);


            leaderboardUserNameTexts[i].text = highScoreManager.TopUsernames[i];
            leaderboardScoreTexts[i].text = highScoreManager.TopScores[i];
        }
    }
}