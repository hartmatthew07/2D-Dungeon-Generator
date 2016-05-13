using UnityEngine;
using System.Collections;


//This Tester Class shows that we don't even need to have a reference 
//to Audio Manager Attached to this gameObject to make it work.
public class ManagerTester: MonoBehaviour 
{

	public AudioClip[] tracks;
	int currentTrack = 0;

	public AudioClip soundFX;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.A))
		{
			AudioManager.PlayMusicTrack(tracks[0]);	
		}

		if(Input.GetKeyDown(KeyCode.S))
		{
			AudioManager.PlayMusicTrack(tracks[1]);	
		}

		if(Input.GetKeyDown(KeyCode.D))
		{
			AudioManager.PlaySoundEffect(soundFX);	
		}

		if(Input.GetKeyDown(KeyCode.F))
		{
			AudioManager.StopMusicTrack();	
		}
	}
}
