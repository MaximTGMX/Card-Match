using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehaviour : MonoBehaviour
{
	public bool HasDelay = false;
	private string BackSceneName;
	
	public void LoadScene(string scene_name)
	{
		BackSceneName = scene_name;
		if (!HasDelay)
			SceneManager.LoadScene(BackSceneName);
		else
			StartCoroutine(Wait());
	}
	
	private IEnumerator Wait()
	{
		yield return new WaitForSeconds(0.6f);
		SceneManager.LoadScene(BackSceneName);
	}
	
	public void ResetGameSettings()
	{
		GameSettings.Instance.ResetGameSettings();
	}
}
