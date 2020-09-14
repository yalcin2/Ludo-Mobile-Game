using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour{

	public GameObject initSpawnPoint;
	public GameObject[] platforms;

	public CameraScript cam;
	public float bounds;


		
	public Vector3 initPosition;
	
	private void Awake(){
		initPosition = initSpawnPoint.transform.position;
		
		currentPositon = initPosition;
//		initPosition = currentPositon = transform.position;
	}
	private void Start (){
		createBlock();		
	}


	private void createBlock(){
		prev = Instantiate(platforms[Random.Range(0, platforms.Length)], currentPositon, Quaternion.identity,transform);
		float minClear = prev.GetComponent<EdgeCollider2D>().bounds.size.x;
		float diff = Random.Range(minClear + 3f, minClear + 4.5f);
		currentPositon = currentPositon + new Vector3(diff, Random.Range(-1.5f, 1.5f), 0);
		currentPositon.y = Mathf.Clamp(currentPositon.y, initPosition.y-bounds, initPosition.y+bounds);	
	}
	
	
	// Update is called once per frame
	private void Update (){
		if (prev.transform.position.x <= cam.getRightBound() + cam.getCamLength()){
			createBlock();
		
		}

		int childCount =  transform.childCount;
		for (int i = childCount-1; i >= 0; --i){
			var trans = transform.GetChild(i);
			if (trans.position.x+cam.getCamLength()*1.5f <= cam.transform.position.x){
				Destroy(trans.gameObject);
			}
			
		}
		
	}
	
	private Vector3 currentPositon;
	private GameObject prev;
	private float camPos;


}
