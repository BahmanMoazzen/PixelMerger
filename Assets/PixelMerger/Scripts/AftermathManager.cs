using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AftermathManager : MonoBehaviour
{

    [SerializeField] Text _bestScore;
    [SerializeField] Text _thisRunScore;
    [SerializeField] GameObject _newBestRecordText, _submitScoreButton;
    [SerializeField] LootLockerRankingManager _rankingManager;
    public void _Back()
    {
        BAHMANBackButtonManager._Instance._ShowMenu();
    }

    IEnumerator Start()
    {
        yield return null;
        _rankingManager._SetActiveLeaderBoard((int)A.Levels.DifficultyLevel, A.Levels.ThisRoundScore);
        _bestScore.text = A.Tools.ScoreToTitle(A.Levels.BestScore);
        _thisRunScore.text = A.Tools.ScoreToTitle(A.Levels.ThisRoundScore);
        if (A.Levels.SetBestScore(A.Levels.ThisRoundScore))
        {
            _newBestRecordText.SetActive(true);
            _submitScoreButton.SetActive(true);
        }
        else
        {
            _newBestRecordText.SetActive(false);
            _submitScoreButton.SetActive(false);
        }
    }
    public void _ShowRanks()
    {
        _rankingManager._LoadRanking((int)A.Levels.DifficultyLevel, _showRankSuccess, _showRankFailed);
    }
    public void _SubmitScore()
    {
        
        _rankingManager._ShowSubmitForm(_submitRankSuccess,_submitRankFailed);
    }
    void _showRankFailed()
    {
        BAHMANMessageBoxManager._INSTANCE._ShowMessage(A.Tags.LootLocker.ShowRankFailed);
    }
    void _showRankSuccess()
    {
        BAHMANMessageBoxManager._INSTANCE._ShowMessage(A.Tags.LootLocker.ShowRankSuccess);
    }
    void _submitRankSuccess()
    {
        _submitScoreButton.SetActive(false);
        BAHMANMessageBoxManager._INSTANCE._ShowMessage(A.Tags.LootLocker.SubmitRankSuccess);
        _ShowRanks();
    }
    void _submitRankFailed()
    {
        BAHMANMessageBoxManager._INSTANCE._ShowMessage(A.Tags.LootLocker.SubmitRankFailed);
        BAHMANMessageBoxManager._INSTANCE._ShowMessage(A.Tags.CheckInternetConnection);
    }
    public void _TryAgain()
    {

        SoundManager._Instance._PlaySound(GameSounds.ButtomClicked);
        BAHMANLoadingManager._INSTANCE._LoadScene(AllScenes.TitleScreenScene);
    }
}
