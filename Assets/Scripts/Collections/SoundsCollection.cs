using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsCollection : MonoBehaviour
{
    public static SoundsCollection instance;

    [Header("Collection of items")]
    public List<Audio> collectionItems = new List<Audio>();
    public GameObject prefab;

    private void Awake()
    {
        instance = this;
    }

    public void CreateAudio(string id, Vector3 position)
    {
        AudioSource obj = Instantiate(prefab).GetComponent<AudioSource>();
        obj.clip = GetAudio(id);
        obj.Play();

        obj.transform.position = position;
    }
    public AudioClip GetAudio(string id)
    {
        foreach (var item in collectionItems)
        {
            if(item.id == id)
            {
                int rdm = Random.Range(0, item.audioClip.Count);
                return item.audioClip[rdm];
            }
        }
        return null;
    }
}

[System.Serializable]
public class Audio
{
    public string id;
    public List<AudioClip> audioClip;
}
