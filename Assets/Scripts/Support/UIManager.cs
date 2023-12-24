using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
                if (_instance == null)
                {
                    GameObject singleton = new GameObject("UIManager");
                    _instance = singleton.AddComponent<UIManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

    #region Properties
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
    [SerializeField] private TextMeshProUGUI inGameLevelText = null;
    [SerializeField] private TextMeshProUGUI winLevelText = null;
    [SerializeField] private TextMeshProUGUI loseLevelText = null;
    [SerializeField] private TextMeshProUGUI comboText = null;
    [SerializeField] private TextMeshProUGUI moveText = null;
    [SerializeField] private TextMeshProUGUI discsText = null;

    [SerializeField] private GameObject moveHolder = null;



    [SerializeField] private Text debugText = null;

    [Header("Settings")]
    [SerializeField] private GameObject settingsBox;
    [SerializeField] private Sprite enabledVibration;
    [SerializeField] private Sprite disabledVibration;
    [SerializeField] private Sprite disabledSFX;
    [SerializeField] private Sprite enabledSFX;
    [SerializeField] private Button SFX;
    [SerializeField] private Button vibration;

    [Header("Reward/Coins")]
    [SerializeField] List<Text> allCurrentCoins = null;
    [SerializeField] List<Transform> coins = null;

    [Header("Post Level")]
    [SerializeField] Button multiplyReward;
    [SerializeField] Text multiplyText;
    [SerializeField] Text levelReward;

    [Header("Daily")]
    [SerializeField] Button dailyReward;
    [SerializeField] Text dailyText;

    Transform coinBarPos;
    #endregion

    #region MonoBehaviour Functions
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        _instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("vibrate", 1) == 0)
        {
            vibration.image.sprite = disabledVibration;
        }
        else
        {
            vibration.image.sprite = enabledVibration;
        }

        if (PlayerPrefs.GetInt("sound", 1) == 0)
        {
            SFX.image.sprite = disabledSFX;
        }
        else
        {
            SFX.image.sprite = enabledSFX;
        }
    }
    #endregion

    #region UI Panel Management
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
    #endregion
    #region Update UI Elements
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

    public void UpdateComboText(int v)
    {
        comboText.text = "COMBO x" + v;

        comboText.gameObject.SetActive(true); // Activate the text
        comboText.transform.localScale = Vector3.one; // Reset the scale

        // Scale up-down "punch" using DOTween
        comboText.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 1f, 3, 0.5f)
.OnComplete(() =>
        {
        // Deactivate the text after 2 seconds
        StartCoroutine(DeactivateTextAfterDelay(1f));
        });

    }

    private IEnumerator DeactivateTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        comboText.gameObject.SetActive(false);
    }
    public void UpdateCurrentCoins(int v)
    {
        foreach (Text t in allCurrentCoins)
        {
            t.text = v + "";
        }
    }

    public void UpdateDiscs(int count)
    {
        discsText.text = count + " Left!";

        discsText.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 1f, 3, 0.5f).OnComplete(() =>
        {
            // Deactivate the text after 2 seconds
            discsText.transform.localScale = Vector3.one;
        });

    
}

    public void UpdateLevelReward(int v)
    {
        levelReward.text = "+" + v + "";
    }

    public void UpdateMoveCount(string s)
    {
        moveText.text = s;
    }

    public void EnableMove()
    {
        moveHolder.SetActive(true);
    }

    public void DisableMove()
    {
        moveHolder.SetActive(false);
    }
    #endregion

    #region OnClick UI Buttons
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

    public void OnClickSFXButton()
    {
        if (PlayerPrefs.GetInt("sound", 1) == 1)
        {
            PlayerPrefs.SetInt("sound", 0);
            SFX.image.sprite = disabledSFX;
        }
        else
        {
            PlayerPrefs.SetInt("sound", 1);
            SFX.image.sprite = enabledSFX;
        }
    }

    public async void SendPoolTo(bool add, Vector3 worldPos)
    {
        // Existing implementation
    }

    public void OnClickVibrateButton()
    {
        // Existing implementation
    }

    public void OnClickSettingsButton()
    {
        settingsBox.SetActive(!settingsBox.activeSelf);
    }
    #endregion

    #region Spawn Texts
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

}
