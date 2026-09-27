
using UnityEngine;

public class MenuMusicPlayer : MonoBehaviour
{
    private void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();

        if (sources.Length < 2 || sources[0].clip == null || sources[1].clip == null)
        {
            Debug.LogError("MenuMusic : intro ou loop manquante.");
            return;
        }

        AudioSource intro = sources[0];
        AudioSource loop = sources[1];

        intro.loop = false;
        loop.loop = true;

        double startTime = AudioSettings.dspTime + 0.1;

        intro.PlayScheduled(startTime);
        loop.PlayScheduled(startTime + intro.clip.length);
    }
}
