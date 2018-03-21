using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocabUIController : MonoBehaviour {

	public VocabController vocabController;

	private GameObject forwardButton;
	private GameObject backButton;
	private GameObject checkButton;

	// Use this for initialization
	void Start () {
		vocabController = GameObject.Find("Vocab Container").GetComponent<VocabController>();
		forwardButton = transform.Find("Arrow Right").gameObject;
		backButton = transform.Find("Arrow Left").gameObject;
		backButton.SetActive(false);
		checkButton = transform.Find("Check").gameObject;
		checkButton.SetActive(false);
	}

	// Update is called once per frame
	void Update () {

	}

	public void Back()
	{
		if (!vocabController.Proceed(-1))
		{
			backButton.SetActive(false);
		}
		checkButton.SetActive(false);
		forwardButton.SetActive(true);
	}

	public void Forward()
	{
		if (!vocabController.Proceed(1))
		{
			forwardButton.SetActive(false);
			checkButton.SetActive(true);
		}
		backButton.SetActive(true);
	}

	public void Finish()
	{
		GameState.Instance.LoadScene(4, GameState.Instance.ActiveRound);
	}

	public void Menu()
	{
		GameState.Instance.LoadMenu();
	}
}
