//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Audio;

//public class AudioManager : MonoBehaviour
//{
//    public List<Sound> sounds = new List<Sound>();
//    public float maxVolume=1f;
//    public float curVolume;
//    public float normalPitch;

//    public static AudioManager instance;

//    private void Awake()
//    {
//        DontDestroyOnLoad(this);
//        if (!instance)
//        {
//            instance = this;
//        }
//        foreach (Sound s in sounds)
//        {
//            s.source = gameObject.AddComponent<AudioSource>();
//            s.source.clip = s.audioClip;
//            s.source.volume = s.volume/10;
//            s.source.pitch = s.pitch;
//            s.source.mute = s.mute;
//            s.source.loop = s.loop;
//        }
//    }
//    private void Start()
//    {
//        if (PlayerPrefs.HasKey("curVolume")) curVolume = PlayerPrefs.GetFloat("curVolume");
//        else curVolume = 5f;
//    }


//    public void Play(string _name)
//    {
//        Sound sound = sounds.Find(s=>s.name ==_name);
//        if (sound!=null)
//        {
//            sound.source.Play();
//        }
//    }
//    public void Stop(string _name)
//    {
//        Sound sound = sounds.Find(s=>s.name ==_name);
//        if (sound!=null)
//        {
//            sound.source.Stop();
//        }
//    }
//    public void StopMusic()
//    {
//        foreach (Sound s in sounds)
//        {
//            if(s.type==TypeAudio.Music)
//                s.source.Stop();
//        }
//    }  
//    public void StopSFX()
//    {
//        foreach (Sound s in sounds)
//        {
//            if(s.type==TypeAudio.SFX)
//                s.source.Stop();
//        }
//    }
//    public void StopAll()
//    {
//        foreach(Sound s in sounds)
//        {
//            s.source.Stop();
//        }
//    }

//}

//[System.Serializable]
//public class Sound
//{
//    public string name;
//    public TypeAudio type;
//    public AudioClip audioClip;
//    [Range(0, 10)]
//    public float volume;
//    [Range(0,3)]
//    public float pitch;
//    public bool mute;
//    public bool loop;

//    [HideInInspector]
//    public AudioSource source;
//}
//public enum TypeAudio 
//{
//    Music,
//    SFX
//}
