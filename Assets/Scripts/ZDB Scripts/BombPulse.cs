using UnityEngine;
using System.Collections;

public class BombPulse : MonoBehaviour 
{
	public Material baseMat;
	public Material pulseMat;
	
	public float pulseSpeed;
	
	Renderer rend;
	float timer = 0.0f;
	public bool pulsing = false;
	
	void Start()
	{
		rend = gameObject.GetComponent<Renderer>();
		rend.material = baseMat;
	}
	
	void Update () 
	{
		if( pulsing )
		{
			timer += Time.deltaTime;
			float lerp = Mathf.PingPong(timer, pulseSpeed) / pulseSpeed;
			rend.material.Lerp(baseMat, pulseMat, lerp);
		}
	}
	
	public void Pulse( bool pulse )
	{
		pulsing = pulse;
		timer = 0;
	}
}
