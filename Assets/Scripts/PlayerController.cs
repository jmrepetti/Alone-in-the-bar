using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField]
	private GameObject game;

	[SerializeField]
	private GameObject carryingSpot;


	private GameObject serving;

	private GameController gameController;
	float step = 7.5f;
	float walkLimit = 22.5f;

	int spot = 3; //Pick up spot (1,3,5),or deliver spot (0,2,4,6)

	// Use this for initialization
	void Start () {
		gameController = game.GetComponent<GameController>();
	}

	void turn180() {
		transform.Rotate(0f, 180f, 0f);
	}
	// Update is called once per frame
	void Update () {
		if (GameState.gameOver)
			return;

		if (Input.GetKeyDown("left") && (transform.position.x < walkLimit)) {
			transform.position += Vector3.right * step;
			spot--;
			turn180();
		}

		if (Input.GetKeyDown("right") && (transform.position.x > -walkLimit)) {
			transform.position += Vector3.left * step;
			spot++;
			turn180();
		}

		if (Input.GetKeyDown("space")) {
			if ((spot % 2) == 0) {
				if ( serving != null ) {
					if ( gameController.deliverFoodTo(serving, spot) ) {
						Destroy(serving);
						serving = null;
					}
				}
			} else {
				if ( serving == null ) {
					serving = gameController.pickUpFrom(spot);
					serving.transform.position = carryingSpot.transform.position;
					serving.transform.parent = carryingSpot.transform;
				}
			}
		}

	}
}
