using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CharacterManager : MonoBehaviour
{
    [Header("Database do Character")]
    public CharacterDatabase CharacterDatabase;
    private int ChosenCharacterInt;
    private string ChosenCharacterString;

    [Header("Database de Iniciais")]
    public PolimonBaseDatabase InitialsDatabase;
    private int ChosenInitialInt;
    private string ChosenInitialName;

    [Header("Database de todos os Polimons")]
    public PolimonBaseDatabase AllPolimonDatabase;

    [Header("Dados do jogador")]
    public GameObject Player;
    public Character PersonagemEscolhido;
    public List<Polimon> Party;

    [SerializeField]private HistoryState state;

    public int level;
    private bool initialSet;

    [Header("Dados da História")]
    public bool PodeSubirAEscada;
    public GameObject TeleportesDaEscada;
    public bool ChaveSaladeEcomp;
    public GameObject PortadeEcomp;
    public GameObject NovaEscada;

    public HistoryState State { get => state; set => state = value; }

    public void HandleFirstIteration() {
        if (SceneManager.GetActiveScene().name == "MainGame")
        {
            if (PersonagemEscolhido == null)
                PersonagemEscolhido = LoadChar();
            if (Party.Count == 0)
            {
                setInitialParty();
            }
        }
    }

    public void LoadAnimationController()
    {
        if (SceneManager.GetActiveScene().name == "MainGame") {
            if (Player != null) {
                Animator anim = Player.GetComponent<Animator>();
                anim.runtimeAnimatorController = PersonagemEscolhido.characterAnimator;
            }
        }
    }

    public void setInitialParty()
    {
        Polimon Inicial = ScriptableObject.CreateInstance("Polimon") as Polimon;
        Inicial.init(LoadInitial(), level);
        Inicial.name = Inicial.Name; //seta o nome no inspetor para melhor visualização
        Party.Add(Inicial);
    }

    public Character LoadChar() {
        ChosenCharacterInt = PlayerPrefs.GetInt("ChosenCharacterInt");
        ChosenCharacterString = PlayerPrefs.GetString("ChosenCharacterString");
        return CharacterDatabase.CharacterList[ChosenCharacterInt];
    }
    
    public PolimonBase LoadInitial() {
        ChosenInitialInt = PlayerPrefs.GetInt("ChosenInitialInt");
        return InitialsDatabase.PolimonList[ChosenInitialInt];
    }

    private void Awake()
    {
        State = HistoryState.Start;
    }

    private void Start()
    {
        PodeSubirAEscada = false;
        ChaveSaladeEcomp = false;
        NovaEscada.SetActive(false);
        PortadeEcomp.SetActive(true);
        TeleportesDaEscada.SetActive(true);
        if (CharacterDatabase != null && InitialsDatabase != null) {
            HandleFirstIteration();
            LoadAnimationController();
        }
    }

    private void Update()
    {
        if (PodeSubirAEscada) {
            GameObject.Destroy(TeleportesDaEscada);
            NovaEscada.SetActive(true);
        }
        if (ChaveSaladeEcomp) {
            GameObject.Destroy(PortadeEcomp);
        }
    }
}
