using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentUser : MonoBehaviour
{
	
	static string userEmail = "";
	static string levelPlayed = "";
	
	static public string getUserEmail()
	{
		return userEmail;
	}
	
	static public void setUserEmail(string newUserEmail)
	{
		userEmail = newUserEmail;
	}
	
	
	
	static public string getLevelPlayed()
	{
		return levelPlayed;
	}
	
	static public void setLevelPlayed(string newLevel)
	{
		levelPlayed = newLevel;
	}
	
	
}
