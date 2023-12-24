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

    [Header("Level Management")]
    [SerializeField] public bool isChallengeLevel;
    [SerializeField] public int maxMoves;

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

        
       
        StartLevel();
    }
    #endregion

    #region Level Management
    public void StartLevel()
    {
        UIManager.Instance.SwitchUIPanel(UIPanelState.Gameplay);
        currentState = GameState.InGame;

        if (isChallengeLevel)
        {
            UIManager.Instance.EnableMove();
            UIManager.Instance.UpdateMoveCount("MOVES: " +maxMoves);

        }
        else
        {
            UIManager.Instance.DisableMove();
        }
        levelStartTime = Time.time;

    }

    public void AddMove(int v)
    {
        numberOfMoves += v;
        if (isChallengeLevel)
        {
            UIManager.Instance.UpdateMoveCount("Moves: " + (maxMoves - numberOfMoves));
            if (numberOfMoves >= maxMoves && currentState == GameState.InGame)
            {
                GameManager.Instance.LoseLevel();
            }
        }
    }


    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1f;
    }


    public void WinLevel()
    {
        if (currentState == GameState.InGame)
        {
            confetti.SetActive(true);
            Invoke("ShowWinUI", 1.4f);

            currentState = GameState.Win;

            PlayerPrefs.SetInt("level", currentLevel + 1);
            currentLevel++;

            foreach (MonoBehaviour m in objectsToDisable)
            {
                m.enabled = false;
            }

            SoundManager.Instance.Play(SoundManager.SoundType.Victory);
            //Send Data
            Analytics.Instance.WinLevel();

        }
    }

    public void LoseLevel()
    {
        if (currentState == GameState.InGame)
        {
            WinLevel();

            return;

            Invoke("ShowLoseUI", 2f);
            currentState = GameState.Lose;
            foreach (MonoBehaviour m in objectsToDisable)
            {
                m.enabled = false;
            }
           
            //Send Data
            Analytics.Instance.LoseLevel();
        }
    }

    public void ChangeLevel()
    {
        AdManager.Instance.ShowNormalAd(5);
    }


    public void DoChangeLevel()
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


