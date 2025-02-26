using UnityEngine;

public class ButtonMethodsForAudioTests : MonoBehaviour {

    public void PlaySoundtrack(AudioClip audioClip/*, AudioSource audioSource*/) {
        AudioManager.Instance.PlaySoundtrack(audioClip);
    }

    public void PlaySFX(AudioClip audioClip) {
        AudioManager.Instance.PlaySFX(audioClip);
    }

    public void PlayDialogue(AudioClip audioClip) {
        AudioManager.Instance.PlayDialogue(audioClip);
    }

    public void PlayLoopedSoundtrack(AudioClip audioClip) {
        AudioManager.Instance.PlayLoopedSoundtrack(audioClip);
    }

    //public void PlayLoopedSFX(AudioClip audioClip) {
    //    AudioManager.Instance.PlayLoopedSFX(audioClip);
    //}

    //public void PlayLoopedDialogue(AudioClip audioClip) {
    //    AudioManager.Instance.PlayLoopedDialogue(audioClip);
    //}

    //public void PlayFadedSoundtrack(AudioClip audioClip) {
    //    AudioManager.Instance.PlayFadedSoundtrack(audioClip);
    //}

    //public void PlaySFXVariablePitch(AudioClip audioClip) {
    //    AudioManager.Instance.PlaySFXVariablePitch(audioClip);
    //}

    public void StopAllSounds() {
        foreach (AudioSource audioSource in AudioManager.Instance.AudioSources) {
            if (audioSource.isActiveAndEnabled) {
                audioSource.Stop();
            }
        }
    }
}
