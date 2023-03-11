using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, ENDTURN, WON, LOST, CAPTURE, RUN }

public class CombatSystem : MonoBehaviour
{
	[SerializeField] private BattleState state;

	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private GameObject enemyPrefab;

	[SerializeField] private Transform playerBattleStation;
	[SerializeField] private Transform enemyBattleStation;

	private CombatEntity enemyUnit;
	private CombatEntity playerUnit;
	private CombatPlayer combatPlayer;

	private int turn;
	private int dodgeEndTurn;

	[Header("UI")]
	[SerializeField] private BattleHUD playerHUD;
	[SerializeField] private BattleHUD enemyHUD;
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

		dialogueText.text = enemyUnit.Name + " ����������� ��� ����...";

		playerHUD.SetHUD(playerUnit);
		enemyHUD.SetHUD(enemyUnit);

		yield return new WaitForSeconds(2.5f);

		state = BattleState.PLAYERTURN;
		PlayerTurn();
	}

	private void PlayerTurn()
	{
		dialogueText.text = "�������� ��������:";
		NextTurn();
	}

	private void EnemyTurn()
    {

		dialogueText.text = enemyUnit.Name + " �������!";

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
					dialogueText.text = "�������� �����!";
					yield return new WaitForSeconds(2f);

					state = BattleState.ENEMYTURN;
					EnemyTurn();
				}
                else
                {
					dialogueText.text = "��� ������!";
					yield return new WaitForSeconds(1.5f);
					state = BattleState.PLAYERTURN;
					PlayerTurn();
				}
				break;
			//������������ ��������
			case "dodge":
				if (isPlayerAttacking)
				{
					dialogueText.text = defensiveUnit.Name + " ����������!";
					yield return new WaitForSeconds(2f);
					state = BattleState.ENEMYTURN;
					EnemyTurn();
				}
                else
                {
					dialogueText.text = "�� ������� ����������!";
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
					dialogueText.text = "�� �������� ��������� ����!";
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
					dialogueText.text = "�� ��������� ��������� �����!";
					yield return new WaitForSeconds(2f);

					state = BattleState.ENEMYTURN;
					EnemyTurn();
				}
				else
				{
					dialogueText.text = "��� �������� ������!";
					yield return new WaitForSeconds(1.5f);
					state = BattleState.PLAYERTURN;
					PlayerTurn();
				}
				break;
			//������������ ����
			case "dead":

				if (isPlayerAttacking)
				{
					dialogueText.text = "�� �������� ����������� ����!";
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
		dialogueText.text = "�� ���������������� �� ��������� ����� � ������ �� �����������.";
		yield return new WaitForSeconds(2f);

		state = BattleState.ENEMYTURN;
		EnemyTurn();
	}

	private void PlayerRun()
    {
		float hpLeft = (enemyUnit.CurrentHP/enemyUnit.MaxHP) * 100;
		Debug.Log("perc hp left  " + hpLeft);
		
		//Maybe next time))))
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
		turnText.text = "���\n" + turn;
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

	private void EndBattle()
	{
		switch (state)
        {
			case BattleState.WON:
				dialogueText.text = "�� ��������!";
				break;
			case BattleState.LOST:
				dialogueText.text = "�� ���������.";
				break;
			case BattleState.CAPTURE:
				dialogueText.text = "�� ��������� ��������!.";
				break;
			case BattleState.RUN:
				dialogueText.text = "�� ������ ������� �� �����.";
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
	}

	public void OnCaptureButton()
	{
		if (state != BattleState.PLAYERTURN)
		{
			return;
		}
		ButtonsState(false, false, false, false, false, false);
	}

	public void OnRunButton()
	{
		if (state != BattleState.PLAYERTURN)
		{
			return;
		}
		//ButtonsState(false, false, false, false, false, false);
		PlayerRun();
	}
	#endregion OnButton
}
