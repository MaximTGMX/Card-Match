using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	private int lockpick1 = 0;
	private int lockpick2 = 0;
	private int lockpick3 = 0;
	private int lockpick4 = 0;
	
	public static Inventory Instance;
	public Text lockText1;
	public Text lockText2;
	public Text lockText3;
	public Text lockText4;
	
	private void Awake()
	{
		if (Instance == null) 
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}
	}
	
	void Start()
    {
		lockpick1 = PlayerPrefs.GetInt("woodpick",0);
		lockText1.text = ": " + lockpick1.ToString();
		
		lockpick2 = PlayerPrefs.GetInt("ironpick",0);
		lockText2.text = ": " + lockpick2.ToString();	
		
		lockpick3 = PlayerPrefs.GetInt("goldpick",0);
		lockText3.text = ": " + lockpick3.ToString();	
		
		lockpick4 = PlayerPrefs.GetInt("diamondpick",0);
		lockText4.text = ": " + lockpick4.ToString();
    }
	
	public void AddLockPick1(int amount)
	{
		lockpick1+=amount;
		if (lockpick1>9999) lockpick1 = 9999;
		lockText1.text = ": " + lockpick1.ToString();
		PlayerPrefs.SetInt("woodpick",lockpick1);
	}	
	
	public void AddLockPick2(int amount)
	{
		lockpick2+=amount;
		if (lockpick2>9999) lockpick2 = 9999;
		lockText2.text = ": " + lockpick2.ToString();
		PlayerPrefs.SetInt("ironpick",lockpick2);
	}	
	
	public void AddLockPick3(int amount)
	{
		lockpick3+=amount;
		if (lockpick3>9999) lockpick3 = 9999;
		lockText3.text = ": " + lockpick3.ToString();
		PlayerPrefs.SetInt("goldpick",lockpick3);
	}	
	
	public void AddLockPick4(int amount)
	{
		lockpick4+=amount;
		if (lockpick4>9999) lockpick4 = 9999;
		lockText4.text = ": " + lockpick4.ToString();
		PlayerPrefs.SetInt("diamondpick",lockpick4);
	}
}
