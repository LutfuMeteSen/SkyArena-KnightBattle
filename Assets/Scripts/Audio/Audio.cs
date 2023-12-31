using UnityEngine;

public class Audio : MonoBehaviour
{
    public static Audio instance;
    //Ses ve Müzik İçin game objectler
    public GameObject music;
    public GameObject sfx;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Slidera bağlı fonksiyon müzik için
    public void MusicVolumeChange(float value)
    {
        music.GetComponent<AudioSource>().volume = value;
    }
    //Slidera bağlı fonkisyon ses efekti için
    public void SfxVolumeChange(float value)
    {
        sfx.GetComponent<AudioSource>().volume = value;
    }
    //Ses Efektini oynatma fonksiyonu
    public void playAudioEffect()
    {
        sfx.GetComponent<AudioSource>().Play();
    }
}
