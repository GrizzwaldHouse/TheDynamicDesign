using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider volume;
    // Start is called before the first frame update
    private void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            load();
        }

        else
        {
            load();
        }
    }


   public void changeVolume()
    {
        AudioListener.volume = volume.value;
        save();
    }

    private void load()
    {
        volume.value = PlayerPrefs.GetFloat("musicVolume");

    }
    private void save()
    {
        PlayerPrefs.SetFloat("musicVolume", volume.value);
    }
}
