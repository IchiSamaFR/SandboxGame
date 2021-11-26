using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsCollection : MonoBehaviour
{
    public static int SoundEffect = 100;
    public static int SoundMusic = 100;
    public static bool Vibration = true;

    public static int Graphic = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if (PlayerPrefs.GetString("settings") != "")
        {
            //JsonConverter.LoadSettings();
        }
        else
        {
            //JsonConverter.SaveSettings();
        }
    }

    public static void EffectVolume(Slider slider)
    {
        SoundEffect = (int)(slider.value);
        //JsonConverter.SaveSettings();
    }
    public static void MusicVolume(Slider slider)
    {
        SoundMusic = (int)(slider.value);
        //JsonConverter.SaveSettings();
    }
    public static void VibrationActive(Toggle toggle)
    {
        Vibration = toggle.isOn;
        //JsonConverter.SaveSettings();
    }
    public static void VibrationActive(bool value)
    {
        Vibration = value;
        //JsonConverter.SaveSettings();
    }
    public static void GraphicQuality(Slider slider)
    {
        Graphic = (int)(slider.value);
        //JsonConverter.SaveSettings();
    }
}
