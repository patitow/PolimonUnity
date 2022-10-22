using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class NpcSystem : MonoBehaviour
{
    private HistoryState state;

    [Header("Configurações")]
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] GameObject CutsceneScreen;
    [SerializeField] GameObject GameMemory;

    [Header("Npcs")]
    [SerializeField] List<NpcBase> ListadeNPCS;
    [SerializeField] List<GameObject> NPCS;

    [Header("Npcs")]
    [SerializeField] string ChosenCharacterString;

    public HistoryState State { get => state; set => state = value; }

    void Start()
    {
        ChosenCharacterString = PlayerPrefs.GetString("ChosenCharacterString");
        State = HistoryState.Start;
        StartCoroutine(SetupDialog());
    }

    void Update()
    {
        switch (State)
        {
            case HistoryState.Start:
                CutsceneScreen.SetActive(true);
                break;
            case HistoryState.AposBlocoC:
                CutsceneScreen.SetActive(false);
                break;
            case HistoryState.AposSamuel:
                CutsceneScreen.SetActive(true);
                break;
            case HistoryState.AposProfessores:
                CutsceneScreen.SetActive(false);
                break;
            case HistoryState.Fim:
                CutsceneScreen.SetActive(true);
                break;
            case HistoryState.Busy:
                // do nothing
                break;
        }
    }


    public IEnumerator FalarComNPC(GameObject NPC) {

        foreach (var npc in ListadeNPCS) {
            if (NPC.name == npc.Character.characterName) {
                // npc certo
                switch (State)
                {
                    case HistoryState.Start:
                        yield return ReadDialoglist(npc.Start);
                        break;
                    case HistoryState.AposBlocoC:
                        yield return ReadDialoglist(npc.AposBlocoC);
                        break;
                    case HistoryState.AposSamuel:
                        yield return ReadDialoglist(npc.AposSamuel);
                        break;
                    case HistoryState.AposProfessores:
                        yield return ReadDialoglist(npc.AposProfessores);
                        break;
                    case HistoryState.Fim:
                        yield return ReadDialoglist(npc.Fim);
                        break;
                    case HistoryState.Busy:
                        // do nothing
                        break;
                }
            }
        }
    }

    public IEnumerator SetupDialog()
    {
        /*yield return ReadDialoglist(InitialDialog);

        state = MagaiverState.CharacterSelection;
        yield return CharacterSelection();

        state = MagaiverState.MidDialog;
        yield return ReadDialoglist(MidDialog);

        state = MagaiverState.PolimonSelection;
        yield return PolimonSelection();

        state = MagaiverState.EndDialog;
        yield return ReadDialoglist(EndDialog);

        state = MagaiverState.Busy;
        SceneManager.LoadScene(1);*/
        yield return null;
    }

    private IEnumerator ReadDialoglist(List<string> DialogList)
    {

        foreach (var Dialog in DialogList)
        {
            string newDialog = "";
            newDialog = Dialog.Replace($"<NAME>", $"{ChosenCharacterString}");
            yield return dialogBox.TypeDialog(newDialog);
            yield return WaitForPlayerInput();
        }
    }

    private IEnumerator WaitForPlayerInput()
    {
        bool pressed = false;
        while (!pressed)
        {
            if (Input.GetButtonDown("Fire3") || Input.GetKeyDown(KeyCode.Z))
            {
                pressed = true;
            }
            yield return null;
        }
    }
}
