using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class AudioManager : MonoBehaviour
{

    [Header("Ambience")]
    [SerializeField] private AudioClip[] ambience;
    [SerializeField] private GameObject ambienceReceiver;
    [SerializeField] private GameObject soundSource2D;

    [Header("Sounds")]
    [SerializeField] private List<Sound> soundList;

    private List<AudioSource> sourceList;
    private Coroutine ambiencethread;

    private void Start()
    {
        sourceList= new List<AudioSource>();
        ambiencethread = StartCoroutine(LoopAmbience(-1));

        EventBroadcaster.Instance.AddObserver(EventNames.GAME_LOOP_EVENTS.ON_GAME_OVER, (Parameters p) => StopAllSounds());
        EventBroadcaster.Instance.AddObserver(EventNames.GAME_LOOP_EVENTS.ON_TIMES_UP, () => {
            StopAllSounds();
            PlaySound2D("Time's Up");
        });
    }

    public void PlaySound(GameObject origin, string name, Action onFinished = null, bool isLooped = false)
    {
        AudioSource source = origin.GetComponent<AudioSource>();
        AudioClip clip = getClip(name);

        if (clip != null)
        {
            source.loop = isLooped;
            source.clip = clip;
            source.Play();

            if(onFinished != null)
                StartCoroutine(doOnFinishedClip(source, onFinished));
        }
            
    }

    public void PlaySound2D(string name, Action onFinished = null)
    {
        AudioSource source = soundSource2D.GetComponent<AudioSource>();
        AudioClip clip = getClip(name);

        if(clip != null)
        {
            source.clip = clip;
            source.Play();

            if (onFinished != null)
                StartCoroutine(doOnFinishedClip(source, onFinished));
        }
     

    }

    public void RegisterAudioSource(AudioSource source)
    {
        sourceList.Add(source);
    }

    public void StopAllSounds()
    {
        print("stopped");
        ambienceReceiver.GetComponent<AudioSource>().Stop();
        StopCoroutine(ambiencethread);
        foreach(var s in sourceList)
            StopPlaying(s);
    }

    public void StopPlaying(GameObject origin)
    {
        AudioSource source = origin.GetComponent<AudioSource>();
        StopPlaying(source);
    }

    private void StopPlaying(AudioSource source)
    {
        source.loop = false;
        source.Stop();
    }

    private AudioClip getClip(string name)
    {
        foreach(var c in soundList)
        {
            if (name == c.name)
                return c.clip;
        }
        return null;
    }

    private IEnumerator doOnFinishedClip(AudioSource source, Action action)
    {
        while (source.isPlaying)
            yield return null;

        action?.Invoke();
    }

    
    private IEnumerator LoopAmbience(int indexExcluded)
    {

        IEnumerator doOnFinish(AudioClip clip, IEnumerator action)
        {
            AudioSource source = ambienceReceiver.GetComponent<AudioSource>();
            source.clip = clip;
            source.Play();

            while (source.isPlaying)
                yield return null;

            yield return new WaitForSeconds(1f);

            yield return action;
        }

        if (ambience.Length == 1)
        {
            yield return doOnFinish(ambience[0], LoopAmbience(-1));
            yield break;
        }

        print("starting");
          

        List<AudioClip> playlist = new List<AudioClip>();

        if(indexExcluded >= 0)
        {
            for(int i =0; i < ambience.Length; i++)
            {
                if(i != indexExcluded)
                    playlist.Add(ambience[i]);
            }
        }else playlist.AddRange(ambience);

        int toPlay = UnityEngine.Random.Range(0, playlist.Count);

        yield return doOnFinish(playlist[toPlay], LoopAmbience(toPlay));
        
    }


    #region
    
        public static AudioManager instance =  null;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                print("Destroying");
                Destroy(gameObject);
            }
        }
 
    #endregion

}
