using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Momo
{
    
    public class Analytics : MonoBehaviour
    {
        [SerializeField] int appSessionCount;
        [SerializeField] int dayNumber;
        [SerializeField] int level;
        [SerializeField] float sessionTime;
        int lastLevel; 

        bool isPaused;
        Dictionary<string, string> fields = new Dictionary<string, string>();

        public static Analytics Instance = null;

        // Start is called before the first frame update
        void Awake()
        {

            GameObject[] objs = GameObject.FindGameObjectsWithTag("Analytics");

            if (objs.Length > 1)
            {
                Destroy(this.gameObject);
            }

            Application.targetFrameRate = 100;
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            Instance = this;

            DontDestroyOnLoad(this.gameObject);           
        }

        private void Start()
        {

            //Check if app opened on a new day - check when was last time opened
            //If opened on a new day, track day and level number

            appSessionCount = PlayerPrefs.GetInt("appSession", 0);
            level = PlayerPrefs.GetInt("level", 1);
          


            dayNumber = PlayerPrefs.GetInt("dayNumber", 0);
        }
  

        // Update is called once per frame
        void Update()
        {

        }


        #region App Level

        private void OnApplicationPause(bool pause)
        {
            isPaused = true;

            //Track session End with level number
        }

        private void OnApplicationFocus(bool focus)
        {
            if(focus && isPaused)
            {
                appSessionCount += 1;
                PlayerPrefs.SetInt("appSession", appSessionCount);
                isPaused = false;

                //Track session Start with level number
            }
        }

        #endregion


        #region Level Trackers

        public void StartLevel(int levelNumber)
        {
            level = levelNumber;
        }

        public void WinLevel()
        {
        }

        public void LoseLevel()
        {
        }

        public void TrackSession(SessionData sd)
        {
            Dictionary<string, object> s = new Dictionary<string, object>();
            s.Add("sessionNumber", sd.sessionNumber);
            s.Add("sessionLength", sd.sessionLength);
            s.Add("sessionLevel", sd.lastLevel);

          
        }

        public void TrackLevel(PlayerLevelData ld)
        {
            Dictionary<string, object> l = new Dictionary<string, object>();
            l.Add("levelNumber", ld.levelNumber);
            l.Add("moves", ld.numberOfMoves);
            l.Add("time", ld.timeSpent);

        }

        public void TrackDay(DayData dd)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d.Add("dayNumber", dd.DayNumber);
            d.Add("numberOfSessions", dd.numberOfSessions);

       

        }
        #endregion
        //Register daily logins
        //Register session times
        //Register last level left
        //FB/GA Login Events (Use Tiny Sauce)

    }
}
