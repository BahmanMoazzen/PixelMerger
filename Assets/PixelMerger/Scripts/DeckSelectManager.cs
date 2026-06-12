using System.Collections;
using UnityEngine;

public class DeckSelectManager : MonoBehaviour
{
    /// <summary>
    /// this is the exit anim name in the animator controller, when the player select a deck, the exit anim will play, and then the scene will change to the battle scene, so make sure the exit anim is long enough to cover the time of the scene change, otherwise the anim will be cut off by the scene change.
    /// </summary>
    const string EXIT_ANIM_NAME = "DeckSelectPanelExit";
    /// <summary>
    /// the animator controller in the deck select panel, it should have an exit anim with the name "DeckSelectPanelExit", when the player select a deck, the exit anim will play, and then the scene will change to the battle scene, so make sure the exit anim is long enough to cover the time of the scene change, otherwise the anim will be cut off by the scene change.
    /// </summary>
    [SerializeField] Animator _anim;
    /// <summary>
    /// the place to instantiate the deck buttons, it should be a child of the deck select panel, and it should have a layout group component to arrange the buttons, when the player select a deck, the exit anim will play, and then the scene will change to the battle scene, so make sure the exit anim is long enough to cover the time of the scene change, otherwise the anim will be cut off by the scene change.
    /// </summary>
    [SerializeField] GameObject _ButtomParentObject;


    private void Awake()
    {
        DeckButtomController.OnDeckSelected += DeckButtomController_OnDeckSelected;
    }
    private void OnDestroy()
    {
        DeckButtomController.OnDeckSelected -= DeckButtomController_OnDeckSelected;
    }
    private void DeckButtomController_OnDeckSelected()
    {
        _anim.Play(EXIT_ANIM_NAME);
    }

    //[SerializeField] string _SKUs;
    private IEnumerator Start()
    {
        yield return null;
        for (int i = 0; i < A.GameSetting.AllDecks.Length; i++)
        {
            //_SKUs += A.GameSetting.AllDecks[i].GetSKUName() + ";";
            Instantiate(A.GameSetting.DeckButtonTemplate, _ButtomParentObject.transform).GetComponent<DeckButtomController>()._CreateButtom(A.GameSetting.AllDecks[i], i);
        }
    }
    public void _CloseWindow()
    {
        BAHMANSoundManager.Instance._PlaySound(GameSounds.CloseWindow);
    }

    public void _Back()
    {
        BAHMANBackButtonManager._Instance._ShowMenu();
    }
}
