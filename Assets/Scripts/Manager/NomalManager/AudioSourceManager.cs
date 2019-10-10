using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager{

    private AudioSource[] audioSource;//0，播放背景音；1播放音效
    private bool playEffectMusic = true;//是否需要播放
    public bool playBGMusic = true;

    public AudioSourceManager()
    {
        audioSource = GameManager.Instance.GetComponents<AudioSource>();
    }
    //有很多不同种类的背景音，要确认需要播放哪一种，所以要参数
    public void PlayBGMusic(AudioClip audioClip)
    {
        if (!audioSource[0].isPlaying||audioSource[0].clip!=audioClip)
        {
            audioSource[0].clip = audioClip;
            audioSource[0].Play();
        }
    }
    public void PlayEffect(AudioClip audioClip)
    {
        if (playEffectMusic)//当前需要播放
        {
            audioSource[1].PlayOneShot(audioClip);//播放一次
        }
    }
    public void CloseBGMusic()
    {
        //Debug.Log(" "+audioSource[0].ToString());
        audioSource[0].Stop();
    }
    public void OpenBGMusic()
    {
        audioSource[0].Play();
    }
    //音乐开关
    public void CloseOrOpenBGMusic()
    {
        //Debug.Log(playBGMusic);
        playBGMusic = !playBGMusic;
        if (playBGMusic)
        {
            OpenBGMusic();
        }
        else
        {
            CloseBGMusic();
        }
    }
    //音效开关
    public void CloseOrOpenEffectMusic()
    {
        playEffectMusic = !playEffectMusic;//音效关了，不需要其他内容，音效都是一次性的
    }
    //按钮音效
    public void PlayButtonAudioClip()
    {
        PlayEffect(GameManager.Instance.factoryManager.audioClipFactory.GetSingleResources("Main/Button"));
    }
    //翻书音效
    public void PlayPagingAudioClip()
    {
        PlayEffect(GameManager.Instance.factoryManager.audioClipFactory.GetSingleResources("Main/Paging"));
    }
}
