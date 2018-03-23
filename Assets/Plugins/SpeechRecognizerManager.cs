using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Unity Speech Recognizer manager.
/// </summary>
public class SpeechRecognizerManager
{
	private const string JAVA_CLASS_NAME = "com.takohi.unity.plugins.SpeechRecognizerManager";
	public const string RESULT_SEPARATOR = "/::/";
	public const int	EVENT_SPEECH_READY = -1; // On Speech ready.
	public const int	EVENT_SPEECH_BEGINNING = -2; // On beginning of Speech.
	public const int	EVENT_SPEECH_END = -3; // On end of Speech.
	public const int	ERROR_NOT_INITIALIZED = -1; // Not initialized.
	public const int	ERROR_AUDIO = 3; // Audio recording error.
	public const int	ERROR_CLIENT = 5;	// Other client side errors.
	public const int	ERROR_INSUFFICIENT_PERMISSIONS = 9; // Insufficient permissions
	public const int	ERROR_NETWORK = 2;	// Other network related errors.
	public const int	ERROR_NETWORK_TIMEOUT = 1;	// Network operation timed out.
	public const int	ERROR_NO_MATCH = 7;	// No recognition result matched.
	public const int	ERROR_RECOGNIZER_BUSY = 8;	// RecognitionService busy.
	public const int	ERROR_SERVER = 4;	// Server sends error status.
	public const int	ERROR_SPEECH_TIMEOUT = 6;	// No speech input

	private static AndroidJavaObject srManager = null;

	public SpeechRecognizerManager (string gameObjectName)
	{
		if (Application.platform != RuntimePlatform.Android)
			return;
		
		srManager = new AndroidJavaObject (JAVA_CLASS_NAME, new object[] {
			gameObjectName
		});
	}

	/// <summary>
	/// Checks whether a speech recognition service is available on the system.
	/// </summary>
	/// <returns><c>true</c> if is available; otherwise, <c>false</c>.</returns>
	public static bool IsAvailable ()
	{
		if (Application.platform != RuntimePlatform.Android)
			return false;

		AndroidJavaClass jc = new AndroidJavaClass (JAVA_CLASS_NAME);
		return jc.CallStatic<bool> ("isAvailable");
	}

	/// <summary>
	/// Starts the listening.
	/// </summary>
	/// <param name="maxResults">Max results (optional, default=5).</param>
	/// <param name="language">Preferred language (optional, default=null for default language).</param>
	public void StartListening(int maxResults=5, string language=null)
	{
		if (Application.platform != RuntimePlatform.Android)
			return;

		srManager.Call ("startListening", language, maxResults);
	}

	/// <summary>
	/// Stops listening for speech. Speech captured so far will be recognized as if the user had stopped speaking at this point. 
	/// </summary>
	public void StopListening ()
	{
		if (Application.platform != RuntimePlatform.Android)
			return;
		
		srManager.Call ("stopListening");
	}

	/// <summary>
	/// Cancels the speech recognition.
	/// </summary>
	public void CancelListening ()
	{
		if (Application.platform != RuntimePlatform.Android)
			return;
		
		srManager.Call ("cancel");
	}

	/// <summary>
	/// Releases the object.
	/// </summary>
	public void Release ()
	{
		if (Application.platform != RuntimePlatform.Android)
			return;
		
		srManager.Call ("release");
	}
}
