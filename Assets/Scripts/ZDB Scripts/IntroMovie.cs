using UnityEngine;
using System.Collections;

public class IntroMovie : MonoBehaviour 
{
	public bool loop;

	void Start()
	{
		((MovieTexture)GetComponent<Renderer>().material.mainTexture).loop = loop;
		((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play();
	}
	void Update () 
	{

	}
}
