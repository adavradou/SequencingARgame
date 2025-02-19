﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlaySolutionVideo : MonoBehaviour
{
	
	public RawImage rawImage;
	public VideoPlayer videoPlayer;
	public AudioSource audioSource;
	
	
	
	
	
    // Start is called before the first frame update
    void Start()
    {
		videoPlayer.url = "Assets/Resources/03_level1.mp4";  //TO FIX: play video depending on the level.
		StartCoroutine(PlayVideo());
        
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
