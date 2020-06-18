using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script has a public interface to play sounds via spawning sound particles (allows to play multiple sounds at once)
/// </summary>
public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_AudioClip = null;

    [SerializeField]
    private GameObject m_PlayedNotePrefab = null;


    public void Play()
    {
        //Spawn a prefab
        GameObject playedNote = Instantiate(m_PlayedNotePrefab);
        playedNote.transform.SetParent(transform);

        //Get AudioSource component
        AudioSource audioSource = playedNote.GetComponent<AudioSource>();

        //Set up and play
        audioSource.clip = m_AudioClip;
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();
    }
}
