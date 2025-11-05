using System;
using UnityEngine;
using CodeMonkey;
using Unity.Collections;
using NUnit.Framework;
public class CharacterBattle : MonoBehaviour
{
    private SpriteRenderer amongSprite;
    private State state;
    public float alivenotes;
    private Vector3 slideTargetPosition;
    private Action onSlideComplete, onRhythmComplete;
    private GameObject selectionCircleGameObject, buttons, rhythmchart;
    private HealthSystem healthSystem;
    public Sprite enemySprite;
    private GameManager gameManager;
    public bool player;

    private enum State
    {
        Idle,
        Rhythming,
        Sliding,
        Busy,
    }

    private void Awake()
    {
        amongSprite = GetComponent<SpriteRenderer>();
        selectionCircleGameObject = transform.Find("activecircle").gameObject;
        HideSelectionCircle();
        state = State.Idle;
    }


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        buttons = GameObject.Find("Buttons");
        rhythmchart = GameObject.Find("AttackRhythmChart");
        if (buttons != null) buttons.SetActive(false);
        if (rhythmchart != null) rhythmchart.SetActive(false);
    }

    public void Setup(bool isPlayerTeam)
    {
        if (isPlayerTeam)
        {
            amongSprite.color = Color.white;
            player = true;
        }

        else
        {
            amongSprite.sprite = enemySprite;
            player = false;
        }
        healthSystem = new HealthSystem(1000);
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Busy:
                break;
            case State.Sliding:
                float slideSpeed = 10f;
                Vector3 moveDir = slideTargetPosition - GetPosition();
                transform.position += moveDir * slideSpeed * Time.deltaTime;

                float reachedDistance = 1f;
                if (Vector3.Distance(GetPosition(), slideTargetPosition) < reachedDistance)
                {
                    transform.position = slideTargetPosition;
                    gameManager.scoreText.text = "Attack Power: " + gameManager.currentscore;
                    var action = onSlideComplete;
                    onSlideComplete = null;
                    state = State.Idle;
                    action?.Invoke();
                }
                break;
            case State.Rhythming:
                if (alivenotes == 0)
                {
                    Destroy(gameManager.notesInstance);
                    gameManager.totalNotes = 0;
                    gameManager.startPlaying = false;
                    buttons.SetActive(false);
                    rhythmchart.SetActive(false);
                    var action = onRhythmComplete;
                    onRhythmComplete = null;
                    state = State.Sliding;
                    action?.Invoke();
                }
                break;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
        bool isCriticalHit = damageAmount > 250f;
        DamagePopup.Create(GetPosition(), damageAmount, isCriticalHit);
    }

    public bool IsDead() 
    {
        return healthSystem.IsDead();
    }

    public void Attack(CharacterBattle targetCharacterBattle, Action onAttackComplete)
    {
        slideTargetPosition = targetCharacterBattle.GetPosition() + (GetPosition() - targetCharacterBattle.GetPosition()).normalized * 1f;
        Vector3 startingPosition = GetPosition();

        if (player)
        {
            RhythmChartBegin(() =>
            {
                SlideToPosition(slideTargetPosition, () =>
                {
                    state = State.Busy;

                    int damageAmount = gameManager.currentscore;
                    targetCharacterBattle.Damage(damageAmount);
                    gameManager.currentscore = 0;
                    gameManager.currentMultiplier = 1;
                    gameManager.multiText.text = "Multiplier: x" + gameManager.currentMultiplier;
                    

                    SlideToPosition(startingPosition, () =>
                    {
                        state = State.Idle;
                        onAttackComplete();
                    });


                });
            });
        }

        else if (!player)
        {
            SlideToPosition(slideTargetPosition, () =>
            {
                state = State.Busy;

                int damageAmount = UnityEngine.Random.Range(100, 275);
                targetCharacterBattle.Damage(damageAmount);

                SlideToPosition(startingPosition, () =>
                {
                    state = State.Idle;
                    onAttackComplete();
                });


            });
        }
        else
        {
            Debug.Log("Well poo");
        }

    }


    private void SlideToPosition(Vector3 slideTargetPosition, Action onSlideComplete)
    {
        this.slideTargetPosition = slideTargetPosition;
        this.onSlideComplete = onSlideComplete;
        state = State.Sliding;
    }

    private void RhythmChartBegin(Action onRhythmComplete)
    {
        this.onRhythmComplete = onRhythmComplete;
        this.alivenotes = gameManager.totalNotes;

        gameManager.activeCharacterBattle = this;

        buttons.SetActive(true);
        rhythmchart.SetActive(true);
        gameManager.notesInstance.SetActive(true);

        state = State.Rhythming;
    }

    public void HideSelectionCircle()
    {
        selectionCircleGameObject.SetActive(false);
    }

    public void ShowSelectionCircle()
    {
        selectionCircleGameObject.SetActive(true);
    }
}
