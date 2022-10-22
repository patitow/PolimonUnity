using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    [SerializeField] TextMeshProUGUI UsesText;
    [SerializeField] TextMeshProUGUI TypeText;
    [SerializeField] TextMeshProUGUI dialogText;

    [SerializeField] List<TextMeshProUGUI> actionTexts;
    [SerializeField] List<TextMeshProUGUI> moveTexts;

    [SerializeField] int letterPerSecond;
    private int letterPerSecond2x;
    [SerializeField] Color highLightColor;
    
    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialog = dialog + " ";
        letterPerSecond2x = letterPerSecond * 3;
        dialogText.text = "";
        for (int i = 0; i < dialog.Length; i++)
        {
            dialogText.text = dialog.Substring(0, i);

            if (Input.GetButtonDown("Fire3") || Input.GetKeyDown(KeyCode.Z))
            {
                dialogText.text = dialog;
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

    public void EnableDialogText(bool enabled) {
        dialogText.enabled = enabled;
    }
    
    public void EnableActionSelector(bool enabled) {    
        actionSelector.SetActive(enabled);
    }
    
    public void EnableMoveSelector(bool enabled) {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    public void UpdateActionSelection(int selectedAction){
        for (int i = 0; i < actionTexts.Count; i++)
        {
            if (i == selectedAction)
            {
                actionTexts[i].color = highLightColor;
            }
            else {
                actionTexts[i].color = Color.black;
            }
        }
    }

    public void UpdateMoveSelection(int selectedMove,Move move) {
        for (int i = 0; i < actionTexts.Count; i++)
        {
            if (i == selectedMove)
            {
                moveTexts[i].color = highLightColor;
            }
            else
            {
                moveTexts[i].color = Color.black;
            }

            if (move != null){
                UsesText.text = $"{move.Uses} / {move.Base.Uses}";
                TypeText.text = move.Base.Type.ToString();
            }
            else {
                UsesText.text = "- / -";
                TypeText.text = "-";
            }
        }
    }

    public void SetMoveNames(List<Move> moves) {
        for (int i = 0;i< moveTexts.Count;i++){
            if (i < moves.Count)
                moveTexts[i].text = moves[i].Base.name;
            else
                moveTexts[i].text = "-";
        }
    }
}
