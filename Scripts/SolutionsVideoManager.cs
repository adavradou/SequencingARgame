using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SolutionsVideoManager : MonoBehaviour
{
	
	public RawImage rawImage;
	public VideoPlayer videoPlayer;
	public AudioSource audioSource;
	
	
	string sceneName;
	
	
    // Start is called before the first frame update
    void Start()
    {
		
		sceneName = CurrentUser.getLevelPlayed(); //Get the name of the last played level/scene.       

		print("BEFORE URL");
		getVideoUrl(sceneName);		
		//videoPlayer.url = "Assets/Resources/03_level2.mp4";  //TO FIX: play video depending on the level.
		print("AFTER URL, STARTING COROUTINE: " + videoPlayer.url);
		StartCoroutine(PlayVideo());
		
		videoPlayer.loopPointReached += EndReached;
		
		
		print("AFTER COROUTINE, GOING BACK");
		//FindObjectOfType<GameSceneManager>().GoToActivitiesMenu(); //Return to Activities menu.			
	}
	
	void EndReached(UnityEngine.Video.VideoPlayer vp)
	{
		FindObjectOfType<GameSceneManager>().GoToActivitiesMenu(); //Return to Activities menu.	
	}
	
	void getVideoUrl(string scene) //TO FIX: USE THE CORRECT URLS FOR EACH CASE. 
	{
		print("SCENE IS: " + scene);
	
		switch (scene) 
		{
		  case "Showering_level_1":
			videoPlayer.url = "Assets/Resources/solution_shower_level1.mp4";
			print("IN: " + "1");
			break;
		  case "Showering_level_2":
			videoPlayer.url = "Assets/Resources/solution_shower_level2.mp4";
			print("IN: " + "2");
			break;
		  case "Showering_level_3":
			videoPlayer.url = "Assets/Resources/solution_shower_level3.mp4";
			print("IN: " + "3");
			break;
		  case "School_level_1":
			videoPlayer.url = "Assets/Resources/solution_school_level1.mp4";
			print("IN: " + "4");
			break;
		  case "School_level_2":
			videoPlayer.url = "Assets/Resources/solution_school_level2.mp4";
			print("IN: " + "5");
			break;
		  case "School_level_3":
			videoPlayer.url = "Assets/Resources/solution_school_level3.mp4";
			print("IN: " + "6");
			break;
		  case "Teeth_level_1":
			videoPlayer.url = "Assets/Resources/solution_teeth_level1.mp4";
			print("IN: " + "7");
			break;	
		  case "Teeth_level_2":
			videoPlayer.url = "Assets/Resources/solution_teeth_level2.mp4";
			print("IN: " + "8");
			break;	
		  case "Teeth_level_3":
			videoPlayer.url = "Assets/Resources/solution_teeth_level3.mp4";
			print("IN: " + "9");
			break;				
		}
		
	}
	
	
	
	IEnumerator PlayVideo()
	{
		videoPlayer.Prepare();
		WaitForSeconds waitForSeconds = new WaitForSeconds(1);
		while (!videoPlayer.isPrepared)
		{
			yield return waitForSeconds;
			break;
		}
		
		rawImage.texture = videoPlayer.texture;
		videoPlayer.Play();
		audioSource.Play();
	}


}
