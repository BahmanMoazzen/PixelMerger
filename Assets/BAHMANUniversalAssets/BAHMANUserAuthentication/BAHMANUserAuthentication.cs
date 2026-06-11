using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Threading.Tasks;
public class BAHMANUserAuthentication : MonoBehaviour
{
    /// <summary>
    /// whether to show the log messages in the UI or not, if you want to show the log messages in the UI, you need to assign a Text component to the _logText field, and also set this field to true, otherwise the log messages will be shown in the console only
    /// </summary>
    [SerializeField] bool _isLogActive = true;
    [SerializeField] bool _showWelcomeMessage = true;
    /// <summary>
    /// whether the user is loged in or not, you can use this field to check if the user is loged in or not, and also to show different UI elements based on the user's authentication status, for example, you can show a login button if the user is not loged in, and hide it if the user is loged in
    /// </summary>
    bool _isUserLoged = false;
    /// <summary>
    /// whether a cached session token exist or not, you can use this field to check if a cached session token exist or not, and also to show different UI elements based on the existence of a cached session token, for example, you can show a loading screen while the game is trying to recover the existing login of a player using the cached session token, and hide it if there is no cached session token
    /// </summary>
    bool _isTokenExists = false;
    /// <summary>
    /// a Text component to show the log messages in the UI, you can assign a Text component to this field to show the log messages in the UI, and also set the _isLogActive field to true, otherwise the log messages will be shown in the console only
    /// </summary>
    [SerializeField] Text _logText;
    /// <summary>
    /// Singleton instance of the BAHMANUserAuthentication, you can use this instance to check if the user is loged in or not, and also to get the playerID and other information about the user
    /// </summary>
    public static BAHMANUserAuthentication Instance { get; private set; }
    /// <summary>
    /// triggered when the user loged in successfully, and returns the playerID as string parameter, you can use this playerID to link with other services like cloud save or remote config
    /// </summary>
    public static event UnityAction<string> OnUserLogedIn;
    /// <summary>
    /// whether the user is loged in or not, you can use this property to check if the user is loged in or not, and also to show different UI elements based on the user's authentication status, for example, you can show a login button if the user is not loged in, and hide it if the user is loged in
    /// </summary>
    public bool IsUserLogedIn { get { return _isUserLoged; } }
    /// <summary>
    ///     whether a cached session token exist or not, you can use this property to check if a cached session token exist or not, and also to show different UI elements based on the existence of a cached session token, for example, you can show a loading screen while the game is trying to recover the existing login of a player using the cached session token, and hide it if there is no cached session token
    /// </summary>
    public bool IsTokenExists { get { return _isTokenExists; } }
    public bool IsInitialized { get { return UnityServices.State == ServicesInitializationState.Initialized; } }
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        if (_logText == null)
        {
            _isLogActive = false;
        }
        else
        {
            _logText.gameObject.SetActive(_isLogActive);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {




        await UnityServices.InitializeAsync();
        dlog($"Unity services initialization: {UnityServices.State}");


        _isTokenExists = AuthenticationService.Instance.SessionTokenExists;
        //Shows if a cached session token exist
        dlog($"Cached Session Token Exist: {_isTokenExists}");
        


        // Shows Current profile
        dlog($"Current Profile: {AuthenticationService.Instance.Profile}");
        await _logIn();


    }

    async Task _logIn()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                _isUserLoged = AuthenticationService.Instance.IsSignedIn;
                OnUserLogedIn?.Invoke(AuthenticationService.Instance.PlayerId);
                dlog($"Sign in anonymously succeeded! Is user loged: {_isUserLoged}");
                dlog($"PlayedID: {AuthenticationService.Instance.PlayerId}");
                //dlog($"Access Token: {AuthenticationService.Instance.AccessToken}");
                dlog($"Current Profile: {AuthenticationService.Instance.Profile}");
                //dlog($"Current Session Token: {AuthenticationService.Instance.SessionToken}");

                if (_isTokenExists && _showWelcomeMessage)
                {
                    BAHMANMessageBoxManager.Instance._ShowMessage(A.Tags.WelcomeBack);
                }

            }
            catch (RequestFailedException ex)
            {
                dlog($"Sign in anonymously failed with error code: {ex.ErrorCode}");
                dlog($"{ex.GetType().Name}: {ex.Message}");
            }
        }
    }
    private void OnDestroy()
    {
        AuthenticationService.Instance.SignOut();
        dlog("User signed out and BAHMANUserAuthentication destroyed.");
    }
    void dlog(string iMessage)
    {
        if (_isLogActive)
        {
            _logText.text += "\n" + iMessage;
        }
        Debug.Log(iMessage);
    }
}
