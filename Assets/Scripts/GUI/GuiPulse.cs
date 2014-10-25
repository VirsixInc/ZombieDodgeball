using UnityEngine;
using System.Collections;

public class GuiPulse : MonoBehaviour {

	public float m_pulseSpeed = 5f;

	private bool m_active;

	void Update() {
		if( GameManager.instance.CurrentMode == (int)GameManager.GameMode.Title ) {
			if( !m_active ) {
				StartCoroutine( "LerpAlpha" );
				m_active = true;
			}
		}
	}

	IEnumerator LerpAlpha() {
		Color t_textureColor;
		while( true ) {
			if( GameManager.instance.CurrentMode != (int)GameManager.GameMode.Title )
				break;

			t_textureColor = guiTexture.color;
			t_textureColor.a = Mathf.PingPong( Time.time * m_pulseSpeed, 1f );
			guiTexture.color = t_textureColor;

			yield return null;
		}

		gameObject.SetActive( false );
	}
}
