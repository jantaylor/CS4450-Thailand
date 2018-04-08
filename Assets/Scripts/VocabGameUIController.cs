using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocabGameUIController : MonoBehaviour {

	public VocabGameController vocabGameController;

	private GameObject forwardButton;
	private GameObject backButton;
	private GameObject checkButton;
	private bool isLastVocab;

	// Use this for initialization
	void Start () {
		vocabGameController = GetComponent<VocabGameController>();
		forwardButton = transform.Find("Arrow Right").gameObject;
		backButton = transform.Find("Arrow Left").gameObject;
		checkButton = transform.Find("Check").gameObject;
		forwardButton.SetActive(false);
		backButton.SetActive(false);
		checkButton.SetActive(false);
		isLastVocab = false;
	}

	void OnClick(MonoBehaviour sender)
	{
		GameObject gameObject = sender.GetComponent<GameObject>();
		if (gameObject.name == "Arrow Right")
		{
			vocabGameController.Proceed(1);
		}
		if (gameObject.name == "Arrow Left")
		{
			vocabGameController.Proceed(-1);
		}
	}

	public void Back()
	{
		if (!vocabGameController.Proceed(-1))
		{
			backButton.SetActive(false);
		}
		checkButton.SetActive(false);
		forwardButton.SetActive(true);
	}

	public void Forward()
	{
		if (!vocabGameController.Proceed(1))
		{
			isLastVocab = true;
		}
		forwardButton.SetActive(false);
		backButton.SetActive(true);
	}

	public void Match()
	{
		if (!isLastVocab)
		{
			forwardButton.SetActive(true);
		}
		else
		{
			checkButton.SetActive(true);
		}
	}

	public void Finish()
	{
		// TODO: Gamestate save progress
		// Menu();
		GameState.Instance.LoadScene(GameState.ACTIVITY_MATCH_GAME, GameState.Instance.ActiveRound);
	}

	public void Menu()
	{
		GameState.Instance.LoadMenu();
	}
}
