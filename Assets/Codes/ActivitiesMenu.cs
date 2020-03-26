using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivitiesMenu : MonoBehaviour
{
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
