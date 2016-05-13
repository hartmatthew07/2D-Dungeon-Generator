using UnityEngine;
using System.Collections;

[RequireComponent( typeof(AudioSource), typeof(AudioSource) )]
public class AudioManager : MonoBehaviour
{
	private const float kMusicFadeTimeSeconds = 0.5f;
	private AudioSource[] MusicSources = new AudioSource[2];

	#region Singleton

	private static AudioManager s_instance = null;

	public static AudioManager Instance
	{
		get { return s_instance; }
	}

	#endregion

	private void Awake()
	{
		//Make sure we only have two audio sources on the Audio Manager;
		RequireTwoSources();

		MusicSources = GetComponents<AudioSource>();
		s_instance = this;	    	
	}

	/// <summary>
	/// Plays an AudioClip at the specified volume after an optional delay.
	/// </summary>
	/// <param name="soundEffectAudioClip">The AudioClip to be played.</param>
	/// <param name="volume">The volume at which to play the AudioClip (optional)</param>
	/// <param name="delaySeconds">Wait this long before playing the AudioClip (optional)</param>
	public static void PlaySoundEffect ( AudioClip soundEffectAudioClip, float volume = 1.0f, float delaySeconds = 0.0f )
	{
		AudioSource newAudioSource = Instance.gameObject.AddComponent<AudioSource>();
		newAudioSource.clip = soundEffectAudioClip;

		Instance.StartCoroutine(Instance.PlayAfterDelay(newAudioSource));
	}

	/// <summary>
	/// Plays an AudioClip as a music track.  If a music track is already playing, the current track will fade out while the new one fades in over kMusicFadeTimeSeconds.
	/// Special case: If AudioManager is already playing musicAudioClip, this method does nothing.
	/// </summary>
	/// <param name="musicAudioClip">The music AudioClip</param>
	/// <param name="volume">The volume at which to play the AudioClip (optional)</param>
	/// <param name="loop">Whether or not the AudioClip should loop after it finishes playing.  True by default.</param>
	public static void PlayMusicTrack ( AudioClip musicAudioClip, float volume = 1.0f, bool loop = true, float fadeTime = 1.0f )
	{

		if(!compareClips(Instance.MusicSources[0].clip, musicAudioClip))
		{
			//Swap the array elements so that the new source replaces the source not in use.
	    	Instance.MusicSources = swap(Instance.MusicSources[0], Instance.MusicSources[1]);

			//stop all current couroutines, they may interfere with a new crossfade.
			Instance.StopAllCoroutines();

			//Setting up the new source for the crossfade
			Instance.MusicSources[0].volume = 0.0f;
			Instance.MusicSources[0].clip = musicAudioClip;
			Instance.MusicSources[0].loop = loop;
			Instance.MusicSources[0].Play();

			Instance.crossfade(Instance.MusicSources[0],volume,fadeTime);
		}
		else
		{
			//do nothing
		}
	}

	/// <summary>
	/// If a music track is playing, fade that track out over kMusicFadeTimeSeconds.
	/// </summary>
	public static void StopMusicTrack()
	{
		//stop all current couroutines, they may interfere fading out the sources.
		Instance.StopAllCoroutines();

		//Fade both sources out over kMusicFadeTimeSeconds so there is no sound.
		Instance.StartCoroutine(Instance.FadeMusicOut(Instance.MusicSources[0],kMusicFadeTimeSeconds));
		Instance.StartCoroutine(Instance.FadeMusicOut(Instance.MusicSources[1],kMusicFadeTimeSeconds));
	}

	//Handles crossfading between the two sources
  	void crossfade(AudioSource newSource, float volume = 1.0f, float fadeTime = 1.0f)
	{
		//Fade new source in.
		Instance.StartCoroutine(Instance.FadeMusicIn(newSource,fadeTime,volume));

		//Fade old source out.
		Instance.StartCoroutine(FadeMusicOut(Instance.MusicSources[1]));
	}
		

	IEnumerator FadeMusicIn(AudioSource source, float fadeTime = 1.0f, float volume = 1.0f)
	{
		//Smoothly increase the volume to the volume variable.
		//Lerp never quite makes it to the value so we compensate.
		while(source.volume < volume - .001f)
		{
			source.volume = Mathf.Lerp(source.volume,volume,Time.deltaTime/fadeTime);
			yield return null;
		}

		//Make sure that if we didn't make it to the full volume in the lerp we set it here
		source.volume = volume;

	}

	IEnumerator FadeMusicOut(AudioSource source, float fadeTime = 1.0f, float volume = 0.02f)
	{
		yield return new WaitForEndOfFrame();
		//Smoothly decrease the volume to the volume variable.
		//Lerp never quite makes it to the value so we compensate.
		while(source.volume > volume + .001f)
		{
			source.volume = Mathf.Lerp(source.volume,volume,Time.deltaTime/fadeTime);
			yield return null;
		}

		//Make sure that if we didn't make it to the no volume in the lerp we set it here
		source.volume = 0;

	}

	IEnumerator PlayAfterDelay(AudioSource source, float volume = 1.0f, float delay = 0.0f)
	{
		yield return new WaitForSeconds(delay);
		source.PlayOneShot(source.clip,volume);

		//wait the duration of the clip then clean up its source.
		yield return new WaitForSeconds(source.clip.length);
		Destroy(source);
	}
		
	//Swap Array elements around 
	public static AudioSource[] swap(AudioSource sourceA, AudioSource sourceB)
	{
		AudioSource sourceC = sourceA;
		sourceA = sourceB;
	    sourceB = sourceC;

		return new AudioSource[] {sourceA,sourceB}; 
	}

	//Unity doesn't really give a good way to require two components 
	//this is how I made sure there were only two audio sources on the AudioManager 
	private void RequireTwoSources()
	{
		while(GetComponents<AudioSource>().Length < 2)
		{
			gameObject.AddComponent<AudioSource>();
		}

		while(GetComponents<AudioSource>().Length > 2)
		{
			Destroy(GetComponent<AudioSource>());
		}
	}

	//Get the data from each clip and 
	private static bool compareClips(AudioClip clip1,AudioClip clip2)
	{
		bool sameClip = false;

		//check to make sure we have at least one clip already playing.
		if(clip1 == null)
		{
			return sameClip;
		}

		//Set up the arrays for our clip data and the get that data.
		float[] firstClipSamples = new float[clip1.samples * clip1.channels];
		float[] secondClipSamples = new float[clip2.samples * clip2.channels];
		
		clip1.GetData(firstClipSamples, 0);
		clip2.GetData(secondClipSamples, 0);

		//this is done to ensure that we don't go out of bounds in either array;
		if(firstClipSamples.Length != secondClipSamples.Length)
		{
			return sameClip;
		}

		//compare the two arrays index by index for equality
		for(int i = 0; i < firstClipSamples.Length; i ++)
		{
			if(firstClipSamples[i] == secondClipSamples[i])
			{
				sameClip = true;
			}
			else
				sameClip = false;
		}
		return sameClip;
	}
}

