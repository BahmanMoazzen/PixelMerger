using UnityEngine;
using LootLocker.Requests;
using UnityEngine.UI;
using UnityEngine.Events;

public class LootLockerRankingManager : MonoBehaviour
{
    const string SAVETAG = "LootLockerRankingManagerSaveTag";
    [SerializeField] GameObject _submitPanel, _rankPanel, _rankItemParent;
    [SerializeField] GameObject _rankItemPrefab;
    [SerializeField] InputField _nameInput;
    [SerializeField] GameObject _noDataGameObject;
    [SerializeField] string[] _leaderBoardKies;
    [SerializeField] int[] _leaderBoardIDs;
    [SerializeField] Button[] _leaderBoardButtons;
    [SerializeField] int _rankCount = 10;
    [SerializeField] GameObject _loadPanel;
    int _memberID;
    int _currentScore;
    int _leaderBoardIndex, _gameLeaderBoardIndex = 0;
    UnityAction _successAction, _failAction;
    public void _SetActiveLeaderBoard(int iLeaderBoardIndex, int iScore)
    {
        _gameLeaderBoardIndex = _leaderBoardIndex = iLeaderBoardIndex;
        _currentScore = iScore;
    }
    public void _ShowLeaderBoardForm()
    {
        _loadPanel.SetActive(true);
        _rankPanel.SetActive(true);
        _leaderBoardButtons[_gameLeaderBoardIndex].Select();
        LootLockerSDKManager.StartGuestSession(SystemInfo.deviceUniqueIdentifier, _loadRanks);
    }
    public void _DifficalityButtonPresses(int iLeaderBoardIndex)
    {
        _LoadRanking(iLeaderBoardIndex, null, null);
    }
    public void _ShowSubmitForm(UnityAction iSubmitSuccess, UnityAction iSubmitFailed)
    {
        _successAction = iSubmitSuccess;
        _failAction = iSubmitFailed;
        _submitPanel.SetActive(true);
        _nameInput.text = PlayerPrefs.GetString(SAVETAG, string.Empty);
    }
    public void _SubmitFormButtonClicked()
    {

    }
    void _loadRanks(LootLockerGuestSessionResponse iResponse)
    {
        foreach (Transform t in _rankItemParent.transform)
        {
            Destroy(t.gameObject);
        }
        _noDataGameObject.SetActive(true);

        if (iResponse.statusCode == 200)
        {
            _memberID = iResponse.player_id;
            
            LootLockerSDKManager.GetMemberRank(_leaderBoardKies[_leaderBoardIndex], _memberID.ToString(), (memberResponse) =>
            {
                if (memberResponse.success)
                {
                    if (memberResponse.rank == 0)
                    {
                        Debug.Log("Upload score to see centered");
                        _failAction?.Invoke();
                        _endProcess();
                        return;
                    }

                    int playerRank = memberResponse.rank;
                    int after = playerRank < 6 ? 0 : playerRank - 5;

                    LootLockerSDKManager.GetScoreList(_leaderBoardKies[_leaderBoardIndex], _rankCount, after, (response) =>
                    {

                        if (response.success)
                        {
                            if (response.items.Length > 0)
                            {
                                _noDataGameObject.SetActive(false);
                                _leaderBoardButtons[_leaderBoardIndex].Select();
                                for (int i = 0; i < response.items.Length; i++)
                                {
                                    Instantiate(_rankItemPrefab, _rankItemParent.transform)
                                    .GetComponent<LootLockerRankItemController>()._LoadRank(response.items[i], _memberID);
                                }
                                _successAction?.Invoke();
                            }
                            else
                            {
                                _failAction?.Invoke();
                            }
                            
                        }
                        else
                        {
                            _failAction?.Invoke();
                            Debug.Log("Error updating Top 10 leaderboard");
                        }
                        _endProcess() ;
                    });
                }
                else
                {
                    _failAction?.Invoke();
                    _endProcess();
                }
            });
        }
        else
        {
            _failAction?.Invoke();
            _endProcess();

        }
        


    }
    public void _LoadRanking(int iLeaderBoardIndex,UnityAction iSuccessAction,UnityAction iFailAction)
    {
        _successAction = iSuccessAction;
        _failAction = iFailAction;
        _rankPanel.SetActive(true);
        _leaderBoardIndex = iLeaderBoardIndex;
        _loadPanel.SetActive(true);
        LootLockerSDKManager.StartGuestSession(SystemInfo.deviceUniqueIdentifier, _loadRanks);

    }
    void _submitScoreSuccess(LootLockerSubmitScoreResponse iResponse)
    {
        if (iResponse.statusCode == 200)
        {
            Debug.Log(iResponse.rank);
            _successAction?.Invoke();
        }
        else
        {
            _failAction?.Invoke();
            Debug.Log("SubmitScoreFailed");
        }
        _endProcess();
    }
    void _setPlayerNameSuccess(PlayerNameResponse iResponse)
    {
        if (iResponse.statusCode == 200)
        {
            LootLockerSDKManager.SubmitScore(_memberID.ToString(), _currentScore, _leaderBoardKies[_leaderBoardIndex], _submitScoreSuccess);
        }
        else
        {
            _failAction?.Invoke();
            _endProcess();
            Debug.Log("Fail to set name");
        }
    }
    void _guestSessionSuccess(LootLockerGuestSessionResponse iResponse)
    {
        if (iResponse.statusCode == 200)
        {
            _memberID = iResponse.player_id;
            LootLockerSDKManager.SetPlayerName(_nameInput.text, _setPlayerNameSuccess);
        }
        else
        {
            _failAction?.Invoke();
            _endProcess();
            Debug.Log("UserConnectionFailed:" + iResponse.errorData.message);
        }
    }
    public void _CloseButtonClicked()
    {
        _failAction?.Invoke();
        _endProcess();
    }

    void _endProcess()
    {
        _failAction = null;
        _successAction = null;
        _loadPanel.SetActive(false);

    }
    public void _SubmitScore()
    {
        
        _loadPanel.SetActive(true);
        PlayerPrefs.SetString(SAVETAG, _nameInput.text);
        _submitPanel.SetActive(false);
        LootLockerSDKManager.StartGuestSession(SystemInfo.deviceUniqueIdentifier, _guestSessionSuccess);

    }
}
