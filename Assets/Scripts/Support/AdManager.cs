using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using AppodealAds.Unity.Api;
//using AppodealAds.Unity.Common;


public class AdManager : MonoBehaviour
{
    public static AdManager Instance = null;
    [Header("Attributes")]
    [SerializeField] bool isBannerOn;
    [SerializeField] bool isInterstitialOn;
    [SerializeField] bool isRewardedOn;
    [SerializeField] AdType adNetwork;
    [SerializeField] string key;
    [SerializeField] RewardType rewardState;

    bool isRewardedGD;

    public int currentId;
    private void Start()
    {
        
    }


    public void LoadBanner()
    {

    }

    public void LoadRewarded()
    {

    }

    public void LoadInterstital()
    {

    }


    public void ShowRewarded()
    {

    }

    public void ShowBanner()
    {

    }

    public void ShowInterstital()
    {

    }

    

    void ShowRewarded(int id)
    {
        currentId = id;
        //Call rewarded show
    }

    void ShowRewardedCallBack()
    {
        switch (currentId)
        {

            case 1:

                break;
            case 2:

                break;
            case 3:

                break;
            case 4:

                break;

        }
    }

}
