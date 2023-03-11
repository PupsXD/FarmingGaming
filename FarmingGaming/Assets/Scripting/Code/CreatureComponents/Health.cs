using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public static Health PlayerHealth { get; private set; }
    [SerializeField] private Stats _stats;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Material _hittedMat;
    [SerializeField] private Movement _movement;
    [SerializeField] private Animator _animator;
    public UnityEvent<int> OnHealthChange = new UnityEvent<int>();
    public UnityEvent OnTakeHit = new UnityEvent();
    public UnityEvent OnDeath = new UnityEvent();
    private int _currentHealth;
    private bool _imune;
    private bool _isDead;
    public bool IsDead { get { return _isDead; } }
    public int MaxHealth { get { return _stats.MaxHealth; } }
    public int Current { get { return _currentHealth; } }

    private void Start()
    {
        _currentHealth = _stats.MaxHealth;
        if (transform.CompareTag("Player"))
            PlayerHealth = this;
        //OnHealthChange.Invoke(_currentHealth);
    }
    public void SetStats(Stats stats)
    {
        _stats = stats;
    }

    public bool TakeDamage(int damage, Vector2 hitPower)
    {
        if (_imune || _isDead) return false;
        _currentHealth -= damage;
        _movement.Kick(hitPower);
        OnHealthChange.Invoke(_currentHealth);
        OnTakeHit.Invoke();
        if (_currentHealth <= 0)
        {
            _isDead = true;
            _animator.SetBool("isDead", true);
            OnDeath.Invoke();
            return true;
        }
        else
            StartCoroutine(Imune());
        return false;
    }

    public void Revive(int enemyLevel)
    {
        _currentHealth = _stats.MaxHealth*enemyLevel;
        OnHealthChange.Invoke(_currentHealth);
        _animator.SetBool("isDead", false);
        _isDead = false;
    }

    IEnumerator Imune()
    {
        Material defaultMat = _spriteRenderer.sharedMaterial;
        _imune = true;
        _spriteRenderer.sharedMaterial = _hittedMat;
        for (float t = 0; t < 0.5f;)
        {
            t += Time.deltaTime;
            yield return null;
        }
        _imune = false;
        _spriteRenderer.sharedMaterial = defaultMat;
    }
}
