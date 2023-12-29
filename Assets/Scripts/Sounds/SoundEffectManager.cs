using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class SoundEffectManager : SingletonMonoBehaviour<SoundEffectManager>
{
    public int soundsVolume = 8;

    void Start()
    {
        SetSoundsVolume(soundsVolume);
    }

    /// <summary>
    /// Play the sound effect
    /// </summary>
    /// <param name="soundEffect"></param>
    public void PlaySoundEffect(SoundEffectSO soundEffect)
    {
        // Play sound using a sound game object and component from the object pool
        SoundEffect sound = (SoundEffect)PoolManager.Instance.ReuseComponent(soundEffect.soundPrefab, Vector3.zero, Quaternion.identity);
        sound.SetSound(soundEffect);
        sound.gameObject.SetActive(true);
        StartCoroutine(DisableSound(sound, soundEffect.soundEffectClip.length));
    }

    /// <summary>
    /// Disable sound effect object after it has played thus returing it to the object pool
    /// </summary>
    /// <param name="sound"></param>
    /// <param name="SoundDuration"></param>
    /// <returns></returns>
    public IEnumerator DisableSound(SoundEffect sound, float SoundDuration)
    {
        yield return new WaitForSeconds(SoundDuration);
        sound.gameObject.SetActive(false);
    }

    /// <summary>
    /// Set sounds volume
    /// </summary>
    /// <param name="soundsVolume"></param>
    private void SetSoundsVolume(int soundsVolume)
    {
        float muteDecibels = -80f;

        if (soundsVolume == 0)
        {
            GameResources.Instance.soundsMasterMixerGroup.audioMixer.SetFloat("soundsVolume", muteDecibels);
        }

        else
        {
            GameResources.Instance.soundsMasterMixerGroup.audioMixer.SetFloat("soundsVolume", HelperUtilities.LinearToDecibels(soundsVolume));
        }

    }
}