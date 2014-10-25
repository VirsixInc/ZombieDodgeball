using UnityEngine;
using System.Collections;

public class ShotManager : MonoBehaviour {

	private static ShotManager _instance;

	public int m_damage = 1;
	public GameObject particle;

	private AudioSource m_audio;
	public GameObject mainCamForOSC;
    public GameObject gunFeedback;
	
	#region Singleton Initialization
	public static ShotManager instance {
		get {
			if(_instance == null)
				_instance = GameObject.FindObjectOfType<ShotManager>();

			return _instance;
		}
	}

	void Awake() {
		if(_instance == null) {
			//If i am the fist instance, make me the first Singleton
			_instance = this;
		} else {
			//If a Singleton already exists and you find another reference in scene, destroy it
			if(_instance != this)
				Destroy(gameObject);
		}
	}
	#endregion

	void Start () {
		m_audio = ManagerScript.instance.GetComponent<AudioSource>();
		mainCamForOSC = GameObject.Find("0");
	}
	
	void Update () {
		if(GameManager.instance.CurrentMode == (int)GameManager.GameMode.Play) {
			if(Input.GetMouseButtonDown(0)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				if(Network.isServer){
					Shoot(ray.origin, ray.direction);
				}
				else{ 
					networkView.RPC("Shoot", RPCMode.Server, ray.origin, ray.direction);
				}
			}

		}
	}

	public void NetworkShoot(float x, float y) {
		if(GameManager.instance.CurrentMode == (int)GameManager.GameMode.Play) {
			Ray ray = Camera.main.ViewportPointToRay(new Vector3(x, y, 0.0f));

			if(Network.isServer) 
				Shoot(ray.origin, ray.direction);
			else
				networkView.RPC("Shoot", RPCMode.Server, ray.origin, ray.direction);
		}
	}

	[RPC]
	public void Shoot(Vector3 origin, Vector3 direction) {
		Ray ray = new Ray(origin, direction);
		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit, 100f)){
			Debug.DrawLine(ray.origin, hit.point, Color.red, 10f);
            //Network.Instantiate(gunFeedback, hit.point, Quaternion.LookRotation(hit.normal), 0);

			if(hit.transform.tag == "Enemy") {
				m_audio.Play();
				Network.Instantiate(particle, hit.point, Quaternion.LookRotation(hit.normal), 0);
				hit.transform.SendMessage("TakeDamage", m_damage);
				
			}
			if(hit.transform.tag == "SpellCast") {
				m_audio.Play();
				Network.Instantiate(particle, hit.point, Quaternion.LookRotation(hit.normal), 0);
				hit.transform.gameObject.GetComponent<enemySpellScript>().reset();
				//hit.transform.SendMessage("TakeDamage", m_damage);

			}
		}
	}
}
