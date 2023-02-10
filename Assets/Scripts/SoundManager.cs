using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum AgentType { Cyan, Orange, Purple }

    [System.Serializable]
    class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    [SerializeField] private List<Sound> cyanSounds = new List<Sound>();
    [SerializeField] private List<Sound> orangeSounds = new List<Sound>();
    [SerializeField] private List<Sound> purpleSounds = new List<Sound>();

    private List<AudioSource> cyanSources = new List<AudioSource>();
    private List<AudioSource> orangeSources = new List<AudioSource>();
    private List<AudioSource> purpleSources = new List<AudioSource>();

    private void Awake()
    {
        foreach (Sound s in cyanSounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = s.clip;
            source.volume = s.volume;
            source.playOnAwake = false;

            cyanSources.Add(source);
        }

        foreach (Sound s in orangeSounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = s.clip;
            source.volume = s.volume;
            source.playOnAwake = false;

            orangeSources.Add(source);
        }

        foreach (Sound s in purpleSounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = s.clip;
            source.volume = s.volume;
            source.playOnAwake = false;

            purpleSources.Add(source);
        }
    }

    public void PlayRandom(AgentType color)
    {
        if (color == AgentType.Cyan)
            cyanSources[Random.Range(0, cyanSources.Count)].Play();
        else if (color == AgentType.Orange)
            orangeSources[Random.Range(0, orangeSources.Count)].Play();
        else if (color == AgentType.Purple)
            purpleSources[Random.Range(0, purpleSources.Count)].Play();
    }
}
