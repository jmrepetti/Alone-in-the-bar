using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientController : MonoBehaviour {

[SerializeField]
	public GameObject requestSpot;
	private int queue;
  private GameObject orderedFood;
	private bool goForward;
	private GameController gameController;
	private Vector3 fromPos;
	private Vector3 newPos;
  private float fraction_of_the_way_there;
	// Use this for initialization
	void Start () {
		fromPos = transform.position;
		newPos = fromPos + Vector3.forward * 7;
		fraction_of_the_way_there = 0f;
	}

	// Update is called once per frame
	void Update () {
		if (goForward) {
			fraction_of_the_way_there  += 0.05f;
			transform.position = Vector3.Lerp(fromPos, newPos, fraction_of_the_way_there);
			if (transform.position == newPos) {
				goForward = false;
				fromPos = newPos;
				newPos = fromPos + Vector3.forward * 7;
				fraction_of_the_way_there = 0;
			}
		}
	}

	public void GoForward() {
		goForward = true;
	}
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "Cube") {
			SetOrder(gameController.OrderFood());
		}
  }

	public void SetGameInstante(GameController _gameController) {
		gameController = _gameController;
	}
  public GameObject GetOrder() {
		return orderedFood;
	}
	public void SetOrder(GameObject food) {
	  GameObject foodInstance = Instantiate(food, requestSpot.transform.position, Quaternion.identity);
		foodInstance.GetComponent<FoodController>().Glow();
		foodInstance.GetComponent<FoodController>().setRotate(true);
		orderedFood = foodInstance;
		foodInstance.transform.parent = requestSpot.transform;
		StartCoroutine(hideFoodOrder());
	}
	IEnumerator hideFoodOrder() {

		if (GameState.tutorialStep <= 3) {
			yield return new WaitForSeconds (3f);
		} else {
			yield return new WaitForSeconds (1f);
		}

		orderedFood.transform.localScale = new Vector3(0, 0, 0);
		// (Behaviour)orderedFood.GetComponent("Halo").enabled = false;
		Behaviour halo = (Behaviour)orderedFood.GetComponent("Halo");
		Destroy(halo);
	}
	public void SetQueueNumber(int number) {
		queue = number;
	}
	public int GetQueueNumber() {
		return queue;
	}
}
