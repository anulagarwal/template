using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Momo;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject singleton = new GameObject("GameManager");
                    _instance = singleton.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

    #region Properties
    [Header("Component Reference")]
    [SerializeField] private GameObject confetti;
    [SerializeField] private List<MonoBehaviour> objectsToDisable;

    [Header("Game Attributes")]
    [SerializeField] private int currentScore;
    [SerializeField] private int currentLevel;
    [SerializeField] private GameState currentState;
    [SerializeField] private int numberOfMoves;
    [SerializeField] private float levelLength;

    public int CurrentScore => currentScore;
    public int CurrentLevel => currentLevel;
    public GameState CurrentState => currentState;

    private float levelStartTime;
    #endregion

    #region MonoBehaviour Functions
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        _instance = this;
        Application.targetFrameRate = 100;
    }

    private void Start()
    {
        currentLevel = PlayerPrefs.GetInt("level", 1);
        UIManager.Instance.UpdateLevel(currentLevel);
        currentState = GameState.Main;
    }
    #endregion

    #region Level Management
    public void StartLevel()
    {
        UIManager.Instance.SwitchUIPanel(UIPanelState.Gameplay);
        currentState = GameState.InGame;
        Analytics.Instance.StartLevel(currentLevel);
        levelStartTime = Time.time;
    }

    public void AddMove(int v)
    {
        numberOfMoves += v;
    }

    public void WinLevel()
    {
        if (currentState == GameState.InGame)
        {
            //confetti.SetActive(true);
            Invoke("ShowWinUI", 1.4f);

            currentState = GameState.Win;

            PlayerPrefs.SetInt("level", currentLevel + 1);
            currentLevel++;

            foreach (MonoBehaviour m in objectsToDisable)
            {
                m.enabled = false;
            }
            levelLength = Time.time - levelStartTime;
            PlayerLevelData pld = new PlayerLevelData();
            pld.Init(currentLevel, 0, true, numberOfMoves, levelLength);

            if (currentLevel == 5)
            {
                if (PlayerPrefs.GetInt("review", 0) == 0)
                {
                    GetComponent<ReviewsManager>().Request();
                    PlayerPrefs.SetInt("review", 1);
                }
            }
            PlayerManager.Instance.AddLevelData(pld);
            //Send Data
            Analytics.Instance.WinLevel();

        }
    }

    public void LoseLevel()
    {
        if (currentState == GameState.InGame)
        {
            Invoke("ShowLoseUI", 2f);
            currentState = GameState.Lose;
            foreach (MonoBehaviour m in objectsToDisable)
            {
                m.enabled = false;
            }
            levelLength = Time.time - levelStartTime;
            PlayerLevelData pld = new PlayerLevelData();
            pld.Init(currentLevel, 1, false, numberOfMoves, levelLength);
            PlayerManager.Instance.AddLevelData(pld);
            //Send Data
            Analytics.Instance.LoseLevel();
        }
    }

    public void ChangeLevel()
    {
        SceneManager.LoadScene("Core");
    }
    #endregion

    #region Public Core Functions
    public void AddScore(int value)
    {
        currentScore += value;
        UIManager.Instance.UpdateScore(currentScore);
    }
    #endregion

    #region Invoke Functions
    private void ShowWinUI()
    {
        UIManager.Instance.SwitchUIPanel(UIPanelState.GameWin);
    }

    private void ShowLoseUI()
    {
        UIManager.Instance.SwitchUIPanel(UIPanelState.GameLose);
    }
    #endregion
}


