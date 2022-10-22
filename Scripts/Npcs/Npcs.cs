using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class Npcs : MonoBehaviour
{
    public GameObject BattleScene;
    public GameObject NpcScreen;
    public CharacterManager GameMemory;
    public NpcBase Npc;
    public TextMeshProUGUI TextBox;
    public TextMeshProUGUI NameBox;
    public bool playerIsClose;
    public int letterPerSecond;
    public Sprite npcImage;


    [SerializeField] private HistoryState state;
    private int letterPerSecond2x;
    private int currentDialog;
    private bool isRunning;
    private bool isNotInBattle;
    public bool haveBeenDefeated;
    private string ChosenCharacterString;

    public List<Polimon> NpcParty;
    public List<PolimonBase> NpcPartyBase;
    public PolimonBaseDatabase PolimonBaseDatabase;
    int timestalked = 0;

    public void Start()
    {
        TurnoffDialog();
        state = GameMemory.State;
        letterPerSecond2x = 2 * letterPerSecond;
        ChosenCharacterString = PlayerPrefs.GetString("ChosenCharacterString");
        isNotInBattle = !GameMemory.Player.GetComponent<PlayerMovement>().isInBattle;
    }

    private void Update()
    {
        state = GameMemory.State;
        isNotInBattle = !GameMemory.Player.GetComponent<PlayerMovement>().isInBattle;
        if ((Input.GetButtonDown("Fire3") || Input.GetKeyDown(KeyCode.Z)) && playerIsClose && NpcScreen != null && !isRunning && isNotInBattle)
        {
            float distx = GameObject.Find("Player").transform.position.x - transform.position.x;
            float disty = GameObject.Find("Player").transform.position.y - transform.position.y;
            Animator anim = GetComponent<Animator>();
            anim.SetFloat("Vertical", disty);
            anim.SetFloat("Horizontal", distx);
            StartCoroutine(StateMachine());
        }
        Debug.Log(timestalked);  

    }

    public IEnumerator StateChange(HistoryState State) {
        GameMemory.State = State;
        state = State;
        Debug.Log(State);
        yield return null;
    }

    public IEnumerator StateMachine() {
        switch (state)
        {
            case HistoryState.Start:
                NpcScreen.SetActive(true);
                NameBox.text = Npc.Character.characterName;
                Debug.Log("Start");
                if (Npc.Character.characterName == "Wylliams" && isNotInBattle) {
                    yield return StateChange(HistoryState.AposBlocoC);
                    yield return ReadDialoglist(Npc.getList(HistoryState.Start));
                } else if ((Npc.Character.characterName == "Magaiver" || Npc.Character.characterName == "Samuel")&& isNotInBattle)
                    yield return ReadDialoglist(Npc.getList(state));
                break;

            case HistoryState.AposBlocoC:
                NpcScreen.SetActive(true);
                NameBox.text = Npc.Character.characterName;
                
                if (Npc.Character.characterName == "Samuel" && isNotInBattle)
                {
                    if (haveBeenDefeated)
                    {
                        GameMemory.ChaveSaladeEcomp = true;
                        GameMemory.State = HistoryState.AposSamuel;
                    }
                    NpcParty.Clear();
                    for (int i = 0; i < NpcPartyBase.Count; i++)
                    {
                        Polimon PolimonDaVez = ScriptableObject.CreateInstance("Polimon") as Polimon;
                        PolimonDaVez.init(NpcPartyBase[i], Random.Range(BattleScene.GetComponent<BattleSystem>().MediumPartyLevel(GameMemory.Party) - 3, BattleScene.GetComponent<BattleSystem>().MediumPartyLevel(GameMemory.Party) + 3));
                        PolimonDaVez.name = PolimonDaVez.Name; //seta o nome no inspetor para melhor visualização
                        NpcParty.Add(PolimonDaVez);
                    }
                    yield return ReadDialoglistWhoCallsBattle(Npc.getList(HistoryState.AposBlocoC));
                }

                if (Npc.Character.characterName == "Wylliams" && isNotInBattle)
                {
                    timestalked++;
                    if (haveBeenDefeated)
                    {
                        yield return StateChange(HistoryState.Busy);
                        GameMemory.PodeSubirAEscada = true;
                    }
                    if (!haveBeenDefeated && timestalked <= 2)
                    {
                        yield return ReadDialoglist(Npc.getList(state));
                        Debug.Log("Intermedio");
                    }
                    else {
                        NpcParty.Clear();
                        for (int i = 0; i < NpcPartyBase.Count; i++)
                        {
                            Polimon PolimonDaVez = ScriptableObject.CreateInstance("Polimon") as Polimon;
                            PolimonDaVez.init(NpcPartyBase[i], Random.Range(20, 50));
                            PolimonDaVez.name = PolimonDaVez.Name; //seta o nome no inspetor para melhor visualização
                            NpcParty.Add(PolimonDaVez);
                        }
                        Debug.Log("Era pra ter rodado");
                        yield return ReadDialoglistWhoCallsBattle(Npc.getList(HistoryState.AposBlocoC));
                    }
                }
                if (Npc.Character.characterName == "Magaiver" && isNotInBattle) {
                    yield return ReadDialoglist(Npc.getList(state));
                }
                break;

            case HistoryState.Busy:
                if (Npc.Character.characterName == "Samuel" && isNotInBattle)
                {
                    if (haveBeenDefeated)
                    {
                        GameMemory.ChaveSaladeEcomp = true;
                        GameMemory.State = HistoryState.AposProfessores;
                    }
                    NpcParty.Clear();
                    for (int i = 0; i < NpcPartyBase.Count; i++)
                    {
                        Polimon PolimonDaVez = ScriptableObject.CreateInstance("Polimon") as Polimon;
                        PolimonDaVez.init(NpcPartyBase[i], Random.Range(BattleScene.GetComponent<BattleSystem>().MediumPartyLevel(GameMemory.Party) - 3, BattleScene.GetComponent<BattleSystem>().MediumPartyLevel(GameMemory.Party) + 3));
                        PolimonDaVez.name = PolimonDaVez.Name; //seta o nome no inspetor para melhor visualização
                        NpcParty.Add(PolimonDaVez);
                    }
                    yield return ReadDialoglistWhoCallsBattle(Npc.getList(HistoryState.AposBlocoC));
                }
                else if ((Npc.Character.characterName == "Magaiver" || Npc.Character.characterName == "Wylliams") && isNotInBattle)
                    yield return ReadDialoglist(Npc.getList(HistoryState.AposBlocoC));
                break;

            case HistoryState.AposSamuel:
                if (Npc.Character.characterName == "Wylliams" && isNotInBattle)
                {
                    if (haveBeenDefeated)
                    {
                        yield return StateChange(HistoryState.AposProfessores);
                        GameMemory.PodeSubirAEscada = true;
                    }
                    NpcParty.Clear();
                    for (int i = 0; i < NpcPartyBase.Count; i++)
                    {
                        Polimon PolimonDaVez = ScriptableObject.CreateInstance("Polimon") as Polimon;
                        PolimonDaVez.init(NpcPartyBase[i], Random.Range(BattleScene.GetComponent<BattleSystem>().MediumPartyLevel(GameMemory.Party), BattleScene.GetComponent<BattleSystem>().MediumPartyLevel(GameMemory.Party) + 5));
                        PolimonDaVez.name = PolimonDaVez.Name; //seta o nome no inspetor para melhor visualização
                        NpcParty.Add(PolimonDaVez);
                    }
                    yield return ReadDialoglistWhoCallsBattle(Npc.getList(HistoryState.AposSamuel));
                }
                else if ((Npc.Character.characterName == "Magaiver" || Npc.Character.characterName == "Samuel") && isNotInBattle)
                    yield return ReadDialoglist(Npc.getList(state));
                break;

            case HistoryState.AposProfessores:
                    yield return StateChange(HistoryState.Fim);
                    yield return ReadDialoglist(Npc.getList(HistoryState.AposProfessores));
                break;

            case HistoryState.Fim:
                if (Npc.Character.characterName == "Magaiver" && isNotInBattle)
                {
                    if (haveBeenDefeated)
                    {
                        yield return StateChange(HistoryState.Fim);
                    }
                    NpcParty.Clear();
                    for (int i = 0; i < NpcPartyBase.Count; i++)
                    {
                        Polimon PolimonDaVez = ScriptableObject.CreateInstance("Polimon") as Polimon;
                        PolimonDaVez.init(NpcPartyBase[i], Random.Range(50, 100));
                        PolimonDaVez.name = PolimonDaVez.Name; //seta o nome no inspetor para melhor visualiza��o
                        NpcParty.Add(PolimonDaVez);
                    }
                    yield return ReadDialoglistWhoCallsBattle(Npc.getList(HistoryState.Fim));
                }
                else if ((Npc.Character.characterName == "Wylliams" || Npc.Character.characterName == "Samuel") && isNotInBattle)
                    yield return ReadDialoglist(Npc.getList(state));
                
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            playerIsClose = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            playerIsClose = false;
            TurnoffDialog();
        }
    }

    private void TurnoffDialog() {
        NpcScreen.SetActive(false);
    }

    public IEnumerator TypeDialog(string dialog)
    {
        letterPerSecond2x = letterPerSecond * 3;
        TextBox.text = "";
        for (int i = 0; i < dialog.Length; i++)
        {
            TextBox.text = dialog.Substring(0, i);

            if ((Input.GetButtonDown("Fire3") || Input.GetKeyDown(KeyCode.Z)) && playerIsClose && NpcScreen != null && isRunning)
            {
                TextBox.text = dialog;
                break;
            }

            yield return new WaitForSeconds(1f / letterPerSecond);
        }
        yield return WaitForPlayerInput();
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

    private IEnumerator ReadDialoglist(List<string> DialogList)
    {
        isRunning = true;
        foreach (var Dialog in DialogList)
        {
            string newDialog = "";
            newDialog = Dialog.Replace($"<NAME>", $"{ChosenCharacterString}");
            yield return TypeDialog(newDialog);
            yield return WaitForPlayerInput();
        }
        isRunning = false;
        TurnoffDialog();
        StopAllCoroutines();
    }

    private IEnumerator ReadDialoglistWhoCallsBattle(List<string> DialogList)
    {
        isRunning = true;
        foreach (var Dialog in DialogList)
        {
            string newDialog = "";
            newDialog = Dialog.Replace($"<NAME>", $"{ChosenCharacterString}");
            yield return TypeDialog(newDialog);
            yield return WaitForPlayerInput();
        }
        isRunning = false;
        TurnoffDialog();
        BattleScene.SetActive(true);
        BattleScene.GetComponent<BattleSystem>().BattleAnimation(this, true);
        StopAllCoroutines();
    }
}
