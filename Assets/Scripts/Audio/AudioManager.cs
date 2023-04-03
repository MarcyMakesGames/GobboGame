using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgmSlider;
    [Space]
    [SerializeField] private AudioLookup audioLookup;


    private float masterVolume = 1f;
    private float sfxAudioVolume = .5f;
    private float bgmAudioVolume = .5f;
    private float dialogueAudioVolume = .5f;


    public static AudioManager instance;

    public float MasterVolume { get => masterVolume; set => UpdateMasterVolume(value); }
    public float SFXVolume { get => sfxAudioVolume; set => UpdateSFXVolume(value); }
    public float BGMVolume { get => bgmAudioVolume; set => UpdateBGMVolume(value); }

    public void PlaySound(AudioClip clipToPlay) => sfxAudioSource.PlayOneShot(clipToPlay);

    public void PlaySound(SoundType sound)
    {
        PlaySound(audioLookup.GetSound(sound));
    }


    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        bgmAudioSource.loop = true;

        //We'll use this if we develop a playerprefs setup for players.
        //MasterVolume = GameManager.Instance.Config.MasterVolume;
        //SFXVolume = GameManager.Instance.Config.SFXVolume;
        //BGMVolume = GameManager.Instance.Config.BGMVolume;
    }

    private void UpdateMasterVolume(float value)
    {
        float newValue = value / 10f;
        masterVolume = newValue;
        sfxAudioSource.volume = sfxAudioVolume * masterVolume;
        bgmAudioSource.volume = bgmAudioVolume * masterVolume;

        //We'll use this if we develop a playerprefs setup for players.
        //GameManager.Instance.Config.MasterVolume = value;
    }

    private void UpdateSFXVolume(float value)
    {
        float newValue = value / 10f;
        sfxAudioVolume = newValue;
        sfxAudioSource.volume = sfxAudioVolume * masterVolume;

        //We'll use this if we develop a playerprefs setup for players.
        //GameManager.Instance.Config.SFXVolume = value;

    }

    private void UpdateBGMVolume(float value)
    {
        float newValue = value / 10f;
        bgmAudioVolume = newValue;
        bgmAudioSource.volume = bgmAudioVolume * masterVolume;

        //We'll use this if we develop a playerprefs setup for players.
        //GameManager.Instance.Config.BGMVolume = value;
    }

    [System.Serializable]
    private class AudioLookup
    {
        [Header("General SFX")]
        [SerializeField] private AudioClip highBlip;
        [SerializeField] private AudioClip highMidBlip;
        [SerializeField] private AudioClip LowMidBlip;
        [SerializeField] private AudioClip LowBlip;
        [SerializeField] private AudioClip Celebration;
        [SerializeField] private AudioClip Failure;
        [SerializeField] private AudioClip Pickup;
        [SerializeField] private AudioClip Drop;

        public AudioClip GetSound(SoundType sound)
        {
            switch (sound)
            {
                case SoundType.highBlip:
                    return highBlip;
                case SoundType.highMidBlip:
                    return highMidBlip;
                case SoundType.LowMidBlip:
                    return LowMidBlip;
                case SoundType.LowBlip:
                    return LowBlip;
                case SoundType.Celebration:
                    return Celebration;
                case SoundType.Failure:
                    return Failure;
                case SoundType.Pickup:
                    return Pickup;
                case SoundType.Drop:
                    return Drop;
            }

            return null;
        }
    }
}
