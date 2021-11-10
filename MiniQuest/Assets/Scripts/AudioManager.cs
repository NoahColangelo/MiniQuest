using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//code from Hooson on youtube https://www.youtube.com/watch?v=yWCHaTwVblk

public class AudioManager : MonoBehaviour
{

    [SerializeField]
    private Slider SoundSlider;

    void Start()
    {
        if(!PlayerPrefs.HasKey("SoundVolume"))//if there are no player prefs for sound then itll make it
        {
            PlayerPrefs.SetFloat("SoundVolume", 1.0f);
            Load();
        }
        else//else load the pref
        {
            Load();
        }
    }

    public void OnBGMSliderChange()//changes the volume via a slider and saves to the player prefs
    {
        AudioListener.volume = SoundSlider.value;
        Save();
    }

    private void Load()//loads the sound value from the player prefs
    {
        if (SoundSlider != null)
            SoundSlider.value = PlayerPrefs.GetFloat("SoundVolume");
        else
            AudioListener.volume = PlayerPrefs.GetFloat("SoundVolume");
    }

    private void Save()//saves to the player prefs
    {
        PlayerPrefs.SetFloat("SoundVolume", SoundSlider.value);
    }
}
