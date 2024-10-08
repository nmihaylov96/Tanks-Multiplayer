using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public GameObject AudioPrefab;
    public static AudioManager Instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play3D(AudioClip clip, Vector3 position)
    {
        GameObject audioGameObject = Instantiate(AudioPrefab, position, Quaternion.identity);
        AudioSource source = audioGameObject.GetComponent<AudioSource>();

        source.clip = clip;
        source.Play();

        Destroy(audioGameObject, clip.length);
    }
}
