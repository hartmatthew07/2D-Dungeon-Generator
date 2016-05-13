using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
	private const float kMusicFadeTimeSeconds = 0.5f;

	#region Singleton

	private static AudioManager s_instance = null;

	public static AudioManager Instance
	{
		get { return s_instance; }
	}

	#endregion

	private void Awake()
	{
		s_instance = this;	    	
	}

	/// <summary>
	/// Plays an AudioClip at the specified volume after an optional delay.
	/// </summary>
	/// <param name="soundEffectAudioClip">The AudioClip to be played.</param>
	/// <param name="volume">The volume at which to play the AudioClip (optional)</param>
	/// <param name="delaySeconds">Wait this long before playing the AudioClip (optional)</param>
	public void PlaySoundEffect ( AudioClip soundEffectAudioClip, float volume = 1.0f, float delaySeconds = 0.0f )
	{
		/* YOUR SOLUTION HERE */
	}

	/// <summary>
	/// Plays an AudioClip as a music track.  If a music track is already playing, the current track will fade out while the new one fades in over kMusicFadeTimeSeconds.
	/// Special case: If AudioManager is already playing musicAudioClip, this method does nothing.
	/// </summary>
	/// <param name="musicAudioClip">The music AudioClip</param>
	/// <param name="volume">The volume at which to play the AudioClip (optional)</param>
	/// <param name="loop">Whether or not the AudioClip should loop after it finishes playing.  True by default.</param>
	public void PlayMusicTrack ( AudioClip musicAudioClip, float volume = 1.0f, bool loop = true )
	{
		/* YOUR SOLUTION HERE */
	}

	/// <summary>
	/// If a music track is playing, fade that track out over kMusicFadeTimeSeconds.
	/// </summary>
	public void StopMusicTrack()
	{
		/* YOUR SOLUTION HERE */
	}
}

