using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
	public void GoToMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
		//SceneManager.LoadScene(0);		
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	
	public void QuitGame()
	{
		Application.Quit();
	}


	public void GoToActivitiesMenu()
	{
		SceneManager.LoadScene("ActivitiesMenu");
	}
		
	
	public void Logout()
	{
		SceneManager.LoadScene("LoginMenu");
	}	


	public void GoToLevel_1()
	{
		SceneManager.LoadScene("ARscene");
	}
	
	
	public void GoBack()
	{
		SceneManager.LoadScene("MainMenu");
	}
}