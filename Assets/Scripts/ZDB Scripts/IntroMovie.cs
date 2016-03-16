using UnityEngine;
using System.Collections;

public class IntroMovie : MonoBehaviour 
{
	void Update () 
	{
		if( !((MovieTexture)GetComponent<Renderer>().material.mainTexture).isPlaying )
			((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play();
	}
}
