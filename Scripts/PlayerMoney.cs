using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoney : MonoBehaviour
{
	public static PlayerMoney Instance;
	public Text moneyText;
	
	public int money = 0;
	
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
		money = PlayerPrefs.GetInt("money",0);
		moneyText.text = money.ToString();
    }
	
	public void addMoney(int amount)
	{
		money+=amount;
		if (money>999999999) money = 999999999;
		moneyText.text = money.ToString();
		PlayerPrefs.SetInt("money",money);
	}
	
	public void subtractMoney(int amount)
	{
		if (amount > money)
			Debug.Log("Not enough money!");
		else
		{
			money-=amount;
			moneyText.text = money.ToString();
			PlayerPrefs.SetInt("money",money);
		}
	}
}
