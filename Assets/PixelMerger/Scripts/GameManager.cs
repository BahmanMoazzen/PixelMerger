using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// the position on the screen
    /// </summary>
    [SerializeField] Transform _topLeft, _bottomRight, _pixelSpawnPoint;

    /// <summary>
    /// gap between each drop
    /// </summary>
    [SerializeField] float _dropInterval = .5f;
    /// <summary>
    /// the text to show score
    /// </summary>
    [SerializeField] Text _scoreText;
    /// <summary>
    /// the total score to show on score board
    /// </summary>
    int _totalScore = 0;
    /// <summary>
    /// all the available deadlines - esay - medium - hard
    /// </summary>
    [SerializeField]
    GameObject[] _deadLines;
    /// <summary>
    /// the total arena box collider
    /// </summary>
    [SerializeField] BoxCollider2D _arena;
    /// <summary>
    /// the line to show at the deadline
    /// </summary>
    LineRenderer _line;
    /// <summary>
    /// the particle system to show on merge point
    /// </summary>
    [SerializeField] GameObject _particle;

    /// <summary>
    /// current pixel GameBobject
    /// </summary>
    GameObject _currentPixel;
    /// <summary>
    /// indicates whether the pixel in dropping or not
    /// </summary>
    bool _isDroping = false;

    /// <summary>
    /// calculated in update method to check if the mouse pointer is inside the arena
    /// </summary>
    bool _insideArena = false;
    /// <summary>
    /// all the pixel in the arena
    /// </summary>
    List<GameObject> _levelPixels;
    /// <summary>
    /// the magnetitude by with the force will apply when tilt is pressed
    /// </summary>
    [SerializeField] float _tiltMagnetute;

    /// <summary>
    /// if hammer activated
    /// </summary>
    bool _isHammerActivated = false;
    /// <summary>
    /// if the adver is showing
    /// </summary>
    bool _isAdverShowing = false;
    /// <summary>
    /// if the purchase process is being done
    /// </summary>
    bool _isShopShowing = false;
    /// <summary>
    /// is game overed
    /// </summary>
    bool _isGameOvered = false;
    /// <summary>
    /// the hammer object to show on screen
    /// </summary>
    [SerializeField] GameObject _hammerObject;
    /// <summary>
    /// the "Hammer Mode" text to actictivate on screen
    /// </summary>
    [SerializeField] GameObject _hammerText;
    /// <summary>
    /// the global light to reduce whenever the hammer selected
    /// </summary>
    [SerializeField] Light2D _globalLight;
    /// <summary>
    /// the current savable item to manipulate
    /// </summary>
    SaveableItem _activeItem;
    /// <summary>
    /// level starup routine
    /// </summary>
    /// <returns>nothing</returns>
    IEnumerator Start()
    {
        MergersController._isAnnounced = false;
        _levelPixels = new List<GameObject>();
        _deadLines[(int)A.Levels.DifficultyLevel].SetActive(true);
        _line = _deadLines[(int)A.Levels.DifficultyLevel].GetComponent<LineRenderer>();
        //_latencies = new Dictionary<int, float>();
        _scoreText.text = A.Tools.ScoreToTitle(_totalScore);
        _line.SetPosition(0, new Vector2(_topLeft.position.x, _deadLines[(int)A.Levels.DifficultyLevel].transform.position.y));
        _line.SetPosition(1, new Vector2(_bottomRight.position.x, _deadLines[(int)A.Levels.DifficultyLevel].transform.position.y));
        yield return null;
        _loadPixel();


    }
    public void _EndGame()
    {
        if (!_isGameOvered)
        {
            _isGameOvered = true;
            A.Levels.ThisRoundScore = _totalScore;
            SoundManager._Instance._PlaySound(GameSounds.GameOver);
            BAHMANLoadingManager._INSTANCE._LoadScene(AllScenes.AftermathScene);
        }

    }
    public void _ItemClicked(SaveableItem iItem)
    {
        _activeItem = iItem;
        if (iItem._HaveStock)
        {
            switch (iItem._Tag)
            {
                case "Hammer":
                    _UseHammer();
                    break;
                case "TiltLeft":
                    _Tilt(-1);
                    iItem._ChangeAmount(-1, false);
                    break;
                case "TiltRight":
                    iItem._ChangeAmount(-1, false);
                    _Tilt(1);
                    break;
                case "UniColor":
                    iItem._ChangeAmount(-1, false);
                    _LoadUnicolor();
                    break;
            }
        }
        else
        {

            BAHMANMessageBoxManager._INSTANCE?._ShowYesNoBox(A.Tags.OutOfStockTag, A.Tags.BuyOneTag, _itemClickedConfirmShowAd);
        }
    }
    void _itemClickedConfirmShowAd()
    {
        BAHMANAdManager._Instance._BuySKU(_activeItem._SKU, AdManager__OnAdRewarded, AdManager__OnAdFailed);
        _isShopShowing = true;
    }

    private void AdManager__OnAdFailed()
    {
        BAHMANMessageBoxManager._INSTANCE?._ShowMessage(A.Tags.PurchaseFailedTag);
        BAHMANMessageBoxManager._INSTANCE._ShowMessage(A.Tags.CheckInternetConnection);
        _isShopShowing = false;

    }

    private void AdManager__OnAdRewarded()
    {
        BAHMANMessageBoxManager._INSTANCE?._ShowMessage(A.Tags.PurchaseSuccessTag);
        _activeItem._ResetAmount();
        _isShopShowing = false;

    }

    private void MergersController_OnMerge(GameObject iFirstPixel, GameObject iSeconPixel, GameObject iNewPixel)
    {
        _totalScore += A.Pixels.PixelScore(iFirstPixel.GetComponent<MergersController>()._MergerInfo.MergerOrder);
        _scoreText.text = A.Tools.ScoreToTitle(_totalScore);
        _levelPixels.Remove(iFirstPixel);
        _levelPixels.Remove(iSeconPixel);
        if (iNewPixel != null)
        {
            Instantiate(_particle, iNewPixel.transform.position, Quaternion.identity);
            _levelPixels.Add(iNewPixel);
        }
    }

    void _loadPixel()
    {
        GameObject go =
        Instantiate(A.Pixels.PixelSkeleton
            , _pixelSpawnPoint.position //new Vector3((_topLeft.position.x + _bottomRight.position.x) / 2, _topLeft.position.y, 0)
            , Quaternion.identity);
        go.GetComponent<MergersController>()._LoadPixel(A.Pixels.RandomPixel(), Vector2.zero, 0, false);
        _currentPixel = go;

    }
    public void _LoadUnicolor()
    {
        Destroy(_currentPixel);
        GameObject go =
        Instantiate(A.Pixels.PixelSkeleton
            , _pixelSpawnPoint.position // new Vector3((_topLeft.position.x + _bottomRight.position.x) / 2, _topLeft.position.y, 0)
            , Quaternion.identity);
        go.GetComponent<MergersController>()._LoadUnicolor();
        _currentPixel = go;
    }
    /// <summary>
    /// use hammer UI button is connected with this method
    /// </summary>
    public void _UseHammer()
    {
        _isHammerActivated = !_isHammerActivated;
        if (_isHammerActivated)
        {
            MergersController.OnPixelClicked += MergersController_OnPixelClicked;
            _hammerObject.SetActive(true);
            _hammerText.SetActive(true);
            _globalLight.intensity = .3f;
        }
        else
        {
            _hammerObject.SetActive(false);
            _hammerText.SetActive(false);
            MergersController.OnPixelClicked -= MergersController_OnPixelClicked;
            _globalLight.intensity = 1f;
        }


    }
    /// <summary>
    /// every pixel clicked is trigger this method
    /// </summary>
    /// <param name="iPixel">the pixel with being clicked</param>
    private void MergersController_OnPixelClicked(GameObject iPixel)
    {

        MergersController.OnPixelClicked -= MergersController_OnPixelClicked;
        _activeItem._ChangeAmount(-1, false);
        _levelPixels.Remove(iPixel);
        Destroy(iPixel);
        StartCoroutine(_deactivateHammerRoutine());
    }
    /// <summary>
    /// deactivates hammer on the game
    /// </summary>
    /// <returns>nothing</returns>
    IEnumerator _deactivateHammerRoutine()
    {
        yield return new WaitForSeconds(.3f);
        _globalLight.intensity = 1f;
        _isHammerActivated = false;
        _hammerObject.SetActive(false);
        _hammerText.SetActive(false);
    }

    private void Update()
    {
        _insideArena = _arena.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (_isHammerActivated)
        {
            Vector2 newpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _hammerObject.transform.position = newpos;
        }
        if (Input.GetMouseButton(0) && !_isDroping && _insideArena && !_isHammerActivated && !_isAdverShowing && !_isShopShowing)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _currentPixel.transform.position = new Vector2(mousePos.x, _currentPixel.transform.position.y);
        }
        if (Input.GetMouseButtonUp(0) && !_isDroping && _insideArena && !_isHammerActivated && !_isAdverShowing && !_isShopShowing)
        {
            _isDroping = true;
            StartCoroutine(_dropDownPixel());
        }
    }
    /// <summary>
    /// enables the gravity for pixel to drops
    /// </summary>
    /// <returns>nothing</returns>
    IEnumerator _dropDownPixel()
    {
        SoundManager._Instance._PlaySound(GameSounds.Throw);
        yield return null;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _currentPixel.transform.position = new Vector3(Mathf.Clamp(mousePos.x, _topLeft.position.x, _bottomRight.position.x), _currentPixel.transform.position.y, 0);
        _currentPixel.GetComponent<MergersController>()._Drop();
        yield return new WaitForSeconds(_dropInterval);
        _isDroping = false;
        _levelPixels.Add(_currentPixel);
        _loadPixel();
    }
    /// <summary>
    /// the back button action on the screen
    /// </summary>
    public void _Back()
    {
        BAHMANBackButtonManager._Instance._ShowMenu();
    }



    /// <summary>
    /// adds force to all pixeld in the arena
    /// </summary>
    /// <param name="iDirection">the direction to add force</param>
    public void _Tilt(int iDirection)
    {
        foreach (GameObject go in _levelPixels)
        {
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(iDirection, -1f) * _tiltMagnetute * rb.mass);
        }

    }
    /// <summary>
    /// listens to the requierd listeners of the game
    /// </summary>
    private void OnEnable()
    {
        MergersController.OnMerge += MergersController_OnMerge;
        MergersController.OnContactLatencyExceed += MergersController_OnContactLatencyExceed;
        //DeadlineController.OnContactLatency += MergersController_OnContactLatency;
        //DeadlineController.OnResetLatency += MergersController_OnResetLatency;
    }

    private void MergersController_OnContactLatencyExceed()
    {
        SoundManager._Instance._PlaySound(GameSounds.GameOver);
        _EndGame();
    }



    /// <summary>
    /// removes the listeners from any game object listened before
    /// </summary>
    private void OnDisable()
    {
        MergersController.OnMerge -= MergersController_OnMerge;
        MergersController.OnContactLatencyExceed -= MergersController_OnContactLatencyExceed;
    }
    /// <summary>
    /// resets the top pixel bound to be spawn to zero to reset the entire game
    /// </summary>
    private void OnDestroy()
    {
        A.Pixels.topPixelBound = 0;
    }



}

