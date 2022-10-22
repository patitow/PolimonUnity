using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Transform movePoint;
    public string lastKey;

    public LayerMask whatStopsMovement;
    public LayerMask grassLayer;
    public Animator animator;

    [SerializeField] private BattleSystem BattleSystem;
    [SerializeField] private GameObject BattleScreen;
    [SerializeField] private GameObject NpcScreen;
    [SerializeField] private GameObject BlackScreen;
    [SerializeField] private GameObject TransitionScreen;

    public bool isInBattle;
    public bool canPlayerMove;
    public bool canPlayerTeleport;

    private GameObject currentTeleporter;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("Speed", 1);
        movePoint.parent = null;
        BattleScreen.SetActive(false);
        canPlayerMove = true;
        canPlayerTeleport = true;
    }

    void Update()
    {
        allowPlayerToMoveIfNotInBattle();
        CalculateMovement();
        if (BattleScreen.activeInHierarchy)
        {
            isInBattle = true;
        } else { isInBattle = false; }
    }

    private void FixedUpdate()
    {
        CheckRandomEncounter();
    }

    public void CalculateMovement() {


        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Input.GetButton("Fire3"))
        {
            moveSpeed = 7f;
            animator.SetFloat("Speed", 7f / 5);
        }
        else if (Input.GetButtonUp("Fire3"))
        {
            moveSpeed = 5f;
            animator.SetFloat("Speed", 1);
        }
        else {
            animator.SetFloat("Speed", 1);
        }
        
        if (Vector3.Distance(transform.position, movePoint.position) == 0)
        {

            if ((Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f) && (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f))
            { // Quando apertados os dois bot�es, ir� priorizar o �ltimo apertado

                if (lastKey == "Horizontal" && canPlayerMove)
                { // estava se movendo na horizontal, e decidiu se mover na vertical
                    animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
                    animator.SetFloat("Horizontal", 0);
                    animator.SetBool("isMoving", true);
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0, Input.GetAxisRaw("Vertical"), 0), .05f, whatStopsMovement))
                    {
                        movePoint.position += new Vector3(0, Input.GetAxisRaw("Vertical"), 0);
                        lastKey = "Vertical";
                    }
                }
                else if (lastKey == "Vertical" && canPlayerMove)
                { // estava se movendo na vertical, e decidiu se mover na horizontal
                    animator.SetFloat("Vertical", 0);
                    animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
                    animator.SetBool("isMoving", true);
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0), .05f, whatStopsMovement))
                    {
                        movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
                        lastKey = "Horizontal";
                    }
                }
                else
                {
                    animator.SetBool("isMoving", false);
                }
            }
            else
            {
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f && canPlayerMove)
                {
                    animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
                    animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
                    animator.SetBool("isMoving", true);
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0), .05f, whatStopsMovement))
                    {
                        movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
                        lastKey = "Horizontal";
                    }
                }
                else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f && canPlayerMove)
                {
                    animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
                    animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
                    animator.SetBool("isMoving", true);
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0, Input.GetAxisRaw("Vertical"), 0), .05f, whatStopsMovement))
                    {
                        movePoint.position += new Vector3(0, Input.GetAxisRaw("Vertical"), 0);
                        lastKey = "Vertical";
                    }
                }
                else
                {
                    animator.SetBool("isMoving", false);
                }
            }
        }

    }

    public void CheckRandomEncounter() {
        if ((Physics2D.OverlapCircle(this.transform.position, 0.4f, grassLayer) != null) &&(Vector3.Distance(transform.position,movePoint.position)!=0f)) {

            if (Random.Range(1, 1001) <= 10)
            {
                //Debug.Log("A wild Polimon has appeared!");
                BattleScreen.SetActive(true);
                BattleSystem.ChamaNovaBatalha();
            }
            else {
            }
        }

    }

    public void allowPlayerToMoveIfNotInBattle() {
        if (!BattleScreen.activeInHierarchy && !NpcScreen.activeInHierarchy && !TransitionScreen.activeInHierarchy)
        {
            canPlayerMove = true;
        }
        else if (BattleScreen.activeInHierarchy || NpcScreen.activeInHierarchy || TransitionScreen.activeInHierarchy) {
            canPlayerMove = false;
        }
        else
        {
            canPlayerMove = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter") && canPlayerTeleport)
        {
            currentTeleporter = collision.gameObject;
            TransitionScreen.SetActive(true);
            StartCoroutine(tpEnter());
        }
    }

    private IEnumerator tpEnter()
    {
        BlackScreen.GetComponent<Animator>().SetBool("StartedTransition", true);
        yield return new WaitForSeconds(1f);
        yield return Teleport();
        TransitionScreen.SetActive(false);
    }

    private IEnumerator Teleport()
    {
        if (currentTeleporter != null && canPlayerTeleport)
        {
            movePoint.position = currentTeleporter.GetComponent<Teleporter>().GetSaida();
            transform.position = movePoint.position;
            Debug.Log("Teleportou para:" + currentTeleporter.name); 
            canPlayerTeleport = false;
        }
        yield return tpLeave();
    }

    private IEnumerator tpLeave()
    {
        BlackScreen.GetComponent<Animator>().SetBool("FinishedTransition", true);
        yield return new WaitForSeconds(1f);
        BlackScreen.GetComponent<Animator>().SetBool("FinishedTransition", false);
        BlackScreen.GetComponent<Animator>().SetBool("StartedTransition", false);
        
        yield return new WaitForSeconds(0.5f);
    }
    
    private IEnumerator TransitionScreensetActive(bool state)
    {
        yield return new WaitForSeconds(1f);
        TransitionScreen.SetActive(state);
        yield return null;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            if ((collision.gameObject == currentTeleporter) && !canPlayerTeleport)
            {
                Vector3 offsetSaida = currentTeleporter.GetComponent<Teleporter>().GetOffset();
                animator.SetFloat("Horizontal", offsetSaida.x);
                animator.SetFloat("Vertical", offsetSaida.y);
                currentTeleporter = null;
                canPlayerTeleport = true;
                StartCoroutine(TransitionScreensetActive(false));
            }
        }
    }


}
