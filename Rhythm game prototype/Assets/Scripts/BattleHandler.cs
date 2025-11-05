using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleHandler : MonoBehaviour
{
    [SerializeField] private Transform pfCharacterBattle;


    private CharacterBattle playerCharacterBattle;
    private CharacterBattle enemyCharacterBattle;
    private CharacterBattle activeCharacterBattle;
    private State state;
    private GameObject buttons, rhythmchart;
    private GameManager gameManager;
    [SerializeField] TextMeshProUGUI attackText, pressAnything, playerWin, playerWinSubtext, playerLose, playerLoseSubtext;
    public bool player;

    private enum State
    {
        WaitingForPlayer, Busy,
    }

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager = gameManager.gameObject.GetComponent<GameManager>();
    }
    private void Start()
    {
        playerCharacterBattle = SpawnCharacter(true);
        enemyCharacterBattle = SpawnCharacter(false);

        playerWin.enabled = false;
        playerWinSubtext.enabled = false;
        playerLose.enabled = false;
        playerLoseSubtext.enabled = false;
        attackText.enabled = false;
        pressAnything.enabled = false;

        SetActiveCharacterBattle(playerCharacterBattle);
        state = State.WaitingForPlayer;
    }

    private void Update()
    {
        if (state == State.WaitingForPlayer)
        {
            attackText.enabled = true;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                attackText.enabled = false;
                if (gameManager.totalNotes > 0)
                {
                    state = State.Busy;
                    playerCharacterBattle.Attack(enemyCharacterBattle, () =>
                    {
                        ChooseNextActiveCharacter();
                    });
                }
                else if (gameManager.totalNotes <= 0)
                {
                    gameManager.NewNotes();

                    state = State.Busy;
                    playerCharacterBattle.Attack(enemyCharacterBattle, () =>
                    {
                        ChooseNextActiveCharacter();
                    });
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (TestBattleOver())
        {
            SceneManager.LoadScene("BattleScene");
        }
        }
    }

    private CharacterBattle SpawnCharacter(bool isPlayerTeam)
    {
        Vector3 position;
        if (isPlayerTeam)
        {
            position = new Vector3(-9, 7);
        }
        else
        {
            position = new Vector3(9, 7);
        }
        Transform characterTransform = Instantiate(pfCharacterBattle, position, Quaternion.identity);
        CharacterBattle characterBattle = characterTransform.GetComponent<CharacterBattle>();
        characterBattle.Setup(isPlayerTeam);

        return characterBattle;
    }

    private void SetActiveCharacterBattle(CharacterBattle characterBattle)
    {
        if (activeCharacterBattle != null)
        {
            activeCharacterBattle.HideSelectionCircle();
        }
        
        activeCharacterBattle = characterBattle;
        activeCharacterBattle.ShowSelectionCircle();
    }

    private void ChooseNextActiveCharacter()
    {
        if (TestBattleOver())
        {
            return;
        }

        if (activeCharacterBattle == playerCharacterBattle)
        {
            SetActiveCharacterBattle(enemyCharacterBattle);
            state = State.Busy;

            enemyCharacterBattle.Attack(playerCharacterBattle, () =>
                {
                    ChooseNextActiveCharacter();
                });
        }
        else
        {
            SetActiveCharacterBattle(playerCharacterBattle);
            state = State.WaitingForPlayer;
        }
    }

    private bool TestBattleOver() 
    {
        if (playerCharacterBattle.IsDead())
        {
            playerLose.enabled = true;
            playerLoseSubtext.enabled = true;
            Debug.Log("Enemy Wins!");
            return true;
        }

        if (enemyCharacterBattle.IsDead())
        {
            playerWin.enabled = true;
            playerWinSubtext.enabled = true;
            Debug.Log("Player Wins!");
            return true;
        }

        return false;
    }
}
