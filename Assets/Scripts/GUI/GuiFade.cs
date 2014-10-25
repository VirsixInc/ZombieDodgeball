using UnityEngine;
using System.Collections;

public class GuiFade : MonoBehaviour {

	public float m_startAlpha = 0f;
	public float m_endAlpha = 1f;
	public float m_fadeTime = 1f;

	bool m_active;
	
	// Update is called once per frame
	void Update () {
		if( GameManager.instance.CurrentMode == (int)GameManager.GameMode.Title ) {
			if( !m_active ) {
				StartCoroutine( "FadeIn" );
				m_active = true;
			}
		} else if ( m_active ) {
			StartCoroutine( "FadeOut" );
			m_active = false;
		}
	}

	IEnumerator FadeIn() {
		float timer = 0f;
		Color t_textureColor;

		while( timer <= 1f ) {
			t_textureColor = guiTexture.color;
			t_textureColor.a = Mathf.Lerp( m_startAlpha, m_endAlpha, timer);
			guiTexture.color = t_textureColor;

			timer += Time.deltaTime / m_fadeTime;
			yield return null;
		}
	}

	IEnumerator FadeOut() {
		float timer = 0f;
		Color t_textureColor;
		
		while( timer <= 1f ) {
			t_textureColor = guiTexture.color;
			t_textureColor.a = Mathf.Lerp( m_endAlpha, m_startAlpha, timer);
			guiTexture.color = t_textureColor;
			
			timer += Time.deltaTime / m_fadeTime;
			yield return null;
		}

		gameObject.SetActive( false );
	}
}
