using UnityEngine;
using System.Collections;

public class HordeEnemyDeathZone : MonoBehaviour {

	void OnTriggerEnter( Collider col ) {
		if( col.tag == "Enemy" ) {
			col.SendMessageUpwards ( "Reset", SendMessageOptions.DontRequireReceiver );

			if( GameManager.instance.isGamePlaying )
				GameManager.instance.ReduceLives( 1 );
		}
	}
}
