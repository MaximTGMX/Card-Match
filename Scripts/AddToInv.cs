using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddToInv : MonoBehaviour
{
	public int amount;
	
	public void ButtonClicked()
	{
		if (PlayerMoney.Instance.money >= amount)
		{
			PlayerMoney.Instance.subtractMoney(amount);
			if (amount==125)
				Inventory.Instance.AddLockPick1(1);
			else if (amount==250)
				Inventory.Instance.AddLockPick2(1);
			else if (amount==375)
				Inventory.Instance.AddLockPick3(1);
			else if (amount==500)
				Inventory.Instance.AddLockPick4(1);
		}
	}
}
