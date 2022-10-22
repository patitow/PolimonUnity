using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum BattleState {
    Start,
    Animation,
    PlayerAction,
    PlayerMove,
    ChangePolimon,
    EnemyMove,
    Busy,
}

public class BattleSystem : MonoBehaviour
{
    
    [SerializeField] BattleHud PlayerHud;
    [SerializeField] BattleHud EnemyHud;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] PolimonBaseDatabase PolimonBaseDatabase;
    [SerializeField] CharacterManager CharacterManager;
    [SerializeField] string ChooseAction;
    [SerializeField] int minLevel;
    [SerializeField] List<Polimon> PlayerParty;
    [SerializeField] List<Polimon> EnemyParty;

    public BattleUnit PlayerUnit;
    public BattleUnit EnemyUnit;
    public PlayerSending jogador;
    public EnemySending npc;
    public PartySelection Party;
    public List<bool> Fainted;   

    public Npcs npcteste;

    [SerializeField] List<TextMeshProUGUI> PolimonButtons;

    BattleState state;
    int currentAction;
    int currentMove;
    int currentPolimon;
    int lastPolimon;
    int currentEnemy;
    bool h_isAxisInUse;
    bool v_isAxisInUse;
    Npcs NPC;
    
    private void Update()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove) {
            HandleMoveSelection();
        }
        else if (state == BattleState.ChangePolimon){
            HandlePolimonSelection();
        }
        for(int i=0; i<6;i++){
            if(i<PlayerParty.Count){
                if(PlayerParty[i].HP==0){
                    Fainted[i]=true;
                }else{
                    Fainted[i]=false;
                }
            }else{
                Fainted[i]=true;
            }
        }
    }

    public void ChamaNovaBatalha() {
        StopAllCoroutines();
        EnemyParty.Clear();
        BattleAnimation(npcteste, false);
    }

    public void BattleAnimation(Npcs npc, bool treinador) {
        foreach (var polimon in PlayerParty) {
            polimon.HP = polimon.MaxHP;
        }
        EnemyUnit.deslizarBatalha();
        if(treinador){
            StartCoroutine(SetupBattleAgaisntPlayer(npc));
        }else{
            StartCoroutine(SetupBattleAgaisntWildPolimon());
        }
    }

    public IEnumerator SetupBattleAgaisntPlayer(Npcs Npc){
        NPC = Npc;
        lastPolimon = currentPolimon;
        EnemyUnit.SetActive(false);
        npc.SetActive(true);
        PlayerHud.desaparecer();
        EnemyHud.desaparecer();
        PlayerUnit.deslizar = false;
        jogador.deslizarBatalha();
        npc.deslizarBatalha(Npc);
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableMoveSelector(false);
        PlayerParty = CharacterManager.Party;
        PlayerHud.SetData(PlayerParty[0]);
        PlayerUnit.Setup(PlayerParty[0],true,true);
        EnemyParty.Clear();
        for(int i=0; i<Npc.NpcParty.Count;i++){
            Polimon PolimonDaVez = ScriptableObject.CreateInstance("Polimon") as Polimon;
            PolimonDaVez.init(Npc.NpcParty[i].Base, Npc.NpcParty[i].Level);
            PolimonDaVez.name = PolimonDaVez.Name; //seta o nome no inspetor para melhor visualiza��o
            EnemyParty.Add(PolimonDaVez);
        }
        EnemyUnit.Setup(EnemyParty[0],false,false);
        EnemyHud.SetData(EnemyParty[0]);

        dialogBox.SetMoveNames(PlayerParty[0].Moves);

        yield return dialogBox.TypeDialog($"{Npc.Npc.Character.characterName} quer batalhar com você!");

        yield return new WaitForSeconds(1f);
        npc.deslizar = 2;
        EnemyUnit.SetActive(true);
        
        yield return dialogBox.TypeDialog($"{Npc.Npc.Character.characterName} escolheu seu {EnemyParty[0].Name}!");
        jogador.parado = false;
        npc.deslizar = 2;
        EnemyUnit.SetActive(true);
        yield return new WaitForSeconds(1f);
        jogador.deslizar = 2;
        
        PlayerUnit.deslizarBatalha();
        PlayerHud.aparecer();
        EnemyHud.aparecer();
        PlayerAction();
    }

    public IEnumerator SetupBattleAgaisntWildPolimon() {
        NPC = null;
        lastPolimon = currentPolimon;
        npc.SetActive(false);
        PlayerHud.desaparecer();
        EnemyHud.desaparecer();
        PlayerUnit.deslizar = false;
        jogador.deslizarBatalha();      
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableMoveSelector(false);
        PlayerParty = CharacterManager.Party;
        PlayerHud.SetData(PlayerParty[0]);
        PlayerUnit.Setup(PlayerParty[0],true,true);

        Polimon PolimonDaVez = ScriptableObject.CreateInstance("Polimon") as Polimon;
        PolimonDaVez.init(PolimonBaseDatabase.PolimonList[Random.Range(0, PolimonBaseDatabase.PolimonList.Count)], Random.Range(minLevel, MediumPartyLevel(PlayerParty) + 3));
        PolimonDaVez.name = PolimonDaVez.Name; //seta o nome no inspetor para melhor visualiza��o
        EnemyParty.Clear();
        EnemyParty.Add(PolimonDaVez);
        EnemyUnit.Setup(EnemyParty[0],false,true);
        EnemyHud.SetData(EnemyParty[0]);

        dialogBox.SetMoveNames(PlayerParty[0].Moves);

        yield return dialogBox.TypeDialog($"Um {EnemyParty[0].Name} selvagem apareceu!");
        jogador.parado = false;
        yield return new WaitForSeconds(1f);
        jogador.deslizar = 2;
        
        PlayerUnit.deslizarBatalha();
        PlayerHud.aparecer();
        EnemyHud.aparecer();
        PlayerAction();
    }

    void Capture() {
        if (EnemyUnit.CanBeCaptured)
        {
            if (Random.Range(0, 101) <= 90)
            {
                PlayerParty.Add(EnemyParty[0]);
                CharacterManager.Party = PlayerParty;
            }
        }
        else {
        
        }
    }

    void SwapPosition(int Index,List<Polimon> targetList) {
        Polimon temp = targetList[0];
        targetList[0] = targetList[Index];
        targetList[Index] = temp;
    }

    bool CheckPartyisAlive(List<Polimon> targetList) {
        foreach (Polimon polimon in targetList) {
            if (polimon.HP >= 1)
                return true;
        }
        return false;
    }

    public int MediumPartyLevel(List<Polimon> targetList) {
        if (targetList.Count > 0)
        {
            int i = 0;
            foreach (Polimon polimon in targetList)
            {
                i += polimon.Level;
            }
            return i / targetList.Count;
        }
        else {
            return 0;
        }
    }

    void PlayerAction() {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog(ChooseAction));
        dialogBox.EnableActionSelector(true);
    }

    void PlayerMove() {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    IEnumerator PerformPlayerMove() {
        state = BattleState.Busy;
        var move = PlayerParty[currentPolimon].Moves[currentMove];
        yield return dialogBox.TypeDialog($"{PlayerParty[currentPolimon].Name} usou {move.Base.Name}");

        yield return new WaitForSeconds(1f);

        var DamageDetails = EnemyParty[currentEnemy].TakeDamage(move, PlayerParty[currentPolimon]);
        yield return EnemyHud.UpdateHP();
        yield return new WaitForSeconds(0.3f);
        yield return ShowDamageDetails(DamageDetails);
        yield return new WaitForSeconds(0.3f);
        yield return PlayerHud.UpdateHP();

        if (DamageDetails.fainted)
        {
            yield return dialogBox.TypeDialog($"{EnemyParty[currentEnemy].Name} desmaiou!");
            yield return new WaitForSeconds(1f);
            if(PlayerParty[currentPolimon].vaiEvoluir(EnemyParty[currentEnemy].Level*10)){
                yield return dialogBox.TypeDialog($"Seu {PlayerParty[currentPolimon].Name} evoluiu para um {PlayerParty[currentPolimon].Base.evolution.Name}!");
                PlayerParty[currentPolimon].addXp(EnemyParty[currentEnemy].Level*10);
                yield return new WaitForSeconds(1f);
                PlayerHud.SetData(PlayerParty[currentPolimon]);
                PlayerUnit.Setup(PlayerParty[currentPolimon],true,true);
            }else{
                PlayerParty[currentPolimon].addXp(EnemyParty[currentEnemy].Level*10);
                PlayerHud.SetData(PlayerParty[currentPolimon]);
            }

            bool enemyFainted=true;
            foreach(var polimon in EnemyParty){
                if(polimon.HP>0){
                    enemyFainted=false;
                }
            }
            if(enemyFainted){
                currentAction = 0;
                currentMove = 0;
                currentPolimon = 0;
                currentEnemy = 0;
                if (NPC != null) {
                    if (NPC.Npc.Character.characterName == "Wylliams")
                    {
                        CharacterManager.PodeSubirAEscada = true;
                    }
                    if (NPC.Npc.Character.characterName == "Samuel")
                    {
                        CharacterManager.ChaveSaladeEcomp = true;
                    }
                }
                this.gameObject.SetActive(false); // provisorio
            }else{
                currentEnemy+=1;
                EnemyUnit.Setup(EnemyParty[currentEnemy],false,false);
                EnemyHud.SetData(EnemyParty[currentEnemy]);
                state = BattleState.PlayerAction;
                PlayerAction();
            }
        }
        else {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;
        var move = EnemyParty[currentEnemy].GetRandomMove();
        yield return dialogBox.TypeDialog($"{EnemyParty[currentEnemy].Name} usou {move.Base.Name}");

        yield return new WaitForSeconds(1f);


        var DamageDetails = PlayerParty[currentPolimon].TakeDamage(move, EnemyParty[currentEnemy]);
        yield return PlayerHud.UpdateHP();
        yield return ShowDamageDetails(DamageDetails);
        yield return EnemyHud.UpdateHP();


        if (DamageDetails.fainted)
        {
            yield return dialogBox.TypeDialog($"{PlayerParty[currentPolimon].Name} desmaiou!");
            yield return new WaitForSeconds(1f);
            if(Fainted[0]==Fainted[1] && Fainted[0]==Fainted[2] && Fainted[0]==Fainted[3] && Fainted[0]==Fainted[4] && Fainted[0]==Fainted[5] && Fainted[0]==true){
                currentAction = 0;
                currentMove = 0;
                currentPolimon = 0;
                currentEnemy = 0;
                this.gameObject.SetActive(false); // provisorio
            }else{
                state = BattleState.ChangePolimon;
                StartCoroutine(TrocarPolimonAoMorrer());
            }
        }
        else
        {
            PlayerAction();
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails) {
        if (damageDetails.critical > 1f) {
            yield return dialogBox.TypeDialog("Ataque Crítico!");
        }

        if (damageDetails.Effectiveness > 1f)
        {
            yield return dialogBox.TypeDialog("Ataque superefetivo!");
        }
        else if (damageDetails.Effectiveness < 1f) {
            yield return dialogBox.TypeDialog("Ataque não muito efetivo!");
        }
    }

    void HandleActionSelection() {
        if (Input.GetAxisRaw("Vertical") != 0){
            if (v_isAxisInUse == false){
                if (currentAction < 2)
                {
                    currentAction += 2;
                }
                else
                {
                    currentAction -= 2;
                }
                v_isAxisInUse = true;
            }
        } 
        else{
            v_isAxisInUse = false;
        }
        
        if (Input.GetAxisRaw("Horizontal") != 0){
            if (h_isAxisInUse == false){
                if (currentAction % 2 == 1){
                    currentAction -= 1;
                }
                else{
                    currentAction += 1;
                }
                h_isAxisInUse = true;
            }
        } 
        else{
            h_isAxisInUse = false;
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Fire3"))
        {
            if (currentAction == 0)
            {
                //lutar
                PlayerMove();
            }
            else if (currentAction == 1)
            {
                //captura
                if (Random.value * 100f <= 30f)
                {
                    StartCoroutine(Captura(true));

                }
                else
                {
                    StartCoroutine(Captura(false));
                }
            }
            else if (currentAction == 2)
            {
                //polimons
                StartCoroutine(TrocarPolimon());
            }
            else if (currentAction == 3)
            {
                //fugir
                if (Random.value * 100f <= 30f) {
                    StartCoroutine(Fugir(true));

                }else{
                    StartCoroutine(Fugir(false));
                } 
            }
        }

        dialogBox.UpdateActionSelection(currentAction);
    }

    IEnumerator Fugir(bool fuga){

        if(fuga==true){
            state = BattleState.Busy;
            currentAction = 0;
            currentMove = 0;
            currentPolimon = 0;
            currentEnemy = 0;
            dialogBox.EnableActionSelector(false);
            yield return dialogBox.TypeDialog("Você conseguiu fugir!");
            yield return new WaitForSeconds(1f);
            this.gameObject.SetActive(false);
        }else{
            state = BattleState.Busy;
            dialogBox.EnableActionSelector(false);
            yield return dialogBox.TypeDialog("Você não conseguiu fugir!");
            yield return new WaitForSeconds(1f);
            StartCoroutine(EnemyMove());
        }

    }

    IEnumerator Captura(bool captura)
    {
        if (captura == true && EnemyUnit.CanBeCaptured && PlayerParty.Count < 6)
        {
            state = BattleState.Busy;
            currentAction = 0;
            currentMove = 0;
            currentPolimon = 0;
            currentEnemy = 0;
            dialogBox.EnableActionSelector(false);
            yield return dialogBox.TypeDialog("Você capturou um " + EnemyParty[0].Name);
            PlayerParty.Add(EnemyParty[0]);
            yield return new WaitForSeconds(1f);
            this.gameObject.SetActive(false);
        }
        else if (!EnemyUnit.CanBeCaptured) {
            state = BattleState.Busy;
            dialogBox.EnableActionSelector(false);
            yield return dialogBox.TypeDialog("Você não pode capturar um Polimon que já possui dono!");
            yield return new WaitForSeconds(1f);
            StartCoroutine(EnemyMove());
        } else if (EnemyUnit.CanBeCaptured && PlayerParty.Count >= 5)
        {
            state = BattleState.Busy;
            dialogBox.EnableActionSelector(false);
            yield return dialogBox.TypeDialog("Sua Party já está cheia, você não pode capturar outro Polimon!");
            yield return new WaitForSeconds(1f);
            StartCoroutine(EnemyMove());
        }
        else
        {
            state = BattleState.Busy;
            dialogBox.EnableActionSelector(false);
            yield return dialogBox.TypeDialog("Você não conseguiu capturar!");
            yield return new WaitForSeconds(1f);
            StartCoroutine(EnemyMove());
        }

    }
    
    IEnumerator TrocarPolimon()
    {
        state = BattleState.ChangePolimon;
        Party.SetActive(true);

        for(int i=0; i<PlayerParty.Count; i++){
            Party.SetPolimon(i,PlayerParty[i]);
        }
        yield return new WaitForSeconds(1f);
        HandlePolimonSelection();

        yield return null;

    }

    IEnumerator TrocarPolimonAoMorrer()
    {
        state = BattleState.ChangePolimon;
        Party.SetActive(true);

        for (int i = 0; i < PlayerParty.Count; i++)
        {
            Party.SetPolimon(i, PlayerParty[i]);
        }
        yield return new WaitForSeconds(1f);
        HandlePolimonSelectionAoMorrer();

        yield return null;

    }

    void HandlePolimonSelectionAoMorrer()
    {

        if (Input.GetKeyDown("down") || Input.GetKeyDown(KeyCode.S))
        {
            if (v_isAxisInUse == false)
            {
                if (currentPolimon < 4)
                {
                    if (PlayerParty.Count > currentPolimon + 2)
                    {
                        currentPolimon += 2;
                    }
                    else if (currentPolimon - 2 >= 0)
                    {
                        currentPolimon -= 2;
                    }
                }
                else
                {
                    if (PlayerParty.Count > 3)
                    {
                        currentPolimon -= 4;
                    }
                    else if (currentPolimon - 2 >= 0)
                    {
                        currentPolimon -= 2;
                    }
                }
                v_isAxisInUse = true;
            }
        }
        else if (Input.GetKeyDown("up") || Input.GetKeyDown(KeyCode.W))
        {
            if (v_isAxisInUse == false)
            {
                if (currentPolimon < 2)
                {
                    if (PlayerParty.Count > currentPolimon + 4)
                    {
                        currentPolimon += 4;
                    }
                    else if (PlayerParty.Count > currentPolimon + 2)
                    {
                        currentPolimon += 2;
                    }
                }
                else
                {
                    currentPolimon -= 2;
                }
                v_isAxisInUse = true;
            }
        }
        else
        {
            v_isAxisInUse = false;
        }

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (h_isAxisInUse == false)
            {
                if (currentPolimon % 2 == 1)
                {
                    currentPolimon -= 1;
                }
                else
                {
                    if (PlayerParty.Count > currentPolimon + 1)
                    {
                        currentPolimon += 1;
                    }
                }
                h_isAxisInUse = true;
            }
        }
        else
        {
            h_isAxisInUse = false;
        }

        if (currentPolimon < PlayerParty.Count)
        {
            Party.UpdatePolimonSelection(currentPolimon, PlayerParty[currentPolimon]);
        }
        else
        {
            Party.UpdatePolimonSelection(currentPolimon, null);
        }

        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Fire3")) && PlayerParty[currentPolimon].HP != 0)
        {
            if (lastPolimon == currentPolimon)
            {
                Party.SetActive(false);
                state = BattleState.PlayerAction;
            }
            else
            {
                PlayerUnit.Setup(PlayerParty[currentPolimon], true, true);
                Party.SetActive(false);
                dialogBox.SetMoveNames(PlayerParty[currentPolimon].Moves);
                PlayerHud.SetData(PlayerParty[currentPolimon]);
                PlayerUnit.Setup(PlayerParty[currentPolimon], true, true);
                dialogBox.EnableActionSelector(false);
                lastPolimon = currentPolimon;
                state = BattleState.EnemyMove;
                PlayerAction();

            }
        }
    }

    void HandlePolimonSelection()
    {
        
        if (Input.GetKeyDown("down") || Input.GetKeyDown(KeyCode.S))
        {
            if (v_isAxisInUse == false)
            {
                if (currentPolimon < 4)
                {
                    if(PlayerParty.Count>currentPolimon+2){
                        currentPolimon += 2;
                    }else if(currentPolimon-2>=0){
                        currentPolimon -= 2;
                    }
                }
                else
                {
                    if(PlayerParty.Count>3){
                        currentPolimon -= 4;
                    }else if(currentPolimon-2>=0){
                        currentPolimon -= 2;
                    }
                }
                v_isAxisInUse = true;
            }
        }
        else if (Input.GetKeyDown("up") || Input.GetKeyDown(KeyCode.W) )
        {
            if (v_isAxisInUse == false)
            {
                if (currentPolimon < 2)
                {
                    if(PlayerParty.Count>currentPolimon+4){
                        currentPolimon += 4;
                    }else if(PlayerParty.Count>currentPolimon+2){
                        currentPolimon += 2;
                    }
                }
                else
                {
                    currentPolimon -= 2;
                }
                v_isAxisInUse = true;
            }
        }
        else
        {
            v_isAxisInUse = false;
        }

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (h_isAxisInUse == false)
            {
                if (currentPolimon % 2 == 1)
                {
                    currentPolimon -= 1;
                }
                else
                {
                    if(PlayerParty.Count>currentPolimon+1){
                        currentPolimon += 1;
                    }
                }
                h_isAxisInUse = true;
            }
        }
        else
        {
            h_isAxisInUse = false;
        }

        if (currentPolimon < PlayerParty.Count)
        {
            Party.UpdatePolimonSelection(currentPolimon, PlayerParty[currentPolimon]);
        }
        else
        {
            Party.UpdatePolimonSelection(currentPolimon, null);
        }

        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Fire3")) && PlayerParty[currentPolimon].HP != 0)
        {
            if(lastPolimon == currentPolimon){
                Party.SetActive(false);
                state = BattleState.PlayerAction;
            }
            else{
                PlayerUnit.Setup(PlayerParty[currentPolimon],true,true);
                Party.SetActive(false);
                dialogBox.SetMoveNames(PlayerParty[currentPolimon].Moves);
                PlayerHud.SetData(PlayerParty[currentPolimon]);
                PlayerUnit.Setup(PlayerParty[currentPolimon],true,true);
                dialogBox.EnableActionSelector(false);
                lastPolimon = currentPolimon;
                state = BattleState.EnemyMove;
                StartCoroutine(EnemyMove());
            }
        }
    }

    void HandleMoveSelection() {
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            if (v_isAxisInUse == false)
            {
                if (currentMove < 2)
                {
                    currentMove += 2;
                }
                else
                {
                    currentMove -= 2;
                }
                v_isAxisInUse = true;
            }
        }
        else
        {
            v_isAxisInUse = false;
        }

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (h_isAxisInUse == false)
            {
                if (currentMove % 2 == 1)
                {
                    currentMove -= 1;
                }
                else
                {
                    currentMove += 1;
                }
                h_isAxisInUse = true;
            }
        }
        else
        {
            h_isAxisInUse = false;
        }
        
        if (currentMove < PlayerParty[currentPolimon].Moves.Count)
        {
            dialogBox.UpdateMoveSelection(currentMove, PlayerParty[currentPolimon].Moves[currentMove]);
        }
        else {
            dialogBox.UpdateMoveSelection(currentMove, null);
        }

        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Fire3")) && currentMove < PlayerParty[currentPolimon].Moves.Count)
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }
    }

}
