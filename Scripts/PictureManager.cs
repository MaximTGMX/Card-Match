using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureManager : MonoBehaviour
{
	public Picture PicturePrefab;
	public Transform PicSpawnPosition;
	public Vector2 startPosition; 
	
	[Space]
	[Header("EndGameScreen")]
	public GameObject EndGamePanel;
	public GameObject Score;
	
	public enum GameState 
	{
		NoAction, 
		MovingOnPositions, 
		DeletingPuzzles, 
		FlipBack, 
		Checking, 
		GameEnd
	};
	
	public enum PuzzleState
	{
		PuzzleRotating,
		CanRotate
	};
	
	public enum RevealedState
	{
		NoRevealed, 
		OneRevealed,
		TwoRevealed,
	};
	
	[HideInInspector]
	public GameState CurrentGameState;
	[HideInInspector]
	public PuzzleState CurrentPuzzleState;
	[HideInInspector]
	public RevealedState PuzzleRevealedNumber;
	
	[HideInInspector]
	public List<Picture> PictureList;
	
	private Vector2 _offset;
	
	private List<Material> _materialList = new List<Material>();
	private List<string> _texturePathList = new List<string>();
	private Material _firstMaterial;
	private string _firstTexturePath;
	
	private int firstRevealedPic;
	private int secondRevealedPic;
	private int thirdRevealedPic;
	private int fourthRevealedPic;
	private int revealedPicNumber = 0;
	private int _pictureToDestroy1;
	private int _pictureToDestroy2;
	private int _pictureToDestroy3;
	private int _pictureToDestroy4;
	private bool coroutineStarted = false;
	
	public int cardNumber=0;
	public int pairNumber=0;
	
	private int _removedPairs=0;
	private int _pairNumber=0;
	
	private Vector3 _newScaleDown;
	
	void Start()
	{
		CurrentGameState = GameState.NoAction;
		CurrentPuzzleState = PuzzleState.CanRotate;
		PuzzleRevealedNumber = RevealedState.NoRevealed;
		revealedPicNumber = 0;
		firstRevealedPic = -1;
		secondRevealedPic = -1;
		thirdRevealedPic = -1;
		fourthRevealedPic = -1;
		_removedPairs = 0;
		
		if (GameSettings.Instance.GetPairNumber() == GameSettings.EPairNumber.E2Pairs)
		{
			pairNumber = 2;
			CurrentGameState = GameState.MovingOnPositions;
		}
		else if (GameSettings.Instance.GetPairNumber() == GameSettings.EPairNumber.E3Pairs) 
		{
			pairNumber = 3;
			CurrentGameState = GameState.MovingOnPositions;
		}
		else if (GameSettings.Instance.GetPairNumber() == GameSettings.EPairNumber.E4Pairs)
		{			
			pairNumber = 4;	
			CurrentGameState = GameState.MovingOnPositions;		
		}
		if (GameSettings.Instance.GetPuzzleCategory() == GameSettings.EPuzzleCategories.Cards2)
			cardNumber = 1;
		else if (GameSettings.Instance.GetPuzzleCategory() == GameSettings.EPuzzleCategories.Cards3)
			cardNumber = 2;		
		else if (GameSettings.Instance.GetPuzzleCategory() == GameSettings.EPuzzleCategories.Cards4)
			cardNumber = 3;		
		else if (GameSettings.Instance.GetPuzzleCategory() == GameSettings.EPuzzleCategories.Cards5)
			cardNumber = 4;		
		else if (GameSettings.Instance.GetPuzzleCategory() == GameSettings.EPuzzleCategories.Cards6)
			cardNumber = 5;		
		else if (GameSettings.Instance.GetPuzzleCategory() == GameSettings.EPuzzleCategories.Cards7)
			cardNumber = 6;		
		else if (GameSettings.Instance.GetPuzzleCategory() == GameSettings.EPuzzleCategories.Cards8)
			cardNumber = 7;		
		else if (GameSettings.Instance.GetPuzzleCategory() == GameSettings.EPuzzleCategories.Cards9)
			cardNumber = 8;
		else if (GameSettings.Instance.GetPuzzleCategory() == GameSettings.EPuzzleCategories.Cards10)
			cardNumber = 9;
		_pairNumber = (cardNumber+1)*pairNumber;
		PosScale();
		LoadMaterials();
		SpawnPictureMesh(cardNumber+1, pairNumber, startPosition, _offset, true);
		MovePicture(cardNumber+1, pairNumber, startPosition, _offset);
	}
	
	public void CheckPicture()
	{
		CurrentGameState = GameState.Checking;
		revealedPicNumber = 0;
		
		for (int id=0;id<PictureList.Count;id++)
			if (PictureList[id].Revealed && revealedPicNumber < pairNumber)
				if (revealedPicNumber==0)
				{
					firstRevealedPic = id;
					revealedPicNumber++;
				}
				else if (revealedPicNumber==1)
				{
					secondRevealedPic = id;
					revealedPicNumber++;
				}				
				else if (revealedPicNumber==2)
				{
					thirdRevealedPic = id;
					revealedPicNumber++;
				}				
				else if (revealedPicNumber==3)
				{
					fourthRevealedPic = id;
					revealedPicNumber++;
				}
				
				
		if (revealedPicNumber == pairNumber) 
		{
			if (pairNumber == 2)
				if (PictureList[firstRevealedPic].GetIndex() == PictureList[secondRevealedPic].GetIndex() && firstRevealedPic != secondRevealedPic)
				{
					CurrentGameState = GameState.DeletingPuzzles;
					_pictureToDestroy1 = firstRevealedPic;
					_pictureToDestroy2 = secondRevealedPic;
				}
				else
					CurrentGameState = GameState.FlipBack;
			else if (pairNumber == 3)
				if (PictureList[firstRevealedPic].GetIndex() == PictureList[secondRevealedPic].GetIndex() && firstRevealedPic != secondRevealedPic &&
				PictureList[secondRevealedPic].GetIndex() == PictureList[thirdRevealedPic].GetIndex() && secondRevealedPic != thirdRevealedPic)
				{
					CurrentGameState = GameState.DeletingPuzzles;
					_pictureToDestroy1 = firstRevealedPic;
					_pictureToDestroy2 = secondRevealedPic;
					_pictureToDestroy3 = thirdRevealedPic;
				}
				else
					CurrentGameState = GameState.FlipBack;
			else if (pairNumber == 4)
				if (PictureList[firstRevealedPic].GetIndex() == PictureList[secondRevealedPic].GetIndex() && firstRevealedPic != secondRevealedPic &&
				PictureList[secondRevealedPic].GetIndex() == PictureList[thirdRevealedPic].GetIndex() && secondRevealedPic != thirdRevealedPic &&
				PictureList[thirdRevealedPic].GetIndex() == PictureList[fourthRevealedPic].GetIndex() && thirdRevealedPic != fourthRevealedPic)
				{
					CurrentGameState = GameState.DeletingPuzzles;
					_pictureToDestroy1 = firstRevealedPic;
					_pictureToDestroy2 = secondRevealedPic;
					_pictureToDestroy3 = thirdRevealedPic;
					_pictureToDestroy4 = fourthRevealedPic;
				}
				else
					CurrentGameState = GameState.FlipBack;
		}
		CurrentPuzzleState = PictureManager.PuzzleState.CanRotate;
		if (CurrentGameState == GameState.Checking) CurrentGameState = GameState.NoAction;
	}
	
	private void DestroyPicture()
	{
		PuzzleRevealedNumber = RevealedState.NoRevealed;
		if (pairNumber == 2)
		{
			PictureList[_pictureToDestroy1].Deactivate();
			PictureList[_pictureToDestroy2].Deactivate();
		}
		else if (pairNumber == 3)
		{
			PictureList[_pictureToDestroy1].Deactivate();
			PictureList[_pictureToDestroy2].Deactivate();			
			PictureList[_pictureToDestroy3].Deactivate();			
		}
		else if (pairNumber == 4)
		{
			PictureList[_pictureToDestroy1].Deactivate();
			PictureList[_pictureToDestroy2].Deactivate();			
			PictureList[_pictureToDestroy3].Deactivate();				
			PictureList[_pictureToDestroy4].Deactivate();				
		}
		revealedPicNumber = 0;
		_removedPairs+=pairNumber;
		CurrentGameState = GameState.NoAction;
		CurrentPuzzleState = PuzzleState.CanRotate;
	}
	
	private IEnumerator FlipBack()
	{
		coroutineStarted = true;
		
		yield return new WaitForSeconds(0.5f);
		
		if (pairNumber == 2)
		{
			PictureList[firstRevealedPic].FlipBack();
			PictureList[secondRevealedPic].FlipBack();
			PictureList[firstRevealedPic].Revealed = false;
			PictureList[secondRevealedPic].Revealed = false;
		}			
		else if (pairNumber == 3)
		{
			PictureList[firstRevealedPic].FlipBack();
			PictureList[secondRevealedPic].FlipBack();
			PictureList[thirdRevealedPic].FlipBack();
			PictureList[firstRevealedPic].Revealed = false;
			PictureList[secondRevealedPic].Revealed = false;			
			PictureList[thirdRevealedPic].Revealed = false;	
		}
		else if (pairNumber == 4)
		{
			PictureList[firstRevealedPic].FlipBack();
			PictureList[secondRevealedPic].FlipBack();
			PictureList[thirdRevealedPic].FlipBack();
			PictureList[fourthRevealedPic].FlipBack();
			PictureList[firstRevealedPic].Revealed = false;
			PictureList[secondRevealedPic].Revealed = false;
			PictureList[thirdRevealedPic].Revealed = false;
			PictureList[fourthRevealedPic].Revealed = false;
		}
		PuzzleRevealedNumber = RevealedState.NoRevealed;
		CurrentGameState = GameState.NoAction;
		
		coroutineStarted = false;
	}
	
	public void PosScale()
	{
		if (pairNumber==2)
		{
			if (cardNumber==1) {startPosition = new Vector2(-1.3f, 1.3f); _newScaleDown = new Vector3(2f, 2.5f, 0.01f); _offset = new Vector2(2.5f, 3f);}
			if (cardNumber==2) {startPosition = new Vector2(-2.5f, 1.3f); _newScaleDown = new Vector3(2f, 2.5f, 0.01f); _offset = new Vector2(2.5f, 3f);}
			if (cardNumber==3) {startPosition = new Vector2(-3.7f, 1.3f); _newScaleDown = new Vector3(2f, 2.5f, 0.01f); _offset = new Vector2(2.5f, 3f);}
			if (cardNumber==4) {startPosition = new Vector2(-4.9f, 1.3f); _newScaleDown = new Vector3(2f, 2.5f, 0.01f); _offset = new Vector2(2.5f, 3f);}
			if (cardNumber==5) {startPosition = new Vector2(-6.1f, 1.3f); _newScaleDown = new Vector3(2f, 2.5f, 0.01f); _offset = new Vector2(2.5f, 3f);}
			if (cardNumber==6) {startPosition = new Vector2(-6.4f, 1.3f); _newScaleDown = new Vector3(1.7f, 2.2f, 0.01f); _offset = new Vector2(2.2f, 2.7f);}
			if (cardNumber==7) {startPosition = new Vector2(-6.7f, 1.3f); _newScaleDown = new Vector3(1.4f, 1.9f, 0.01f); _offset = new Vector2(1.9f, 2.4f);}
			if (cardNumber==8) {startPosition = new Vector2(-6.3f, 1.6f); _newScaleDown = new Vector3(1.1f, 1.6f, 0.01f); _offset = new Vector2(1.6f, 2.1f);}
			if (cardNumber==9) {startPosition = new Vector2(-7.2f, 1.6f); _newScaleDown = new Vector3(1.1f, 1.6f, 0.01f); _offset = new Vector2(1.6f, 2.1f);}
		}
		else if (pairNumber==3)
		{
			if (cardNumber==1) {startPosition = new Vector2(-1.3f, 2.5f); _newScaleDown = new Vector3(2f, 2.5f, 0.01f); _offset = new Vector2(2.5f, 3f);}
			if (cardNumber==2) {startPosition = new Vector2(-2.5f, 2.5f); _newScaleDown = new Vector3(2f, 2.5f, 0.01f); _offset = new Vector2(2.5f, 3f);}
			if (cardNumber==3) {startPosition = new Vector2(-3.7f, 2.5f); _newScaleDown = new Vector3(2f, 2.5f, 0.01f); _offset = new Vector2(2.5f, 3f);}
			if (cardNumber==4) {startPosition = new Vector2(-4.9f, 2.5f); _newScaleDown = new Vector3(2f, 2.5f, 0.01f); _offset = new Vector2(2.5f, 3f);}
			if (cardNumber==5) {startPosition = new Vector2(-6.1f, 2.5f); _newScaleDown = new Vector3(2f, 2.5f, 0.01f); _offset = new Vector2(2.5f, 3f);}
			if (cardNumber==6) {startPosition = new Vector2(-6.4f, 2.2f); _newScaleDown = new Vector3(1.7f, 2.2f, 0.01f); _offset = new Vector2(2.2f, 2.7f);}
			if (cardNumber==7) {startPosition = new Vector2(-6.7f, 1.9f); _newScaleDown = new Vector3(1.4f, 1.9f, 0.01f); _offset = new Vector2(1.9f, 2.4f);}
			if (cardNumber==8) {startPosition = new Vector2(-6.3f, 1.6f); _newScaleDown = new Vector3(1.1f, 1.6f, 0.01f); _offset = new Vector2(1.6f, 2.1f);}
			if (cardNumber==9) {startPosition = new Vector2(-7.2f, 1.6f); _newScaleDown = new Vector3(1.1f, 1.6f, 0.01f); _offset = new Vector2(1.6f, 2.1f);}
		}	
		else
		{
			if (cardNumber==1) {startPosition = new Vector2(-0.9f, 2.5f); _newScaleDown = new Vector3(1.1f, 1.6f, 0.01f); _offset = new Vector2(1.6f, 2.1f);}
			if (cardNumber==2) {startPosition = new Vector2(-1.6f, 2.5f); _newScaleDown = new Vector3(1.1f, 1.6f, 0.01f); _offset = new Vector2(1.6f, 2.1f);}
			if (cardNumber==3) {startPosition = new Vector2(-2.3f, 2.5f); _newScaleDown = new Vector3(1.1f, 1.6f, 0.01f); _offset = new Vector2(1.6f, 2.1f);}
			if (cardNumber==4) {startPosition = new Vector2(-3f, 2.5f); _newScaleDown = new Vector3(1.1f, 1.6f, 0.01f); _offset = new Vector2(1.6f, 2.1f);}
			if (cardNumber==5) {startPosition = new Vector2(-3.8f, 2.5f); _newScaleDown = new Vector3(1.1f, 1.6f, 0.01f); _offset = new Vector2(1.6f, 2.1f);}
			if (cardNumber==6) {startPosition = new Vector2(-4.6f, 2.5f); _newScaleDown = new Vector3(1.1f, 1.6f, 0.01f); _offset = new Vector2(1.6f, 2.1f);}
			if (cardNumber==7) {startPosition = new Vector2(-5.4f, 2.5f); _newScaleDown = new Vector3(1.1f, 1.6f, 0.01f); _offset = new Vector2(1.6f, 2.1f);}
			if (cardNumber==8) {startPosition = new Vector2(-6.3f, 2.5f); _newScaleDown = new Vector3(1.1f, 1.6f, 0.01f); _offset = new Vector2(1.6f, 2.1f);}
			if (cardNumber==9) {startPosition = new Vector2(-7.2f, 2.5f); _newScaleDown = new Vector3(1.1f, 1.6f, 0.01f); _offset = new Vector2(1.6f, 2.1f);}			
		}
	}
	
	private void LoadMaterials()
	{
		var materialFilePath = GameSettings.Instance.GetMaterialDirectoryName();
		var textureFilePath = GameSettings.Instance.GetPuzzleCategoryDirectoryName();
		const string matBaseName = "FrontCard";
		var firstMaterialName = "BackCard";
		
		for (int index=1;index<=cardNumber+1;index++)
		{
			var currentFilePath = materialFilePath + matBaseName + index;
			Material mat = Resources.Load(currentFilePath, typeof(Material)) as Material;
			_materialList.Add(mat);
			
			var currentTextureFilePath = textureFilePath + matBaseName + index;
			_texturePathList.Add(currentTextureFilePath);
		}
		
		_firstTexturePath = textureFilePath + firstMaterialName;
		_firstMaterial = Resources.Load(materialFilePath + firstMaterialName, typeof(Material)) as Material;
	}
	
	void Update()
	{
		if (CurrentGameState == GameState.DeletingPuzzles)
			if (CurrentPuzzleState == PuzzleState.CanRotate)
			{
				DestroyPicture();
				CheckGameEnd();
			}
		
		if (CurrentGameState == GameState.FlipBack && !coroutineStarted)
			if (CurrentPuzzleState == PuzzleState.CanRotate)
				StartCoroutine(FlipBack());
		if (CurrentGameState==GameState.GameEnd)
			if (!EndGamePanel.activeSelf)
				if (!PictureList[firstRevealedPic].gameObject.activeSelf && !PictureList[secondRevealedPic].gameObject.activeSelf)
					ShowEndGameInfo();
	}
	
	private bool CheckGameEnd()
	{
		if (_removedPairs==_pairNumber)
			CurrentGameState = GameState.GameEnd;
		return (CurrentGameState==GameState.GameEnd);
	}
	
	private void ShowEndGameInfo()
	{
		EndGamePanel.SetActive(true);
		
		var money = (cardNumber+1)*pairNumber;
		if (pairNumber==3) money+=20;
		else if (pairNumber==4) money+=40;
		var newText = "+" + money.ToString();
		Score.GetComponent<Text>().text = newText;
		
		PlayerMoney.Instance.addMoney(money);
	}
	
	private void SpawnPictureMesh(int row, int columns, Vector2 Pos, Vector2 offset, bool scaleDown)
	{
		for (int i=0;i<columns;i++)
			for (int j=0;j<row;j++)
			{
				var tempPicture = (Picture)Instantiate(PicturePrefab, PicSpawnPosition.position, PicturePrefab.transform.rotation);
				if (scaleDown) tempPicture.transform.localScale = _newScaleDown;
				tempPicture.name = tempPicture.name + 'c' + i + 'r' + j;
				PictureList.Add(tempPicture);
			}
		ApplyTextures();
	}
	
	public void ApplyTextures()
	{
		var randMaterialIndex = Random.Range(0, _materialList.Count);
		var AppliedTimes = new int[_materialList.Count];
		
		for (int k=0;k<_materialList.Count;k++) AppliedTimes[k] = 0;
		foreach(var o in PictureList)
		{
			var randPrevious = randMaterialIndex;
			var counter = 0;
			var forceMat = false;
			
			while (AppliedTimes[randMaterialIndex]>=pairNumber || ((randPrevious == randMaterialIndex) && !forceMat))
			{
				randMaterialIndex = Random.Range(0, _materialList.Count);
				counter++;
				if (counter>100)
				{
					for (var l=0;l<_materialList.Count;l++)
						if (AppliedTimes[l]<pairNumber) {randMaterialIndex = l; forceMat = true;}
					if (forceMat == false) return;
				}
			}
			
			o.SetFirstMaterial(_firstMaterial, _firstTexturePath);
			o.ApplyFirstMaterial();
			o.SetSecondMaterial(_materialList[randMaterialIndex], _texturePathList[randMaterialIndex]);
			o.SetIndex(randMaterialIndex);
			o.Revealed = false;
			AppliedTimes[randMaterialIndex]++;
			forceMat = false;
		}
	}
	
	private void MovePicture(int row, int columns, Vector2 Pos, Vector2 offset)
	{
		var index=0;
		for (var i=0;i<columns;i++)
			for (var j=0;j<row;j++)
			{
				var targetPosition = new Vector3((Pos.x+(offset.x*j)), (Pos.y-(offset.y*i)), 0.0f);
				StartCoroutine(MoveToPosition(targetPosition, PictureList[index]));
				index++;
			}
	}
	
	private IEnumerator MoveToPosition(Vector3 target, Picture obj)
	{
		var randomDis = 10;
		while (obj.transform.position!=target)
		{
			obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, randomDis*Time.deltaTime);
			yield return 0;
		}
	}
}
