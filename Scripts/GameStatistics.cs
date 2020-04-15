using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class GameStatistics
{
	
	public string gameAttempts;
	public string gameDuration;
	public string levelPassed;
	
	public GameStatistics(){}
	
	
	public GameStatistics(string attempts, string duration, string passed)
	{
		gameAttempts = attempts;
		gameDuration = duration;
		levelPassed = passed;
	}


}
