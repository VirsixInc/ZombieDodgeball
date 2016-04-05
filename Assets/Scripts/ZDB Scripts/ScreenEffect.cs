using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenEffect : MonoBehaviour 
{
	public bool isImage;

	public float onScreenTime;
	public float fadeTime;
	
	bool activated = false;
	float timer = 0.0f;
	
	void Update () 
	{
		if( activated )
		{
			if( timer <= 0.0f )
			{
				activated = false;
				gameObject.SetActive(false);
			}
			else if( timer <= fadeTime )
			{
				ChangeAlpha( timer / fadeTime );
			}
			timer -= Time.deltaTime;
		}
	}
	
	public void Activate()
	{
		Debug.Log("activating");
		gameObject.SetActive(true);
		activated = true;
		timer = onScreenTime;
		ChangeAlpha(1f);
	}
	
	void ChangeAlpha( float alpha )
	{
		Color color;
		if( isImage )
		{
			color = gameObject.GetComponent<Image>().color;
			color.a = alpha;
			gameObject.GetComponent<Image>().color = color;
		}
		else
		{
			color = gameObject.GetComponent<SpriteRenderer>().color;
			color.a = alpha;
			gameObject.GetComponent<SpriteRenderer>().color = color;
		}
	}
	
}
