using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SpeedControlManager : MonoBehaviour {

	public Sprite slowSprite;
	// public Sprite mediumSprite;
	public Sprite fastSprite;

	private GameObject speedOpener;
	private GameObject speedPanel;
	private GameObject speedSlow;
	private GameObject speedFast;

	private bool open;
	public bool Open {
		get { return open; }
		set {
			open = value;
			speedPanel.SetActive(value);
			speedFast.SetActive(value);
			speedSlow.SetActive(value);
		}
	}

	void Start ()
	{
		speedOpener = transform.GetChild(0).gameObject;
		speedPanel = transform.GetChild(1).gameObject;
		speedSlow = transform.GetChild(2).gameObject;
		speedFast = transform.GetChild(3).gameObject;
		UpdateSpeedUI();
	}

	#region GUI

	public void OpenPanel()
	{
		Open = true;
	}

	public void ClosePanel()
	{
		Open = false;
	}

	public void SetSlow()
	{
		SetSpeed(GameState.AUDIO_SPEED_SLOW);
		ClosePanel();
	}

	public void SetFast()
	{
		SetSpeed(GameState.AUDIO_SPEED_FAST);
		ClosePanel();
	}

	public void SetSpeed(int speed)
	{
		GameState.Instance.ActiveAudioSpeed = speed;
		UpdateSpeedUI();
	}

	public void UpdateSpeedUI()
	{
		switch (GameState.Instance.ActiveAudioSpeed)
		{
			case GameState.AUDIO_SPEED_SLOW:
				speedOpener.GetComponent<Image>().sprite = slowSprite;
				break;
			// case GameState.AUDIO_SPEED_MEDIUM:
			// 	speedOpener.GetComponent<Image>().sprite = mediumSprite;
			// 	break;
			case GameState.AUDIO_SPEED_FAST:
				speedOpener.GetComponent<Image>().sprite = fastSprite;
				break;
		}

		ClosePanel();
	}

	#endregion


}
