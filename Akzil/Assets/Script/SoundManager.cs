using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;

[System.Serializable]
public struct Clip
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private Dictionary<string, AudioClip> clipDic = new Dictionary<string, AudioClip>();
    [SerializeField] private List<Clip> clips = new List<Clip>();

    [SerializeField] private GameObject playPrefab;

    private void Awake() 
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        foreach (var item in clips)
        {
            clipDic.Add(item.name, item.clip);
        }
        Play("BGM", true);
    }

    public void Play(string name, bool loof)
    {
        var source = Instantiate(playPrefab, transform.position, Quaternion.identity).GetComponent<AudioSource>();
        source.clip = clipDic[name];
        source.loop = loof;
        source.Play();
    }
}
