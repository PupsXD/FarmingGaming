using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, ENDTURN, WON, LOST, CAPTURE, RUN }

public class CombatSystem : MonoBehaviour
{
	[SerializeField] private Creature _creature;

	[SerializeField] private BattleState state;

	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private GameObject enemyPrefab;

	[SerializeField] private Transform playerBattleStation;
	[SerializeField] private Transform enemyBattleStation;

	private CombatEntity enemyUnit;
	private CombatEntity playerUnit;
	private CombatPlayer combatPlayer;
	private CombatEnemy combatEnemy;

	private int turn;
	private int dodgeEndTurn;

	[Header("UI")]
	[SerializeField] private BattleHUD playerHUD;
	[SerializeField] private BattleHUD enemyHUD;
	[SerializeField] private GameObject inventory;
	[SerializeField] private Text dialogueText;
	[SerializeField] private Text turnText;
	

	[Header("Buttons")]
	[SerializeField] private Button attackButton;
	[SerializeField] private Button dodgeButton;
	[SerializeField] private Button ultimateButton;
	[SerializeField] private Button inventoryButton;
	[SerializeField] private Button captureButton;
	[SerializeField] private Button runButton;

	private bool attackButtonState = true;
	private bool dodgeButtonState = true;
	private bool ultimateButtonState = false;
	private bool inventoryButtonState = true;
	private bool captureButtonState = false;
	private bool runButtonState = true;

	private void Start()
    {
        state = BattleState.START;
        turn = 0;
        StartCoroutine(SetupBattle());
    }

	private IEnumerator SetupBattle()
	{
		GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
		playerUnit = playerGO.GetComponentInChildren<CombatPlayer>();
		combatPlayer = playerGO.GetComponentInChildren<CombatPlayer>();

		GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
		enemyUnit = enemyGO.GetComponentInChildren<CombatEnemy>();
		combatEnemy = enemyGO.GetComponentInChildren<CombatEnemy>();


		dialogueText.text = enemyUnit.Name + " You're going to die!";

		playerHUD.SetHUD(playerUnit);
		enemyHUD.SetHUD(enemyUnit);
		
		yield return new WaitForSeconds(2.5f);
	
		state = BattleState.PLAYERTURN;
		PlayerTurn();
	}

	private void PlayerTurn()
	{
		dialogueText.text = "Take that:";
		NextTurn();
	}

	private void EnemyTurn()
    {

		dialogueText.text = enemyUnit.Name + " bastard!";

		//���� � ����� ���������� ���� �� ����� ������������ ������ �����
		if (enemyUnit.CurrentMana.Equals(enemyUnit.MaxMana))
        {
			StartCoroutine(UltimateAttack(enemyUnit, playerUnit, enemyHUD, playerHUD, false));
			return;
        }

		StartCoroutine(Attack(enemyUnit, playerUnit, enemyHUD, playerHUD, false));
    }

	private IEnumerator Attack(CombatEntity attackerUnit, CombatEntity defensiveUnit, BattleHUD attackerHUD, BattleHUD defensiveHUD, bool isPlayerAttacking)
	{
		attackerUnit.Attack();
		string defensiveState = defensiveUnit.TakeDamage(attackerUnit.Damage);
		state = BattleState.ENDTURN;
		yield return new WaitForSeconds(1f);

		switch (defensiveState)
		{
			//������������ ������� ����
			case "hit":
				defensiveHUD.SetHP(defensiveUnit.CurrentHP);
				attackerUnit.AddMana();
				attackerHUD.SetMana(attackerUnit.CurrentMana);

				if (isPlayerAttacking)
                {
					dialogueText.text = "Die creature!";
					yield return new WaitForSeconds(2f);

					state = BattleState.ENEMYTURN;
					EnemyTurn();
				}
                else
                {
					dialogueText.text = "Take that!";
					yield return new WaitForSeconds(1.5f);
					state = BattleState.PLAYERTURN;
					PlayerTurn();
				}
				break;
			//������������ ��������
			case "dodge":
				if (isPlayerAttacking)
				{
					dialogueText.text = "You can't see me!";
					yield return new WaitForSeconds(2f);
					state = BattleState.ENEMYTURN;
					EnemyTurn();
				}
                else
                {
					dialogueText.text = attackerUnit.Name + " won't catch me!";
					yield return new WaitForSeconds(1.5f);
					state = BattleState.PLAYERTURN;
					PlayerTurn();
				}
				break;
			//������������ ����
			case "dead":
				defensiveHUD.SetHP(defensiveUnit.CurrentHP);
				attackerUnit.AddMana();
				attackerHUD.SetMana(attackerUnit.CurrentMana);

				if (isPlayerAttacking)
                {
					dialogueText.text = "Don't mess with me!";
					yield return new WaitForSeconds(2f);
					state = BattleState.WON;
					EndBattle();
				}
                else
                {
					yield return new WaitForSeconds(1f);
					state = BattleState.LOST;
					EndBattle();
				}
				break;
		}
	}
	private IEnumerator UltimateAttack(CombatEntity attackerUnit, CombatEntity defensiveUnit, BattleHUD attackerHUD, BattleHUD defensiveHUD, bool isPlayerAttacking)
	{
		attackerUnit.Attack();
		string defensiveState = defensiveUnit.TakeDamage(attackerUnit.Damage, 1);
		state = BattleState.ENDTURN;
		yield return new WaitForSeconds(1f);

		defensiveHUD.SetHP(defensiveUnit.CurrentHP);
		attackerUnit.CurrentMana -= attackerUnit.CurrentMana;
		attackerHUD.SetMana(attackerUnit.CurrentMana);

		switch (defensiveState)
		{
			//������������ ������� ����
			case "hit":

				if (isPlayerAttacking)
				{
					dialogueText.text = "For glory and honor!";
					yield return new WaitForSeconds(2f);

					state = BattleState.ENEMYTURN;
					EnemyTurn();
				}
				else
				{
					dialogueText.text = "Charge!";
					yield return new WaitForSeconds(1.5f);
					state = BattleState.PLAYERTURN;
					PlayerTurn();
				}
				break;
			//������������ ����
			case "dead":

				if (isPlayerAttacking)
				{
					dialogueText.text = "No retreat, no surrender!";
					yield return new WaitForSeconds(2f);
					state = BattleState.WON;
					EndBattle();
				}
				else
				{
					yield return new WaitForSeconds(1f);
					state = BattleState.LOST;
					EndBattle();
				}
				break;
		}
	}
	private IEnumerator PlayerDodge()
    {
		dodgeEndTurn = combatPlayer.DodgeAbilityON(turn);
		dialogueText.text = "Victory or death!";
		yield return new WaitForSeconds(2f);

		state = BattleState.ENEMYTURN;
		EnemyTurn();
	}

	private void PlayerRun()
    {
		float hpLeft = (enemyUnit.CurrentHP/enemyUnit.MaxHP);
		float escapeChance = Convert.ToSingle(0.1 + (1 - hpLeft) * 0.9);
		Debug.Log(escapeChance);
		var range = UnityEngine.Random.Range(0f, 1f);
		if (range <= escapeChance)
        {
			Debug.Log("Escaped");
			state = BattleState.RUN;
			EndBattle();

		}
        else
        {
			Debug.Log("Fail");
			state = BattleState.ENEMYTURN;
			EnemyTurn();
		}
	}

	private IEnumerator CreatureCapture()
    {

		combatEnemy.CaptureON();
		yield return new WaitForSeconds(1f);
		Inventory.Instance.TryAdd(_creature, 1);
		combatEnemy.CaptureOFF();
		enemyUnit.GetComponentInChildren<SpriteRenderer>().enabled = false;
		state = BattleState.CAPTURE;
		yield return new WaitForSeconds(0.5f);
		EndBattle();
	}

	private void ButtonsState(bool attackButtonState, bool dodgeButtonState, bool ultimateButtonState, bool inventoryButtonState, bool captureButtonState, bool runButtonState)
    {
		attackButton.interactable = attackButtonState;
		dodgeButton.interactable = dodgeButtonState;
		ultimateButton.interactable = ultimateButtonState;
		inventoryButton.interactable = inventoryButtonState;
		captureButton.interactable = captureButtonState;
		runButton.interactable = runButtonState;
	}

	public void NextTurn()
	{
		turn += 1;
		turnText.text = "Round\n" + turn;
		attackButtonState = true;


		if (playerUnit.CurrentMana.Equals(playerUnit.MaxMana))
        {
			ultimateButtonState = true;
		}
		if (enemyUnit.CurrentHP <= (enemyUnit.MaxHP * 0.1))
        {
			captureButtonState = true;
        }

		if (turn.Equals(dodgeEndTurn))
        {
			combatPlayer.DodgeAbilityOFF();
		}

		if (turn >= (dodgeEndTurn + 2))
        {
			dodgeButtonState = true;
        }

		ButtonsState(attackButtonState, dodgeButtonState, ultimateButtonState, inventoryButtonState, captureButtonState, runButtonState);
	}

	[SerializeField] private GameObject returnToWorldButton;
	private void EndBattle()
	{
		switch (state)
        {
			case BattleState.WON:
				dialogueText.text = "Victory!";
				returnToWorldButton.SetActive(true);
				break;
			case BattleState.LOST:
				dialogueText.text = "I'll be back.";
				returnToWorldButton.SetActive(true);
				break;
			case BattleState.CAPTURE:
				dialogueText.text = "Let me go!";
				returnToWorldButton.SetActive(true);
				break;
			case BattleState.RUN:
				dialogueText.text = "Try to catch me!";
				returnToWorldButton.SetActive(true);
				break;
		}
	}

    #region OnButton
    public void OnAttackButton()
	{
		if (state != BattleState.PLAYERTURN)
		{
			return;
		}
		ButtonsState(false, false, false, false, false, false);

		StartCoroutine(Attack(playerUnit, enemyUnit, playerHUD, enemyHUD, true));
	}

	public void OnDodgeButton()
	{
		if (state != BattleState.PLAYERTURN)
		{
			return;
		}
		dodgeButtonState = false;
		ButtonsState(false, false, false, false, false, false);

		StartCoroutine(PlayerDodge());
	}

	public void OnUltimateButton()
	{
		if (state != BattleState.PLAYERTURN)
		{
			return;
		}
		ultimateButtonState = false;
		ButtonsState(false, false, false, false, false, false);

		StartCoroutine(UltimateAttack(playerUnit, enemyUnit, playerHUD, enemyHUD, true));
	}

	public void OnInventoryButton()
	{
		if (state != BattleState.PLAYERTURN)
		{
			return;
		}

		inventory.SetActive(true);		
	}

	public void OnCaptureButton()
	{
		if (state != BattleState.PLAYERTURN)
		{
			return;
		}
		ButtonsState(false, false, false, false, false, false);
		StartCoroutine(CreatureCapture());
	}

	public void OnRunButton()
	{
		if (state != BattleState.PLAYERTURN)
		{
			return;
		}
		ButtonsState(false, false, false, false, false, false);
		PlayerRun();
	}
	#endregion OnButton
}
