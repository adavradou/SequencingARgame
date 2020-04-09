/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
	public TextMeshProUGUI concratsText; //	Variable for the 3D text displaying when game is completed.	
	public AudioSource audioData; //Sound effect that plays when game is completed.
	public AudioClip clip;
	bool gameCompleted = false; //Checks if game has been completed.	

	private int num = 3; //Number of QR codes in this game. Change it respectively.	
	float x1, x2, x3;
	int trials = -1;
	string order = "";
	string previousOrder = "";

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {		
		concratsText.enabled = false;
		audioData = (AudioSource)gameObject.AddComponent<AudioSource>();

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
		
		if (gameCompleted == false) { 
			
			if(activeTrackables.Count==num) //If all the QR codes of the game are currently tracked. 
			{			
				// Iterate through the list of active trackables
				foreach (TrackableBehaviour tb in activeTrackables) {
					
					if (tb.TrackableName == "QR1_scaled"){ //If name contains "1"
						x1 = tb.transform.position.x;
						//Debug.Log(tb.TrackableName + " is at " + x1);
					}
					else if (tb.TrackableName == "QR2_scaled"){ //If name contains "2", etc...
						x2 = tb.transform.position.x;
						//Debug.Log(tb.TrackableName + " is at " + x2);
					}
					else if (tb.TrackableName == "QR3_scaled"){
						x3 = tb.transform.position.x;
						//Debug.Log(tb.TrackableName + " is at " + x3);
					}

				}
				
				// for i in activeTrackables-1
				// if x[i] <
				
				if ((x1 < x2) && (x2 < x3)){ //CHANGE!!
					order = "123";
				}
				else if ((x1 < x3) && (x3 < x2)){
					order = "132";
				}
				else if ((x2 < x1) && (x1 < x3)){
					order = "213";
				}
				else if ((x2 < x3) && (x3 < x1)){
					order = "231";
				}
				else if ((x3 < x2) && (x2 < x1)){
					order = "321";
				}			
				else if ((x3 < x1) && (x1 < x2)){
					order = "312";
				}		
				
				if (order == "123"){
					//trials = trials+1;
					
					if (order != previousOrder){
						trials = trials+1;
						previousOrder = order; 		
						Debug.Log("!!!!!!!!!" + trials + "!!!!!!!!!!" + " " + order);					
					}
					
					gameCompleted = true;
					
					Debug.Log("CONGRATULATIONS!!!!" + trials);
					
					concratsText.enabled = true; //Display congratulations 3D shaded text. 
					AudioSource.PlayClipAtPoint(clip, transform.position); 	//Play epic effect audio.				
						
					Invoke("GameComplete", 3f); //Wait 3 seconds before calling the GameComplete method.
								
					

				}
				else{
					//Debug.Log("......" + trials + "........." + order + " " + previousOrder);
					
					if (order != previousOrder){
						trials = trials+1;
						previousOrder = order; 		
						Debug.Log("!!!!!!!!!" + trials + "!!!!!!!!!!" + " " + order);					
					}
				}	
				

				
			}
			else //If not all the QR codes are currently tracked. 
			{
			}					
			
			
		}



	}
	
	void GameComplete(){
		FindObjectOfType<GameSceneManager>().GoToActivitiesMenu(); //Return to Activities menu.
	}

    #endregion // PROTECTED_METHODS
}
