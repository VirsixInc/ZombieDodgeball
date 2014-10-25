using UnityEngine;
using System.Collections;

public class enemySpellScript : MonoBehaviour {
	public GameObject spell;
	public bool loaded = false;
	public bool wasParried = false;
	public GameObject target;
	public Vector3 targetOffset;
	public float projectileSpeed = 5f;
	public Stats playerHealth;
	// Use this for initialization
	void Start () {
		target = GameObject.Find("CameraMotor 2");
		spell = this.gameObject;
		playerHealth = target.GetComponent<Stats>();

	}
	
	// Update is called once per frame
	void Update () {
		targetOffset = new Vector3(target.transform.position.x, target.transform.position.y+1f, target.transform.position.z);
		if(loaded){
			transform.position = Vector3.MoveTowards(transform.position, targetOffset, projectileSpeed*Time.deltaTime);
			//iTween.MoveTo(spell, targetOffset, 1f);
		}
		if(transform.position == targetOffset){
			//doDamageToPlayer();
			reset();
		}
		transform.LookAt(target.transform);
	}

	public void loadAndShoot(GameObject shooter, GameObject target){
		if(!loaded){
			this.transform.position = new Vector3(shooter.transform.position.x, shooter.transform.position.y+1f, shooter.transform.position.z);
			loaded = true;
		}

	}

//	void doDamageToPlayer(){
//		playerHealth.ApplyDamage(1);
//		print (playerHealth.GetHealth());
//	}


	public void reset(){
			transform.position = new Vector3(0f,-10f, 0f);
			loaded = false;
	}
}
