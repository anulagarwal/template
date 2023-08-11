using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Spine;
using Spine.Unity;

public class PrefabLoader : MonoBehaviour
{
    public SkeletonAnimation[] characterPrefabs = new SkeletonAnimation[12];
    private int _counter = 0;

    private SkeletonAnimation _characterCell = null;

    // UI
    public Toggle animationLoopToggle;
    public Text characterNameText;

    void Start()
    {
        LoadPrefab();
    }

    public void GetPreviousCharacter()
    {
        _counter--;
        if (_counter < 0) _counter = characterPrefabs.Length - 1;

        LoadPrefab();
    }

    public void GetNextCharacter()
    {
        _counter++;
        if (_counter == characterPrefabs.Length) _counter = 0;

        LoadPrefab();
    }

    public void SetAnimation(string animationName)
    {
        _characterCell.AnimationState.SetAnimation(0, animationName, animationLoopToggle.isOn);
    }

    private void LoadPrefab()
    {
        if(_characterCell != null)
        {
            _characterCell.gameObject.SetActive(false);
            _characterCell = null;
        }

        _characterCell = characterPrefabs[_counter];
        _characterCell.gameObject.SetActive(true);

        characterNameText.text = _characterCell.gameObject.name;
        SetAnimation("idle");
    }
}
