using UnityEngine;

public class MusicVolumeResetOnMenu : MonoBehaviour
{
    void Start()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.FadeToVolume(1f, 1f);
        }
    }
}