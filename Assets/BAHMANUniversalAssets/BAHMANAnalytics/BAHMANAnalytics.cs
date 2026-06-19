using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;

#region Custom Events


public class BAHMANButtonClickedEvent : Unity.Services.Analytics.Event
{
    public BAHMANButtonClickedEvent() : base("BAHMANButtonClicked") { }

    public string ButtonName { set { SetParameter("ButtonName", value); } }
}

public class BAHMANClaimSelectedEvent : Unity.Services.Analytics.Event
{
    public BAHMANClaimSelectedEvent() : base("BAHMANClaimSelected") { }
    public float Ratio { set { SetParameter("Ratio", value); } }
}

public class BAHMANDeckSelectedEvent : Unity.Services.Analytics.Event
{
    public BAHMANDeckSelectedEvent() : base("BAHMANDeckSelected") { }

    public string DeckName { set { SetParameter("DeckName", value); } }
}

public class BAHMANGameStartedEvent : Unity.Services.Analytics.Event
{
    public BAHMANGameStartedEvent() : base("BAHMANGameStarted") { }

    public string DeckName { set { SetParameter("DeckName", value); } }
}

public class BAHMANGameEndedEvent : Unity.Services.Analytics.Event
{
    public BAHMANGameEndedEvent() : base("BAHMANGameEnded") { }

    public string DeckName { set { SetParameter("DeckName", value); } }
    public float TimePlayed { set { SetParameter("TimePlayed", value); } }
    public int TotalScore { set { SetParameter("TotalScore", value); } }
}

public class BAHMANMergeEvent : Unity.Services.Analytics.Event
{
    public BAHMANMergeEvent() : base("BAHMANMerge") { }

    public int MergeLevel { set { SetParameter("MergeLevel", value); } }
}

public class BAHMANPowerupUsedEvent : Unity.Services.Analytics.Event
{
    public BAHMANPowerupUsedEvent() : base("BAHMANPowerupUsed") { }

    public string PowerupName { set { SetParameter("PowerupName", value); } }
}


#endregion


public class BAHMANAnalytics : MonoBehaviour
{
    public static BAHMANAnalytics Instance;

    async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LogButtonClicked(string buttonName)
    {
        var evt = new BAHMANButtonClickedEvent
        {
            ButtonName = buttonName
        };

        AnalyticsService.Instance.RecordEvent(evt);
        Debug.Log("LogButtonClicked");
        _flush();
    }

    public void LogClaimSelected(float iRatio)
    {
        var evt = new BAHMANClaimSelectedEvent()
        {
            Ratio = iRatio
        };
        AnalyticsService.Instance.RecordEvent(evt);
        Debug.Log("LogClaimSelected");
        _flush();
    }

    public void LogDeckSelected(string deckName)
    {
        var evt = new BAHMANDeckSelectedEvent
        {
            DeckName = deckName
        };

        AnalyticsService.Instance.RecordEvent(evt);
        Debug.Log("LogDeckSelected");
        _flush();
    }

    public void LogGameStarted(string deckName)
    {
        var evt = new BAHMANGameStartedEvent
        {
            DeckName = deckName
        };

        AnalyticsService.Instance.RecordEvent(evt);
        Debug.Log("LogGameStarted");
        _flush();
    }

    public void LogGameEnded(string deckName, float timePlayed, int totalScore)
    {
        var evt = new BAHMANGameEndedEvent
        {
            DeckName = deckName,
            TimePlayed = timePlayed,
            TotalScore = totalScore
        };

        AnalyticsService.Instance.RecordEvent(evt);
        Debug.Log("LogGameEnded");
        _flush();
    }

    public void LogMerge(int mergeLevel)
    {
        var evt = new BAHMANMergeEvent
        {
            MergeLevel = mergeLevel
        };

        AnalyticsService.Instance.RecordEvent(evt);
        Debug.Log("LogMerge");
        _flush();
    }

    public void LogPowerupUsed(string powerupName)
    {
        var evt = new BAHMANPowerupUsedEvent
        {
            PowerupName = powerupName
        };

        AnalyticsService.Instance.RecordEvent(evt);
        Debug.Log("LogPowerupUsed");
        _flush();
    }

    void _flush()
    {
#if UNITY_EDITOR
        AnalyticsService.Instance.Flush();
#endif
    }
}


