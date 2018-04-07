using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchGameUIController : MonoBehaviour {

	public MatchGameController matchGameController;

	private GameObject forwardButton;
	private GameObject backButton;
	private GameObject checkButton;
	private bool isLastVocab;

	// Use this for initialization
	void Start () {
		matchGameController = GetComponent<MatchGameController>();
		forwardButton = transform.Find("Arrow Right").gameObject;
		backButton = transform.Find("Arrow Left").gameObject;
		checkButton = transform.Find("Check").gameObject;
		forwardButton.SetActive(false);
		backButton.SetActive(false);
		checkButton.SetActive(false);
		isLastVocab = false;
	}

	// public void Back()
	// {
	// 	if (!matchGameController.Proceed(-1))
	// 	{
	// 		backButton.SetActive(false);
	// 	}
	// 	checkButton.SetActive(false);
	// 	forwardButton.SetActive(true);
	// }

	public void Forward()
	{
		matchGameController.Proceed();
		forwardButton.SetActive(false);
	}

	public void EnableForward(bool finished)
	{
		if (finished)
		{
			checkButton.SetActive(true);
		}
		else
		{
			forwardButton.SetActive(true);
		}
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
		Menu();
	}

	public void Menu()
	{
		GameState.Instance.LoadMenu();
	}
}
