using UnityEngine;
using System.Collections;

public class Door:MonoBehaviour{

	void Start(){
	
	}
	
	void Update(){
	
	}
	
	void OnTriggerEnter2D(Collider2D target){
		if(target.gameObject.tag == "PlayerWithKey")
		Destroy(gameObject);
		//else null;//Have box collider in Unity
	}
}