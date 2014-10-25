using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SphereCollider))]
public class ExitTrigger : MonoBehaviour {

	[System.NonSerialized]
	public bool m_hasEntered = false;

	void OnTriggerEnter( Collider col ) {
		if( col.tag == "Player" )
			m_hasEntered = true;
	}
}
