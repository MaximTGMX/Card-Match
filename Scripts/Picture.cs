using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picture : MonoBehaviour
{
	public AudioClip PressSound;
	public AudioClip PressSound2;
	private Material _firstMaterial;
	private Material _secondMaterial;
	
	private Quaternion CurrentRotation;
	
	[HideInInspector] 
	public bool Revealed = false;
	private PictureManager pictureManager;
	private bool clicked = false;
	private int _index;
	
	private AudioSource _audio;
	private AudioSource _audio2;
	
	public void SetIndex(int id) {_index = id;}
	public int GetIndex(){return _index;}
	
    void Start()
    {
		Revealed = false;
		clicked = false;
		pictureManager = GameObject.Find("[PictureManager]").GetComponent<PictureManager>();
        CurrentRotation = gameObject.transform.rotation;
		_audio = GetComponent<AudioSource>();
		_audio2 = GetComponent<AudioSource>();
		_audio.clip = PressSound;
		_audio2.clip = PressSound2;
    }

    void Update()
    {
        
    }
	
	void OnMouseDown()
	{
		if (clicked==false)
		{
			pictureManager.CurrentPuzzleState = PictureManager.PuzzleState.PuzzleRotating;
			if (!GameSettings.Instance.IsSoundMuted()) _audio.PlayOneShot(PressSound);
			StartCoroutine(LoopRotation(45,false));
			clicked = true;
		}
	}
	
	public void FlipBack()
	{
		if (gameObject.activeSelf)
		{
			pictureManager.CurrentPuzzleState = PictureManager.PuzzleState.PuzzleRotating;
			Revealed = false;
			if (!GameSettings.Instance.IsSoundMuted()) _audio2.PlayOneShot(PressSound2);
			StartCoroutine(LoopRotation(45,true));
		}
	}
	
	IEnumerator LoopRotation(float angle, bool FirstMaterial)
	{
		var rot = 0f;
		const float dir = 1f;
		const float rotSpeed = 180;
		const float rotSpeed1 = 90;
		var startAngle = angle;
		var assigned = false;
		
		if (FirstMaterial)
			while (rot < angle)
			{
				var step = Time.deltaTime * rotSpeed1;
				gameObject.GetComponent<Transform>().Rotate(new Vector3(0,2,0)*step*dir);
				if (rot >= (startAngle-2) && assigned==false)
				{
					ApplyFirstMaterial();
					assigned = true;
				}
				rot += (1*step*dir);
				yield return null;
			}
		else
			while (angle > 0)
			{
				float step = Time.deltaTime * rotSpeed;
				gameObject.GetComponent<Transform>().Rotate(new Vector3(0,2,0)*step*dir);
				angle -= (1*step*dir);
				yield return null;
			}
		
		gameObject.GetComponent<Transform>().rotation = CurrentRotation;
		
		if (!FirstMaterial)
		{
			Revealed = true;
			ApplySecondMaterial();
			pictureManager.CheckPicture();
		}
		else
		{
			pictureManager.PuzzleRevealedNumber = PictureManager.RevealedState.NoRevealed;
			pictureManager.CurrentPuzzleState = PictureManager.PuzzleState.CanRotate;
		}
		clicked = false;
	}
	
	public void SetFirstMaterial(Material mat, string texturePath)
	{
		_firstMaterial = mat;
		_firstMaterial.mainTexture = Resources.Load(texturePath, typeof(Texture2D)) as Texture2D;
	}	
	
	public void SetSecondMaterial(Material mat, string texturePath)
	{
		_secondMaterial = mat;
		_secondMaterial.mainTexture = Resources.Load(texturePath, typeof(Texture2D)) as Texture2D;
	}
	
	public void ApplyFirstMaterial()
	{
		gameObject.GetComponent<Renderer>().material = _firstMaterial;
	}	
	
	public void ApplySecondMaterial()
	{
		gameObject.GetComponent<Renderer>().material = _secondMaterial;
	}
	
	public void Deactivate()
	{
		StartCoroutine(DeactivateCoroutine());
	}
	
	private IEnumerator DeactivateCoroutine()
	{
		Revealed = false;
		yield return new WaitForSeconds(1f);
		gameObject.SetActive(false);
	}
}
