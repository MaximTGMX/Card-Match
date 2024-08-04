using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
	private readonly Dictionary<EPuzzleCategories, string> _puzzleCatDirectory = new Dictionary<EPuzzleCategories, string>(); 
	private int settings;
	private const int SettingsNumber = 2;
	private bool MuteFX = false;
	
	public enum EPairNumber
	{
		NotSet = 0,
		E2Pairs = 2,
		E3Pairs = 3,
		E4Pairs = 4,
	}
	
	public enum EPuzzleCategories
	{
		NotSet,
		Cards2 = 1,
		Cards3 = 2,
		Cards4 = 3,
		Cards5 = 4,
		Cards6 = 5,
		Cards7 = 6,
		Cards8 = 7,
		Cards9 = 8,
		Cards10 = 9,
	}
	
	public struct Settings
	{
		public EPairNumber PairsNumber;
		public EPuzzleCategories PuzzleCategory;
	}
	
	private Settings gameSettings;
	
	public static GameSettings Instance;
	
	void Awake()
	{
		if (Instance == null) 
		{
			DontDestroyOnLoad(this);
			Instance = this;
		}
		else
		{
			Destroy(this);
		}
	}
	
    void Start()
    {
        gameSettings = new Settings();
		ResetGameSettings();
    }

	public void SetPairNumber(EPairNumber Number)
	{
		if (gameSettings.PairsNumber == EPairNumber.NotSet) settings++;
		gameSettings.PairsNumber = Number;
	}
	
	public void SetPuzzleCategories(EPuzzleCategories cat)
	{
		if (gameSettings.PuzzleCategory == EPuzzleCategories.NotSet) settings++;
		gameSettings.PuzzleCategory = cat;
	}
	
	public EPairNumber GetPairNumber()
	{
		return gameSettings.PairsNumber;
	}
	
	public EPuzzleCategories GetPuzzleCategory()
	{
		return gameSettings.PuzzleCategory;
	}
	
	public void ResetGameSettings()
	{
		settings = 0;
		gameSettings.PuzzleCategory = EPuzzleCategories.NotSet;
		gameSettings.PairsNumber = EPairNumber.NotSet;
	}
	
	public bool AllSettingsReady()
	{
		return settings == SettingsNumber;
	}
	
	public string GetMaterialDirectoryName()
	{
		return "Materials/";
	}	
	
	public string GetPuzzleCategoryDirectoryName()
	{
		return "Graphics/PuzzleCat/Fruits/";
	}
	
	public void Mute(bool muted)
	{
		MuteFX = muted;
	}
	
	public bool IsSoundMuted()
	{
		return MuteFX;
	}
}
