using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    #region Properties
    public static UIManager Instance = null;

    [Header("Components Reference")]

    [SerializeField] private GameObject PointText;
    [SerializeField] private GameObject AwesomeText;
    [SerializeField] private GameObject JoyStick;  

    [Header("UI Panel")]
    [SerializeField] private GameObject mainMenuUIPanel = null;
    [SerializeField] private GameObject gameplayUIPanel = null;
    [SerializeField] private GameObject gameOverWinUIPanel = null;
    [SerializeField] private GameObject gameOverLoseUIPanel = null;
    [SerializeField] private TextMeshProUGUI scoreText = null;
    [SerializeField] private Text mainLevelText = null;
    [SerializeField] private Text inGameLevelText = null;
    [SerializeField] private Text winLevelText = null;
    [SerializeField] private Text loseLevelText = null;
    [SerializeField] private Text debugText = null;


    [Header("Reward/Coins")]
    [SerializeField] List<Text> allCurrentCoins = null;

    [Header("Post Level")]
    [SerializeField] Button multiplyReward;
    [SerializeField] Text multiplyText;
    [SerializeField] Text levelReward;


    [Header("Daily")]
    [SerializeField] Button dailyReward;
    [SerializeField] Text dailyText;



    #endregion

    #region MonoBehaviour Functions
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        //SwitchControls(Controls.Touch);
    }
    #endregion

    #region Getter And Setter

    #endregion

    #region Public Core Functions
    public void SwitchUIPanel(UIPanelState state)
    {
        switch (state)
        {
            case UIPanelState.MainMenu:
                mainMenuUIPanel.SetActive(true);
                gameplayUIPanel.SetActive(false);
                gameOverWinUIPanel.SetActive(false);
                gameOverLoseUIPanel.SetActive(false);
                break;
            case UIPanelState.Gameplay:
                mainMenuUIPanel.SetActive(false);
                gameplayUIPanel.SetActive(true);
                gameOverWinUIPanel.SetActive(false);
                gameOverLoseUIPanel.SetActive(false);
                break;
            case UIPanelState.GameWin:
                mainMenuUIPanel.SetActive(false);
                gameplayUIPanel.SetActive(false);
                gameOverWinUIPanel.SetActive(true);
                gameOverLoseUIPanel.SetActive(false);
                break;
            case UIPanelState.GameLose:
                mainMenuUIPanel.SetActive(false);
                gameplayUIPanel.SetActive(false);
                gameOverWinUIPanel.SetActive(false);
                gameOverLoseUIPanel.SetActive(true);
                break;
        }
    }

    public void UpdateScore(int value)
    {
        scoreText.text = "" + value;
    }

    public void UpdateDebugText(string s)
    {
        debugText.text = s;
    }
    public void UpdateLevel(int level)
    {
        mainLevelText.text = "LEVEL " + level;
        inGameLevelText.text = "LEVEL " + level;
        winLevelText.text = "LEVEL " + level;
        loseLevelText.text = "LEVEL " + level;

    }

    public void UpdateCurrentCoins(int v)
    {
        foreach(Text t in allCurrentCoins)
        {
            t.text = v + "";
        }
    }

    public void UpdateLevelReward(int v)
    {
        levelReward.text ="+"+ v + "";
    }

    #region Give Rewards

    #endregion

    #region OnClickUIButtons    

    public void OnClickPlayButton()
    {
        GameManager.Instance.StartLevel();
    }

    public void OnClickChangeButton()
    {
        GameManager.Instance.ChangeLevel();
    }

    public void OnClickMove()
    {
        GameManager.Instance.AddMove(1);
    }

    public void OnClickWin()
    {
        GameManager.Instance.WinLevel();
    }

    #endregion


    public void SpawnPointText(Vector3 point)
    {
        Instantiate(PointText, point, Quaternion.identity);
    }

    public void SpawnAwesomeText(Vector3 point, string s)
    {
        GameObject g = Instantiate(AwesomeText, new Vector3(point.x, 2, point.z), Quaternion.identity);
        g.GetComponentInChildren<TextMeshPro>().text = s;
    }


    #endregion


    #region Button Click events


    #endregion
}




