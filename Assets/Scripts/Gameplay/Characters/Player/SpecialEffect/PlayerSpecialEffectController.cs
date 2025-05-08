using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

// 管理Player的特效，及特效资源加载
public class PlayerSpecialEffectController : MonoBehaviour
{
    public GameObject footParticle;

    public GameObject armMark;

    public FlyParticleController flyParticle;

    private Dictionary<string, GameObject> specialEffects = new Dictionary<string, GameObject>();
    private AudioSource audioSource;

    private string hitSoundAddress = "Audio/SoundEffect/PlayerSound/Movement/hitSound.wav";
    private AudioClip hitSound;

    // Start is called before the first frame update
    void Start()
    {
        this.audioSource = GetComponent<AudioSource>();
        AudioManager.Instance.addSoundAudioSource(audioSource);

        AddressableAssetManager.Instance.loadAsset(hitSoundAddress, obj =>
        {
            hitSound = (AudioClip)obj;
        });
    }

    public void startFly()
    {
        flyParticle.enableEmission();
    }

    public void stopFly()
    {
        flyParticle.disableEmission();
    }

    public void playHitSound()
    {
        if(hitSound != null)
        {
            playOneShot(hitSound);
        }
    }

    public void addToFootParticle(string address, GameObject gameObject)
    {
        specialEffects[address] = gameObject;
        gameObject.transform.SetParent(footParticle.transform, false);
    }

    public GameObject getArmMark()
    {
        return armMark;
    }

    public void remove(string address)
    {
        if (specialEffects.ContainsKey(address))
        {
            specialEffects.Remove(address);
        }
    }

    public void playOneShot(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void playOneShot(AudioClip audioClip, float volumeScale)
    {
        audioSource.PlayOneShot(audioClip, volumeScale);
    }

    void OnDestroy()
    {
        AddressableAssetManager.Instance.releaseAsset(hitSoundAddress);
        AudioManager.Instance.removeSoundAudioSource(audioSource);
    }
}
