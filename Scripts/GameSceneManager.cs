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
		//FindObjectOfType<AuthController>().Logout(); //Return to Activities menu.
		SceneManager.LoadScene("LoginMenu");
	}	

	
	public void GoBack()
	{
		SceneManager.LoadScene("MainMenu");
	}
	
	public void GoToStatisticsScene()
	{
		SceneManager.LoadScene("StatisticsScene");
	}
	
	
	static public string getActiveSceneName()
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
	
	public void goToSolutionsScene()
	{
		SceneManager.LoadScene("SolutionsScene");
	}
	
	//Showering activity.
	public void GoToShoweringLevel_1()
	{
		SceneManager.LoadScene("Showering_level_1");
	}	
	
	public void GoToShoweringLevel_2()
	{
		SceneManager.LoadScene("Showering_level_2");
	}	

	public void GoToShoweringLevel_3()
	{
		SceneManager.LoadScene("Showering_level_3");
	}	

	//Going to school activity.
	public void GoToSchoolLevel_1()
	{
		SceneManager.LoadScene("School_level_1");
	}	
	
	public void GoToSchoolLevel_2()
	{
		SceneManager.LoadScene("School_level_2");
	}	

	public void GoToSchoolLevel_3()
	{
		SceneManager.LoadScene("School_level_3");
	}		
	
	//Brushing the teeth activity. 
	public void GoToTeethLevel_1()
	{
		SceneManager.LoadScene("Teeth_level_1");
	}	
	
	public void GoToTeethLevel_2()
	{
		SceneManager.LoadScene("Teeth_level_2");
	}	

	public void GoToTeethLevel_3()
	{
		SceneManager.LoadScene("Teeth_level_3");
	}		
		

}