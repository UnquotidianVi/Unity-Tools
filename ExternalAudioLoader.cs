using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

static class ExternalAudioLoader
{
    /// <summary>
    /// Coroutine which loads a audio clip from an external file and plays it with given audio source.
    /// </summary>
    /// <param name="audioSource">Audio source to play the external audio with.</param>
    /// <param name="filePath">Audio file path.</param>
    /// <returns></returns>
    public static IEnumerator PlayExternalAudioClip(AudioSource audioSource, string filePath)
    {
        if (File.Exists(filePath))
        {
            //Change the audio type if you want to load anything else except .ogg
            //TODO: Check for possible file endings.
            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.OGGVORBIS);
            yield return request.SendWebRequest();

            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);

            if(audioClip != null)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
                audioSource.loop = false;
            }
        }
        else
        {
            Debug.LogError("Audio File does not exist.");
        }
    }
}
