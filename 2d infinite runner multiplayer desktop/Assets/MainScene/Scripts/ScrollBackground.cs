using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour{


	public GameObject prev;
	public GameObject next;

	public float scrollSpeed = 5;
	
	void Start (){
		initPrevPositon = prev.transform.localPosition;
		initNextPositon = next.transform.localPosition;

		
	}
	
	void Update (){
		if (next.transform.localPosition.x <= initPrevPositon.x){
			//move prev to next pos
			prev.transform.localPosition = initNextPositon;
			next.transform.localPosition = initPrevPositon;
			//swap next and prev
			GameObject temp = next;
			next = prev;
			prev = temp;
		}
		
		Vector3 move = new Vector3(scrollSpeed * Time.deltaTime,0,0);
		prev.transform.localPosition -=move;
		next.transform.localPosition -=move;
	
		
	}
		
	private Vector3 initPrevPositon;
	private Vector3 initNextPositon;

}
