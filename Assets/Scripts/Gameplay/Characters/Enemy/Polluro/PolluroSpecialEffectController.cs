using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolluroSpecialEffectController : MonoBehaviour
{
    public GameObject footParticle;
    public GameObject armMark;

    private Dictionary<string, GameObject> specialEffects = new Dictionary<string, GameObject>();
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AudioManager.Instance.addSoundAudioSource(audioSource);
    }

    public void AddToFootParticle(string address, GameObject gameObject)
    {
        specialEffects[address] = gameObject;
        gameObject.transform.SetParent(footParticle.transform, false);
    }

    public GameObject GetArmMark()
    {
        return armMark;
    }

    public void Remove(string address)
    {
        if (specialEffects.ContainsKey(address))
        {
            specialEffects.Remove(address);
        }
    }

    public void PlayOneShot(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void PlayOneShot(AudioClip audioClip, float volumeScale)
    {
        audioSource.PlayOneShot(audioClip, volumeScale);
    }

    void OnDestroy()
    {
        AudioManager.Instance.removeSoundAudioSource(audioSource);
    }
}
