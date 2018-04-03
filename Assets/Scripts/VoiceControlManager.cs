using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class VoiceControlManager : MonoBehaviour {


	///<summary>
	/// Stores the results of the speech recognition.
	///</summary>
	public string input;
	public Sprite enabledSprite;
	public Sprite disabledSprite;
	public Sprite stopSprite;

	///<summary>
	/// The GameObject Script that handles the results of the speech recognition.
	///</summary>
	public SpeechHandler speechHandler;

	private Text debugText;
	private GameObject panel;
	private GameObject startSpeech;
	private GameObject cancelSpeech;
	private SpeechRecognizerManager SpeechManager = null;

	private bool isListening = false;
	public bool IsListening {
		get { return isListening; }
		set {
			isListening = value;
			startSpeech.GetComponent<Image>().sprite = (value? stopSprite : enabledSprite);
			cancelSpeech.SetActive(value);
			panel.SetActive(value);
		}
	}

	private bool enabled;
	public bool Enabled {
		get { return enabled; }
		set {
			enabled = value;
			if (value) {
				startSpeech.GetComponent<Image>().sprite = enabledSprite;
			}
			else {
				startSpeech.GetComponent<Image>().sprite = disabledSprite;
			}
		}
	}

	///<summary>
	/// For Debugging
	///</summary>
	private string message = "";
	public string Message {
		get { return message; }
		set {
			message = value;
			DebugLog(message);
		}
	}

	#region MONOBEHAVIOUR

	void Start ()
	{
		panel = transform.GetChild(0).gameObject;
		startSpeech = transform.GetChild(1).gameObject;
		cancelSpeech = transform.GetChild(2).gameObject;

		#if UNITY_EDITOR
		debugText = GameObject.Find("DebugMesh").GetComponent<Text>();
		#endif

		IsListening = false;
		Enabled = true;

		SpeechManager = new SpeechRecognizerManager(gameObject.name);

		DebugLog("Devices: " + String.Join(", ", Microphone.devices));


		if (Application.platform != RuntimePlatform.Android) {
			Debug.Log ("Speech recognition is only available on Android platform.");
			Enabled = false;
			return;
		}

		if (!SpeechRecognizerManager.IsAvailable ()) {
			Debug.Log ("Speech recognition is not available on this device.");
			Enabled = false;
			return;
		}

		// We pass the game object's name that will receive the callback messages.
		SpeechManager = new SpeechRecognizerManager (gameObject.name);
	}

	void OnDestroy ()
	{
		if (SpeechManager != null)
		SpeechManager.Release ();
	}

	#endregion

	#region GUI

	///<summary>
	/// Starts/finishes the recording/recognizing.
	///</summary>
	public void ToggleListening()
	{
		DebugLog("Toggle Listening");
		if (SpeechRecognizerManager.IsAvailable() && Enabled)
		{
			DebugLog("Available and Enabeld");
			if (!IsListening)
			{
				DebugLog("Listening");
				IsListening = true;
				SpeechManager.StartListening(3, "en-UK"); // Return 3 results maximum, and use English
			}
			else
			{
				DebugLog("Stopped");
				IsListening = false;
				SpeechManager.StopListening();
			}
		}
	}

	///<summary>
	/// Cancels the recording/recognizing.
	///</summary>
	public void CancelListening()
	{
		if (IsListening)
		{
			IsListening = false;
			SpeechManager.CancelListening();
			DebugLog("Cancelled");
		}
	}

	#endregion


	#region SPEECH_CALLBACKS

	void OnSpeechEvent (string e)
	{
		switch (int.Parse (e)) {
			case SpeechRecognizerManager.EVENT_SPEECH_READY:
			DebugLog ("Ready for speech");
			break;
			case SpeechRecognizerManager.EVENT_SPEECH_BEGINNING:
			DebugLog ("User started speaking");
			break;
			case SpeechRecognizerManager.EVENT_SPEECH_END:
			DebugLog ("User stopped speaking");
			break;
		}
	}

	void OnSpeechResults (string results)
	{
		IsListening = false;

		// Need to parse
		string[] texts = results.Split (new string[] { SpeechRecognizerManager.RESULT_SEPARATOR }, System.StringSplitOptions.None);
		input = String.Join(" ", texts);
		DebugLog ("Speech results:\n   " + string.Join ("\n   ", texts));

		// If a script is assigned to handle the speech results, call it.
		if (speechHandler != null)
		{
			speechHandler.OnSpeechResults(texts);
		}
	}

	void OnSpeechError (string error)
	{
		switch (int.Parse (error)) {
			case SpeechRecognizerManager.ERROR_AUDIO:
			DebugLog ("Error during recording the audio.");
			break;
			case SpeechRecognizerManager.ERROR_CLIENT:
			DebugLog ("Error on the client side.");
			break;
			case SpeechRecognizerManager.ERROR_INSUFFICIENT_PERMISSIONS:
			DebugLog ("Insufficient permissions. Do the RECORD_AUDIO and INTERNET permissions have been added to the manifest?");
			break;
			case SpeechRecognizerManager.ERROR_NETWORK:
			DebugLog ("A network error occured. Make sure the device has internet access.");
			break;
			case SpeechRecognizerManager.ERROR_NETWORK_TIMEOUT:
			DebugLog ("A network timeout occured. Make sure the device has internet access.");
			break;
			case SpeechRecognizerManager.ERROR_NO_MATCH:
			DebugLog ("No recognition result matched.");
			break;
			case SpeechRecognizerManager.ERROR_NOT_INITIALIZED:
			DebugLog ("Speech recognizer is not initialized.");
			break;
			case SpeechRecognizerManager.ERROR_RECOGNIZER_BUSY:
			DebugLog ("Speech recognizer service is busy.");
			break;
			case SpeechRecognizerManager.ERROR_SERVER:
			DebugLog ("Server sends error status.");
			break;
			case SpeechRecognizerManager.ERROR_SPEECH_TIMEOUT:
			DebugLog ("No speech input.");
			break;
			default:
			break;
		}

		IsListening = false;
	}

	#endregion

	#region DEBUG

	private void DebugLog (string message)
	{
		Debug.Log (message);

		#if UNITY_EDITOR
		debugText.text = message;
		#endif
	}

	#endregion


}
