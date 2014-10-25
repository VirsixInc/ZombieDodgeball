using UnityEngine;
using System.Collections;

public class FogLerp : MonoBehaviour {

	public float m_lerpTime = 0.15f;
	public float m_targetFogDensity = 0.25f;

	private bool m_active = false;

	void OnTriggerEnter( Collider col ) {
		if( col.tag == "Player" )
			StartCoroutine( "LerpFog" );
	}

	IEnumerator LerpFog() {
		RenderSettings.fogColor = Color.black;
		RenderSettings.fog = true;
		float startFogDensity = RenderSettings.fogDensity;
		float timer = 0f;


		while( timer <= 1f ) {
			RenderSettings.fogDensity = Mathf.Lerp( startFogDensity, m_targetFogDensity, timer );

			timer += Time.deltaTime / m_lerpTime;
			yield return null;
		}
	}
}
