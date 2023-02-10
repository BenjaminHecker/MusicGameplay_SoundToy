using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Agent;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        public AudioSource source;
        public float timer;
    }

    [SerializeField] private float cooldownTimer = 0.2f;

    [SerializeField] private List<Sound> cyanSounds = new List<Sound>();
    [SerializeField] private List<Sound> orangeSounds = new List<Sound>();
    [SerializeField] private List<Sound> purpleSounds = new List<Sound>();

    private void Awake()
    {
        foreach (Sound s in cyanSounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = s.clip;
            source.volume = s.volume;
            source.playOnAwake = false;
            s.source = source;
            s.timer = 0f;
        }

        foreach (Sound s in orangeSounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = s.clip;
            source.volume = s.volume;
            source.playOnAwake = false;
            s.source = source;
            s.timer = 0f;
        }

        foreach (Sound s in purpleSounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = s.clip;
            source.volume = s.volume;
            source.playOnAwake = false;
            s.source = source;
            s.timer = 0f;
        }
    }

    private void Update()
    {
        UpdateTimers();
    }

    private void UpdateTimers()
    {
        foreach (Sound s in cyanSounds)
            s.timer += Time.deltaTime;
    }

    public void PlayGroup(AgentType color)
    {
        Sound sound;

        if (color == AgentType.Cyan)
            sound = cyanSounds[Random.Range(0, cyanSounds.Count)];
        else if (color == AgentType.Orange)
            sound = orangeSounds[Random.Range(0, orangeSounds.Count)];
        else
            sound = purpleSounds[Random.Range(0, purpleSounds.Count)];

        if (sound.timer >= cooldownTimer)
        {
            sound.timer = 0f;
            sound.source.Play();
        }
    }
}
