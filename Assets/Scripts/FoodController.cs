using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour {

	// Use this for initialization
	private bool isRotating = false;
	private Behaviour halo;


	void Start () {
		// halo =(Behaviour)gameObject.GetComponent ("Halo");
		// halo.enabled = false;
	}

	// Update is called once per frame
	void Update () {
		if (isRotating) {
			transform.Rotate(0, Time.deltaTime * 50f, 0);
		}
	}

	public void Glow() {
		// halo.enabled = true;
	}

	public void setRotate(bool _isRotating) {
		this.isRotating = _isRotating;
	}
}
