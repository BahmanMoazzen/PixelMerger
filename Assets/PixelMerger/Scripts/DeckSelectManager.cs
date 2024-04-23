using System.Collections;
using UnityEngine;

public class DeckSelectManager : MonoBehaviour
{
    [SerializeField] GameObject _ButtomParentObject;
    //[SerializeField] string _SKUs;
    private IEnumerator Start()
    {
        yield return null;
        //_SKUs = string.Empty;
        for (int i = 0; i < A.GameSetting.AllDecks.Length; i++)
        {
            //_SKUs += A.GameSetting.AllDecks[i].GetSKUName() + ";";
            Instantiate(A.GameSetting.DeckButtonTemplate, _ButtomParentObject.transform).GetComponent<DeckButtomController>()._CreateButtom(A.GameSetting.AllDecks[i],i);
        }
    }
    public void _Back()
    {
        BAHMANBackButtonManager._Instance._ShowMenu();
    }
}
