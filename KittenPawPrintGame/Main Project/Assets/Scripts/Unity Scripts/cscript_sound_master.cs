using UnityEngine;
using System.Collections;

public class cscript_sound_master : MonoBehaviour 
{
	public AudioClip correctAnswer;
	public AudioClip incorrectAnswer;
	public AudioClip navigation;
	
	public void PlaySound(AudioClip s)
	{
		gameObject.GetComponent<AudioSource>().clip = s;
		gameObject.GetComponent<AudioSource>().Play ();
	}
}