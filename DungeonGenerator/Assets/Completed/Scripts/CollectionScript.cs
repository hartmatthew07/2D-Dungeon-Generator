using UnityEngine;
using System.Collections;

public class Collected:MonoBehavior{
	
	void Start(){
		
	}
	
	void Update(){
		
	}
	
	void OnTriggerEnter2D(Collider2D target){
		if(target.gameObject.tag == "Player")
			Destroy (gameObject);
	}
	
}

