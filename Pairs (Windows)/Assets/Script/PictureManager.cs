using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureManager : MonoBehaviour
{
    public ClipManager clipManager;
    public Picture PicturePrefab;
    public Transform PicSpawnPosition;
    public Transform WordPosition;
    public Vector2 startPosition = new Vector2(-2.8f, 1f);

    [Space]
    [Header("End Game Screen")]
    public GameObject EndGamePanel;
    public GameObject NewBestScoreText;
    public GameObject YourScoreText;
    public GameObject EndTimeText;
    public Image _image;
    public int cardCont;
    public bool waitForFlip;
    public bool waitForDestroy;
    public bool gameIsPaused;

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
        NoReavealed,
        OneRevealed,
        TwoRevealed
    };

    [HideInInspector]
    public GameState currrentGameState;

    [HideInInspector]
    public PuzzleState currentPuzzleState;

    [HideInInspector]
    public RevealedState puzzleRevealedNumber;

    [HideInInspector]
    public List<Picture> pictureList;

    private Vector2 _offest = new Vector2(1.4f, 1.1f);
    private List<Material> _materialList = new List<Material>();
    private List<string> _texturePathList = new List<string>();
    private List<string> _materialNames = new List<string>();
    private List<Material> _firstMaterial = new List<Material>();
    private List<string> _firstTexturePath = new List<string>();
    private int _firstRevealedPic;
    private int _secondRevealedPic;
    private int _revealdPicNumber = 0;
    private int picToDestroy1;
    private int picToDestroy2;
    private bool _coRoutineStarted = false;
    private int _pairNumber;
    private int _removedPairs;
    private Timer _gameTimer;

    void Start()
    {
        _image = GameObject.Find("Imagen").GetComponent<Image>();
        _image.enabled = false;
        clipManager = GameObject.Find("ClipManager").GetComponent<ClipManager>();
        initVars();
        loadMaterials();
        Vector2 newstartPosition = startPosition;
        currrentGameState = GameState.MovingOnPositions;
        spawnPictureMesh(5, 4, newstartPosition, _offest, true);
        movePicture(5, 4, newstartPosition, _offest);
    }

    void Update()
    {
        if (gameIsPaused == true)
        {
            Time.timeScale = 1f;
            gameIsPaused = false;
        }

        if (currrentGameState == GameState.DeletingPuzzles)
        {
            if (currentPuzzleState == PuzzleState.CanRotate)
            {
                destroyPic();
                checkGameEnd();
            }
        }
        if (currrentGameState == GameState.FlipBack)
        {
            if (currentPuzzleState == PuzzleState.CanRotate && _coRoutineStarted == false)
            {
                StartCoroutine(flipBack());
            }
        }
        if (currrentGameState == GameState.GameEnd)
        {
            if (
                pictureList[_firstRevealedPic].gameObject.activeSelf == false
                && pictureList[_secondRevealedPic].gameObject.activeSelf == false
                && EndGamePanel.activeSelf == false
            )
            {
                showEndGameInformation();
            }
        }
    }

    private void showEndGameInformation()
    {
        EndGamePanel.SetActive(true);
        if (Config.isBestScore())
        {
            NewBestScoreText.SetActive(true);
            YourScoreText.SetActive(true);
        }
        else
        {
            NewBestScoreText.SetActive(false);
            YourScoreText.SetActive(true);
        }
        var timer = _gameTimer.getCurrentTime();
        var minutes = Mathf.Floor(timer / 60);
        var seconds = Mathf.RoundToInt(timer % 60);
        var newText = minutes.ToString("00") + ":" + seconds.ToString("00");
        EndTimeText.GetComponent<Text>().text = newText;
    }

    private bool checkGameEnd()
    {
        if (_removedPairs == _pairNumber && currrentGameState != GameState.GameEnd)
        {
            currrentGameState = GameState.GameEnd;
            _gameTimer.stopTimer();
            Config.playScoreOnBoard(_gameTimer.getCurrentTime());
        }
        return (currrentGameState == GameState.GameEnd);
    }

    public void checkPicture()
    {
        currrentGameState = GameState.Checking;
        _revealdPicNumber = 0;
        for (int id = 0; id < pictureList.Count; id++)
        {
            if (pictureList[id].revealed && _revealdPicNumber < 2)
            {
                if (_revealdPicNumber == 0)
                {
                    _firstRevealedPic = id;
                    _revealdPicNumber++;
                }
                else if (_revealdPicNumber == 1)
                {
                    _secondRevealedPic = id;
                    _revealdPicNumber++;
                }
            }
        }
        if (_revealdPicNumber == 2)
        {
            if (
                pictureList[_firstRevealedPic].getIndex()
                    == pictureList[_secondRevealedPic].getIndex()
                && _firstRevealedPic != _secondRevealedPic
            )
            {
                currrentGameState = GameState.DeletingPuzzles;
                picToDestroy1 = _firstRevealedPic;
                picToDestroy2 = _secondRevealedPic;
            }
            else
            {
                currrentGameState = GameState.FlipBack;
            }
        }
        currentPuzzleState = PuzzleState.CanRotate;
        if (currrentGameState == GameState.Checking)
        {
            currrentGameState = GameState.NoAction;
        }
    }

    public IEnumerator pauseGameToShowCard(string picName)
    {
        gameIsPaused = true;
        var textureFilePath = GameSettings.Instance.GetPuzzleCategoryTextureDirectoryName();
        var category = textureFilePath.Substring(19, 3);
        var texture = textureFilePath + picName + "V";
        _image.sprite = Resources.Load<Sprite>(texture);
        _image.enabled = true;
        clipManager.playSound(picName + category + "A");
        waitForDestroy = true;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(3);
        Time.timeScale = 1f;
        waitForDestroy = false;
        _image.enabled = false;
        gameIsPaused = false;
    }

    private void destroyPic()
    {
        puzzleRevealedNumber = RevealedState.NoReavealed;
        StartCoroutine(pauseGameToShowCard(pictureList[picToDestroy1].getPictureName()));
        pictureList[picToDestroy1].Deactivate();
        pictureList[picToDestroy2].Deactivate();
        _revealdPicNumber = 0;
        _removedPairs++;
        currrentGameState = GameState.NoAction;
        currentPuzzleState = PuzzleState.CanRotate;
    }

    private IEnumerator flipBack()
    {
        _coRoutineStarted = true;
        yield return new WaitForSeconds(0.5f);
        pictureList[_firstRevealedPic].flipBack();
        pictureList[_secondRevealedPic].flipBack();
        pictureList[_firstRevealedPic].revealed = false;
        pictureList[_secondRevealedPic].revealed = false;
        puzzleRevealedNumber = RevealedState.NoReavealed;
        currrentGameState = GameState.NoAction;
        _coRoutineStarted = false;
    }

    private void initVars()
    {
        cardCont = 0;
        waitForFlip = false;
        waitForDestroy = false;
        gameIsPaused = false;
        currrentGameState = GameState.NoAction;
        currentPuzzleState = PuzzleState.CanRotate;
        puzzleRevealedNumber = RevealedState.NoReavealed;
        _firstRevealedPic = -1;
        _secondRevealedPic = -1;
        _revealdPicNumber = 0;
        _pairNumber = 10;
        _removedPairs = 0;
        _gameTimer = GameObject.Find("Main Camera").GetComponent<Timer>();
    }

    private void loadMaterials()
    {
        var materialFilePath = GameSettings.Instance.GetMaterialDirectoryName();
        var textureFilePath = GameSettings.Instance.GetPuzzleCategoryTextureDirectoryName();
        var pairNumber = (int)10;
        const string matBaseName = "Pic";
        string picName = "";
        var firstMaterialName = "Back";
        for (var index = 1; index <= pairNumber; index++)
        {
            picName = matBaseName;
            var currentFilePath = materialFilePath + matBaseName + index;
            picName = picName + index.ToString();
            _materialNames.Add(picName);
            Material mat = Resources.Load(currentFilePath, typeof(Material)) as Material;
            _materialList.Add(mat);
            var currentTextureFilePath = textureFilePath + matBaseName + index;
            _texturePathList.Add(currentTextureFilePath);
        }
        for (var index = 1; index <= pairNumber * 2; index++)
        {
            Material fmat = Resources.Load(materialFilePath + firstMaterialName + index, typeof(Material)) as Material;
            _firstMaterial.Add(fmat);
            _firstTexturePath.Add(textureFilePath + firstMaterialName + index);
        }
    }

    private void spawnPictureMesh(
        int rows,
        int columns,
        Vector2 pos,
        Vector2 offset,
        bool scaleDown
    )
    {
        for (int col_index = 0; col_index < columns; col_index++)
        {
            for (int row_index = 0; row_index < rows; row_index++)
            {
                var tempPicture = (Picture)Instantiate(
                    PicturePrefab,
                    PicSpawnPosition.position,
                    PicturePrefab.transform.rotation
                );
                tempPicture.name = tempPicture.name + 'c' + col_index + 'r' + row_index;
                pictureList.Add(tempPicture);
            }
        }
        ApplyTextures();
    }

    public void ApplyTextures()
    {
        var rndMatIndex = Random.Range(0, _materialList.Count);
        var appliedTimes = new int[_materialList.Count];

        for (int index = 0; index < _materialList.Count; index++)
        {
            appliedTimes[index] = 0;
        }
        int cardNumber = 0;
        foreach (var o in pictureList)
        {
            var randPrevious = rndMatIndex;
            var counter = 0;
            var forceMat = false;
            while (appliedTimes[rndMatIndex] >= 2 || ((randPrevious == rndMatIndex) && !forceMat))
            {
                rndMatIndex = Random.Range(0, _materialList.Count);
                counter++;
                if (counter > 100)
                {
                    for (var j = 0; j < _materialList.Count; j++)
                    {
                        if (appliedTimes[j] < 2)
                        {
                            rndMatIndex = j;
                            forceMat = true;
                        }
                    }
                    if (forceMat == false)
                    {
                        return;
                    }
                }
            }
            o.setFirstMaterial(_firstMaterial[cardNumber], _firstTexturePath[cardNumber]);
            o.applyFirstMaterial();
            o.setSecondMaterial(_materialList[rndMatIndex], _texturePathList[rndMatIndex]);
            //o.applySecondMaterial();
            o.setPictureName(_materialNames[rndMatIndex]);
            o.setIndex(rndMatIndex);
            o.revealed = false;
            appliedTimes[rndMatIndex] += 1;
            forceMat = false;
            cardNumber = cardNumber + 1;
        }
    }

    private void movePicture(int rows, int columns, Vector2 pos, Vector2 offset)
    {
        var index = 0;
        for (var col_index = 0; col_index < columns; col_index++)
        {
            for (int row_index = 0; row_index < rows; row_index++)
            {
                var targetPosition = new Vector3(
                    (pos.x + (offset.x * row_index)),
                    (pos.y - (offset.y * col_index)),
                    0.0f
                );
                StartCoroutine(moveToPosition(targetPosition, pictureList[index]));
                index++;
            }
        }
    }

    private IEnumerator moveToPosition(Vector3 target, Picture obj)
    {
        var randomDis = 7;
        while (obj.transform.position != target)
        {
            obj.transform.position = Vector3.MoveTowards(
                obj.transform.position,
                target,
                randomDis * Time.deltaTime
            );
            yield return 0;
        }
    }
}
