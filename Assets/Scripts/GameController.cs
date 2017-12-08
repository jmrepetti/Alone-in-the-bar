using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour {

	[SerializeField]
	private GameObject[] requestSpawnSpot;
	[SerializeField]
	private GameObject[] deliverySpawnSpot;
	// Use this for initialization
	[SerializeField]
	private GameObject[] food;
	[SerializeField]
	private GameObject clientPrefab;

	[SerializeField]
	private GameObject tutorial;

	[SerializeField]
	private GameObject score;
	[SerializeField]
	private GameObject finalScore;

	[SerializeField]
	private GameObject level;

	[SerializeField]
	private GameObject levelDisplay;

	[SerializeField]
	private GameObject mistakes;

	[SerializeField]
 	private GameObject gameOverDisplay;

	private int maxClientsPerQueue = 6;
	private Queue<int> orderQueue;
	private Queue<GameObject>[] clientQueues;

	private GameObject[] clientRequests;
	private GameObject[] readyRequests;
	// Use this for initialization


	void Start () {
		GameState.score = 0;
		GameState.level = 1;
		GameState.tutorialStep = 1;
		GameState.mistakes = 3;
		GameState.gameOver = false;
		GameState.displayGameOver = false;
		Destroy(tutorial, 5f);

		clientQueues = new Queue<GameObject>[4];

		clientQueues[0] = new Queue<GameObject>();
		clientQueues[1] = new Queue<GameObject>();
		clientQueues[2] = new Queue<GameObject>();
		clientQueues[3] = new Queue<GameObject>();

		orderQueue = new Queue<int>();

		clientRequests = new GameObject[4]
		{
			null, null, null, null
		};

		readyRequests = new GameObject[3]
		{
			null, null, null
		};

		StartCoroutine (NextLevel ());
		StartCoroutine (SpawnDeliveryFood ());
		StartCoroutine (SpawnClients ());
		// InstatiateInspector ();

	}

	// Update is called once per frame
	void Update () {
		if (GameState.gameOver) {

			if (Input.GetKeyDown(KeyCode.R)) {
				SceneManager.LoadScene("Game");

			  // SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			}
		}
	}

	public GameObject OrderFood() {
		int foodNumber = Random.Range(0, 6);
		orderQueue.Enqueue(foodNumber);
		return food[foodNumber];
	}


	IEnumerator NextLevel() {
		while (!GameState.gameOver) {
			yield return new WaitForSeconds (25f);
			level.GetComponent<TextMesh>().text = "-------";
			GameState.level += 1;
			level.GetComponent<TextMesh>().text = "LEVEL " + GameState.level.ToString();
		}
	}

	IEnumerator SpawnDeliveryFood() {
		while (!GameState.gameOver) {

			for (int i = 0; i < readyRequests.Length; i++) {
				if ( (readyRequests[i] == null) && (orderQueue.Count > 0) ) {
					int foodNumber = orderQueue.Dequeue();
					GameObject foodInstance = Instantiate(food[foodNumber], deliverySpawnSpot[i].transform.position, Quaternion.identity);
					readyRequests[i] = foodInstance;
				}
			}

			yield return new WaitForSeconds (Random.Range(1f, 3f));
		}

	}
	bool FullBar() {
		return 	clientQueues[0].Count == maxClientsPerQueue &&
						clientQueues[1].Count == maxClientsPerQueue &&
						clientQueues[2].Count == maxClientsPerQueue &&
						clientQueues[3].Count == maxClientsPerQueue;
	}
	IEnumerator SpawnClients() {
		while (!GameState.gameOver) {


			if (FullBar()) {
				GameState.gameOver = true;
				finalScore.GetComponent<TextMesh>().text = GameState.GetScoreText();
				gameOverDisplay.SetActive(true);
			}

			if (GameState.tutorialStep == 1) {
				GameState.tutorialStep = 2;
				yield return new WaitForSeconds(7f);
			}

			int spawnQueue = Random.Range(0, 4);
			Queue<GameObject> queue = clientQueues[spawnQueue];
			int clientsInQueue = queue.Count;
			if (clientsInQueue < maxClientsPerQueue) {
				float x = -22.5f + (15f * (float)spawnQueue);
				float z = 4f - (7f * (float)(clientsInQueue));
				Vector3 newClientPosition = new Vector3(x,1.5f,z);

				GameObject newClient = Instantiate(clientPrefab, newClientPosition, Quaternion.identity);
				newClient.GetComponent<ClientController>().SetGameInstante(this);
				newClient.GetComponent<ClientController>().SetQueueNumber(spawnQueue);
				queue.Enqueue(newClient);
			}

			if ( GameState.tutorialStep == 2 ) {
				GameState.tutorialStep = 3;
				levelDisplay.SetActive(true);
				yield return new WaitForSeconds (5f);
			} else {
				yield return new WaitForSeconds (1f + (1f / GameState.level));
			}

		}

	}
	public GameObject pickUpFrom(int pickUpQueueNumber) {

		int queue = 0;
		GameObject pickedup;

		switch (pickUpQueueNumber) {
			case 1:
				queue = 2;
				break;
			case 3:
				queue = 1;
				break;
			case 5:
				queue = 0;
				break;
		}

		pickedup = readyRequests[queue];
		readyRequests[queue] = null;
		return pickedup;
	}

	public bool deliverFoodTo(GameObject food, int clientNumber) {
		int queue = 0;


		switch (clientNumber) {
			case 0:
				queue = 3;
				break;
			case 2:
				queue = 2;
				break;
			case 4:
				queue = 1;
				break;
			case 6:
				queue = 0;
				break;
		}

		if ( clientQueues[queue].Peek() != null) {

			GameObject orderedFood = clientQueues[queue].Peek().GetComponent<ClientController>().GetOrder();

			if (orderedFood.name == food.name) {
				GameState.tutorialStep = 4;

				GameObject clientServed = clientQueues[queue].Dequeue();
				Destroy(clientServed);

				foreach (GameObject client in clientQueues[queue])
				{
					client.GetComponent<ClientController>().GoForward();
				};
				// GameState.UpdateScore(10);
				GameState.score += 10;

				score.GetComponent<TextMesh>().text = GameState.GetScoreText();

				return true;
			} else {
				GameState.mistakes--;

				if (GameState.mistakes < 0) {
					GameState.gameOver = true;
					finalScore.GetComponent<TextMesh>().text = GameState.GetScoreText();
					gameOverDisplay.SetActive(true);
				} else {
					mistakes.GetComponent<TextMesh>().text = GameState.GetMistakeText();
				}

				return false;
			}
		} else {
			return false;
		}


	}
}
