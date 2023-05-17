using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// audio clip group
[CreateAssetMenu(fileName = "New Audio Clip Group", menuName = "Audio/Audio Clip Group")]
public class AudioClipGroup : ScriptableObject
{
    // audio clips
    [SerializeField] List<AudioClip> m_clips;

    // audio volume
    [SerializeField] float m_volume = 1f;

    // audio pitch
    [SerializeField] Vector2 m_pitchVariance;

    // time restriction
    [SerializeField] float m_minInterval = 0f;
    [SerializeField] public float m_lastPlayedTime = 0;

    private void OnEnable()
    {
        m_lastPlayedTime = 0;
    }

    public void PlayOneShot(AudioSource audioSource, float maxDistance = -1)
    {
        if (Time.time >= m_lastPlayedTime && (Time.time - m_lastPlayedTime) < m_minInterval) return;

        AudioClip clip = m_clips[Random.Range(0, m_clips.Count)];
        float volume = m_volume;
        audioSource.pitch = Random.Range(m_pitchVariance.x, m_pitchVariance.y);
        m_lastPlayedTime = Time.time;

        if (maxDistance < 0)
        {
            audioSource.PlayOneShot(clip); 
            return;
        }

        float distance = Vector2.Distance(audioSource.transform.position, GlobalGameData.playerGO.transform.position);
        audioSource.panStereo = 0;

        if (maxDistance > 0)
        {
            float ratio = (distance / maxDistance);
            volume = (1 - ratio) * m_volume;

            if (audioSource.transform.position.x > GlobalGameData.playerGO.transform.position.x)
            {
                // Sound source is on the right
                audioSource.panStereo = ratio;
            }
            else
            {
                // Sound source is on the left
                audioSource.panStereo = -ratio;
            }
        }
        else
        {
            volume = m_volume;
        }
        audioSource.PlayOneShot(clip, volume);
        // Debug.Log(clip + " played");
    }
}
