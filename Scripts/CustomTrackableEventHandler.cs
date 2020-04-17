/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/


/*
CustomTrackableEventHandler that inherits from the ITrackableEventHandler.
Each Image Target should run this instead of the DefaultTrackableEventHandler.

Important requirement for this to work is to carefully put each Video/Activity to the Image Targets.
More specifically, the Image Target's name, also shows the place in the sequence for this specific card.
For example:
If the correct sequence is [shower, wear pyjamas, go to sleep], then the video of showering should be 
placed on the Image target with name "QR1_scaled", the wear pyjamas video, should be placed on the 
Image Target "QR2_scaled", etc..

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Vuforia;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
///
/// Changes made to this file could be overwritten when upgrading the Vuforia version.
/// When implementing custom event handler behavior, consider inheriting from this class instead.
/// </summary>
public class CustomTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    #region PROTECTED_MEMBER_VARIABLES
	
	//Initialize variables.	
	public TextMeshProUGUI congratsText; //	Variable for the 3D text displaying when game is completed.	
	public TextMeshProUGUI failedText; //	Variable for the 3D text displaying when game is completed.	
	public AudioSource audioData; //Sound effect that plays when game is completed.
	public AudioClip clip;
	bool correctSequence = false;
	bool gameCompleted = false; //Checks if game has been completed.	
	
	public FirebaseManager firebaseManager;
	
	public Text attemptsText; 
	public Text shuffleText;
	public Text countdownText;

	int totalTrackablesNum; //Number of QR codes in this game. Change it respectively.	
	
	float startTime; 
	float remainingTime;
	int intRemainingTime;

	int attempts = -1; //Keeps the number of user's attempts till game completion.
	string order = ""; //Keeps the current order of the cards.
	string previousOrder = ""; //Keeps the previous order of the cards.
	
	float[] x_pos;  //It stores the x position of each tracked card.
	int[] sequenceOrder; // It stores the respective card order, e.g. [2, 1, 3]
	
	string sceneName;

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS
	
	
	
    protected virtual void Start()
    {		
		congratsText.enabled = false;
		failedText.enabled = false;
		shuffleText.enabled = false;
		audioData = (AudioSource)gameObject.AddComponent<AudioSource>();
		
		sceneName = getActiveSceneName(); //Get the name of the active scene. 
		totalTrackablesNum = getTrackableNumber(); //Get the number of trackables in the current level.
		
		startTime = getLevelTime(); //Get the start time of the current level (e.g. 40s).
		remainingTime = startTime;		

		//Create the array depending on the number of trackables.
		if (totalTrackablesNum == 3)
		{
			x_pos = new float[3];
			sequenceOrder = new int[3];
		}
		else if (totalTrackablesNum == 4)
		{
			x_pos = new float[4];
			sequenceOrder = new int[4];
		}
		else if (totalTrackablesNum == 5)
		{
			x_pos = new float[5];
			sequenceOrder = new int[5];
		}						

		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;

        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");

            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);		

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;
    }


    protected virtual void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;
    }
	
	void Update(){
		
		StateManager sm = TrackerManager.Instance.GetStateManager();
		IList<TrackableBehaviour> activeTrackables = (IList<TrackableBehaviour>) sm.GetActiveTrackableBehaviours();
		
		if (remainingTime <= 0.0f && failedText.enabled == false )
		{
		//The "failedText.enabled == false" in the if statement is used because the script run on all the image targets, and thus each of them would write on the Firebase.

			failedText.enabled = true;
			countdownText.enabled = false; 
			Debug.Log("TIME ENDED");			

			FindObjectOfType<FirebaseManager>().SaveData(attempts.ToString(), startTime.ToString(), "no", sceneName); //Write game statistics to Firebase.			
							
			
			Invoke("GameComplete", 2f); //Wait 2 seconds before calling the GameComplete method.
			//timeEnded();
			//TO FIX: Later will call the timeEnded method, which will lead to another scene, showing the correct answer.
		}
		else{
			if (gameCompleted == false) { 			
			
				remainingTime -= Time.deltaTime;
				intRemainingTime = (int) remainingTime;
				countdownText.GetComponent<Text>().text =  "Time remaining: " + intRemainingTime.ToString(); //Update the remaining time on the screen.		
				
				if(activeTrackables.Count == totalTrackablesNum) //If all the QR codes of the game are currently tracked. 
				{
					int index = 0;
					
					// Iterate through the list of active trackables
					foreach (TrackableBehaviour tb in activeTrackables) {	

						print("TRACKABLE BEVAVIOR: " + tb);
						print("x_POS: " + tb.transform.position.x);
					
						x_pos[index] = tb.transform.position.x; //Stores all the x positions (not in a sorted order).
						
						//Store the card order of the sequence, depending on the Image Target's name.
						if (tb.TrackableName == "QR1_scaled"){ 
							sequenceOrder[index] = 1;		
						}
						
						if (tb.TrackableName == "QR2_scaled"){ 
							sequenceOrder[index] = 2;		
						}					
						
						if (tb.TrackableName == "QR3_scaled"){ 
							sequenceOrder[index] = 3;
						}	

						if (tb.TrackableName == "QR4_scaled"){ 
							sequenceOrder[index] = 4;		
						}
						
						if (tb.TrackableName == "QR5_scaled"){ 
							sequenceOrder[index] = 5;		
						}					
						
						if (tb.TrackableName == "QR6_scaled"){ 
							sequenceOrder[index] = 6;
						}		
						
						if (tb.TrackableName == "QR7_scaled"){ 
							sequenceOrder[index] = 7;		
						}					
						
						if (tb.TrackableName == "QR8_scaled"){ 
							sequenceOrder[index] = 8;
						}	
						
						if (tb.TrackableName == "QR9_scaled"){ 
							sequenceOrder[index] = 9;		
						}					
						
						if (tb.TrackableName == "QR10_scaled"){ 
							sequenceOrder[index] = 10;
						}	
					
						index = index + 1;
						
					}
					
					sortArrays(x_pos, sequenceOrder);
					
					order = "";
					for (int i = 0; i < sequenceOrder.Length; i++) 
						order = order + sequenceOrder[i].ToString(); //Convert int array to string variable.
					
					correctSequence = isSorted(sequenceOrder); //Checks if sequence is correct.				
		

					if (correctSequence == true){ 	

						Debug.Log("correctSequence");


						if (attempts == -1)
						{
							Debug.Log("attempts are 0:");
							//Visualize a text to shuffle the cards
							//shuffleText. enabled = true;
							StartCoroutine(ShowMessage(3));
						}
						else{
							Debug.Log("attempts are NOT 0:" + attempts);
							attempts = attempts+1;
							attemptsText.GetComponent<Text>().text = "Attempts: " + attempts.ToString(); //Update the attempts count on the screen.
							gameCompleted = true;
							
							Debug.Log("CONGRATULATIONS! YOU WON!! Attempts: " + attempts + " Final Sequence: " + order);
											
							string gameDuration = ((int) startTime - intRemainingTime).ToString();
							if (congratsText.enabled == false){ 
							//The if statement is used because the script run on all the image targets, and thus each of them would write on the Firebase.
								FindObjectOfType<FirebaseManager>().SaveData(attempts.ToString(), gameDuration, "yes", sceneName); //Write game statistics to Firebase.		
							}			
													
							congratsText.enabled = true; //Display congratulations 3D shaded text. 
							AudioSource.PlayClipAtPoint(clip, transform.position); 	//Play epic effect audio.			



								
							Invoke("GameComplete", 3f); //Wait 3 seconds before calling the GameComplete method.								
						}					

					}
					else{
						
						if (order != previousOrder){ //Checks if the user has changed the order of the cards.
							attempts = attempts+1;
							previousOrder = order; 		
							attemptsText.GetComponent<Text>().text = "Attempts: " + attempts.ToString(); //Update the attempts count on the screen.
							Debug.Log("..... Attempts: " + attempts + "......" + " Current order: " + order);					
						}
					}					
				}			
			}
		}			
	}
		

	
	
	public IEnumerator ShowMessage (float delay)
	{
		shuffleText.enabled = true;
		yield return new WaitForSeconds(delay);
		shuffleText.enabled = false;
	}
	
	public float getLevelTime()
	{
		/*
		Depending on the scene name, the available time of the countdown timer changes.. 
		Returns the respective time of the level.
		*/
		
		if (sceneName.Contains("level_1"))		
			return 20f;
		else if (sceneName.Contains("level_2")) 
			return 40f;
		else if (sceneName.Contains("level_3"))
			return 60f;
		else
			return 0;		
	}
	
	
	public int getTrackableNumber()
	{
		/*
		Depending on the scene name, the number of total trackables number is different. 
		Returns the respective number.
		*/
		
		if (sceneName.Contains("level_1"))		
			return 3;
		else if (sceneName.Contains("level_2")) 
			return 4;
		else if (sceneName.Contains("level_3"))
			return 5;
		else
			return 0;
	}
	
	string getActiveSceneName()
	{
		/*
		Returns the active's scene name.
		*/
		
		Scene m_Scene;
		string currentScene;
		m_Scene = SceneManager.GetActiveScene();
		currentScene = m_Scene.name;
		
		return currentScene;
	}

    bool isSorted(int [] arr) 
    {   
	/*
	Checks if the cards' names are placed in ascending order.
	If yes, then that means that also the activities are in correct order.
	*/
        for (int i = 0; i < arr.Length - 1; i++) 
			
            // Unsorted pair found 
            if (arr[i] > arr[i+1]) 
                return false; 
  
        // No unsorted pair found 
        return true; 
    } 
	
	
	public (float[], int[]) sortArrays(float[] pos, int[] order)
	{
	/*
	Sorts two arrays, the x-positions of the cards and their respective names.
	*/		
        float pos_temp; 
		int order_temp;		
  
        // traverse 0 to array length 
        for (int i = 0; i < pos.Length - 1; i++) 
  
            // traverse i+1 to array length 
            for (int j = i + 1; j < pos.Length; j++) 
  
                // compare array element with  
                // all next element 
                if (pos[i] > pos[j]) { 
  
                    pos_temp = pos[i]; 
                    pos[i] = pos[j]; 
                    pos[j] = pos_temp; 
							
                    order_temp = order[i]; 
                    order[i] = order[j]; 
                    order[j] = order_temp; 					
					
                } 				
				return (pos, order);		
	}


	void GameComplete(){
		FindObjectOfType<GameSceneManager>().GoToActivitiesMenu(); //Return to Activities menu.		
	}
	
	
    #endregion // PROTECTED_METHODS
}
