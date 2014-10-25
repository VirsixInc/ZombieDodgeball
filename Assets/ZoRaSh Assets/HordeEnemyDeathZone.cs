using UnityEngine;
using System.Collections;

public class HordeEnemyDeathZone : MonoBehaviour {

	void OnTriggerEnter( Collider col ) {
		if( col.tag == "Enemy" ) {
			col.SendMessageUpwards ( "Reset", SendMessageOptions.DontRequireReceiver );
		}
	}
}
