using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
	public void GoToMainMenu()
	{
		//SceneManager.LoadScene(0);
		SceneManager.LoadScene("MainMenu");
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	
	public void QuitGame()
	{
		Application.Quit();
	}



//public class MainMenu : MonoBehaviour

	public void GoToActivitiesMenu()
	{
		//SceneManager.LoadScene(0);
		SceneManager.LoadScene("ActivitiesMenu");
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	
	public void Logout()
	{
		//Application.Quit();
		SceneManager.LoadScene("LoginMenu");
	}
	



//public class ActivitiesMenu : MonoBehaviour

	public void GoToLevel_1()
	{
		//SceneManager.LoadScene(0);
		SceneManager.LoadScene("ARscene");
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	
	public void GoBack()
	{
		//Application.Quit();
		SceneManager.LoadScene("MainMenu");
	}
}