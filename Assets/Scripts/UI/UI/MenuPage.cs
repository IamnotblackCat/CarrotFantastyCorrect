using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPage : MonoBehaviour {

    private NormalModelPanel normalModelPanel;

    private void Awake()
    {
        normalModelPanel = GetComponentInParent<NormalModelPanel>();
    }
    //private void OnEnable()
    //{
    //    GameController.Instance.isPause = true;
    //    gameObject.SetActive(true);
    //}
    public void ContinueGame()
    {
        GameManager.Instance.audioSourceManager.PlayButtonAudioClip();
        GameController.Instance.isPause = false;
        gameObject.SetActive(false);
    }
    public void Replay()
    {
        GameManager.Instance.audioSourceManager.PlayButtonAudioClip();
        normalModelPanel.Replay();
    }
    public void ChooseLevel()
    {
        GameManager.Instance.audioSourceManager.PlayButtonAudioClip();
        normalModelPanel.ChooseOtherLevel();
    }
}
