using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteSound : MonoBehaviour
{
	public Sprite UnMutedFXSprite;
	public Sprite MutedFXSprite;
	
	private Button _button;
	private SpriteState _state;
	
    void Start()
    {
        _button = GetComponent<Button>();
		if (GameSettings.Instance.IsSoundMuted())
		{
			_state.pressedSprite = MutedFXSprite;
			_state.highlightedSprite = MutedFXSprite;
			_button.GetComponent<Image>().sprite = MutedFXSprite;
		}
		else
		{
			_state.pressedSprite = UnMutedFXSprite;
			_state.highlightedSprite = UnMutedFXSprite;
			_button.GetComponent<Image>().sprite = UnMutedFXSprite;			
		}
    }
	
	private void OnGUI()
	{
		if (GameSettings.Instance.IsSoundMuted())
			_button.GetComponent<Image>().sprite = MutedFXSprite;
		else
			_button.GetComponent<Image>().sprite = UnMutedFXSprite;
	}
	
	public void ToggleFXIcon()
	{
		if (GameSettings.Instance.IsSoundMuted())
		{
			_state.pressedSprite = UnMutedFXSprite;
			_state.highlightedSprite = UnMutedFXSprite;
			GameSettings.Instance.Mute(false); 
		}
		else
		{
			_state.pressedSprite = MutedFXSprite;
			_state.highlightedSprite = MutedFXSprite;
			GameSettings.Instance.Mute(true); 
		}	
		_button.spriteState = _state;
	}
}
