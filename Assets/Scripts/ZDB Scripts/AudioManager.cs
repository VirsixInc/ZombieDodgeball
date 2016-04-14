using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour 
{
	//prefabs
	public AudioSource zombieSoundPrefab;
	public AudioSource werewolfSoundPrefab;

	//sounds
	public List<AudioSource> zombieSounds;
	public List<AudioSource> werewolfSounds;
	
	public AudioSource bomb;
	public AudioSource heartContainerShatter;
	public AudioSource zombieAttack;
	
	public void PlaySound( string audio )
	{
		switch( audio )
		{
		case "ZombieHit":
			foreach( AudioSource source in zombieSounds )
			{
				if( !source.isPlaying )
				{
					source.Play();
					return;
				}
			}
			AudioSource newAudio = ( (GameObject)Instantiate( zombieSoundPrefab, transform.position, Quaternion.identity ) ).GetComponent<AudioSource>();
			newAudio.Play();
			zombieSounds.Add( newAudio );
			break;
		case "WerewolfHit":
			foreach( AudioSource source in werewolfSounds )
			{
				if( !source.isPlaying )
				{
					source.Play();
					return;
				}
			}
			AudioSource newAudio1 = ( (GameObject)Instantiate( werewolfSoundPrefab, transform.position, Quaternion.identity ) ).GetComponent<AudioSource>();
			newAudio1.Play();
			zombieSounds.Add( newAudio1 );
			break;
		case "Bomb":
			bomb.Play();
			break;
		case "HeartContainerShatter":
			heartContainerShatter.Play();
			break;
		case "ZombieAttack":
			zombieAttack.Play();
			break;
		}
	}
	
	public string GetHitSoundName( string objectName )
	{
		if( objectName.Contains("Zombie") )
			return "ZombieHit";
		else if( objectName.Contains("Werewolf") )
			return "WerewolfHit";
		return "";
	}
}
