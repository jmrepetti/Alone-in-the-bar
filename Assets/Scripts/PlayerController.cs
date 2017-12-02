using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	float step = 7.5f;
	float walkLimit = 22.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown("left") && (transform.position.x < walkLimit))
//			transform.Translate(Vector3.left * -step);
			transform.position += Vector3.right * step;
		
		if (Input.GetKeyDown("right") && (transform.position.x > -walkLimit))
			transform.position += Vector3.left * step;
		
//		
//
//
//		if (Input.GetButton("Jump")) {
//			_thrusterForce = Vector3.up * thrusterForce;
//			SetJointSettings(0f);
//		} else {
//			SetJointSettings(jointSpring);
//		}
	}
}
