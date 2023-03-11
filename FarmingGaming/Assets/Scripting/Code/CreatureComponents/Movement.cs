using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{
    [SerializeField] private Stats _stats;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    [HideInInspector] public UnityEvent<bool> OnFlip = new UnityEvent<bool>();
    private bool _movementMuted;
    private Vector2 _input;
    private bool _flipControllMuted;
    private bool _flipped;
    public bool Flipped
    {
        get { return _flipped; }
    }
    public bool MovementMuted
    {
        get { return _movementMuted; }
        set { _movementMuted = value; }
    }
    public Animator Animator 
    { get { return _animator; } }
    protected virtual float MoveIncreaseFactor
    {
        get { return _level; }
    }

    private int _level = 1;

    /// <summary>
    /// Set new stats
    /// </summary>
    /// <param name="stats"></param>
    public void SetStats(Stats stats)
    {
        _stats = stats;
    }
    /// <summary>
    /// Set move input
    /// </summary>
    /// <param name="moveVector"></param>
    public void Move(Vector2 moveVector)
    {
        _input = moveVector;
    }

    /// <summary>
    /// You can push creature with this method in choosen direction
    /// </summary>
    /// <param name="force"></param>
    public void Kick(Vector2 force)
    {
        _rigidbody.AddForce(force, ForceMode2D.Impulse);
        _movementMuted = true;
        StartCoroutine(KickProcess());
    }
    /// <summary>
    /// Flip creatures graphics
    /// </summary>
    /// <param name="value"></param>
    public void ForceFlip(bool value)
    {
        _spriteRenderer.flipX = value;
        _flipped = value;
        OnFlip.Invoke(value);
    }
    /// <summary>
    /// Block auto fliping
    /// </summary>
    public void TakeoverOfFlipControll()
    {
        _flipControllMuted = true;
    }
    /// <summary>
    /// Continue auto fliping
    /// </summary>
    public void ReleaseFlipControll()
    {
        _flipControllMuted = false;
    }

    /// <summary>
    /// Enable/Disable function
    /// </summary>
    /// <param name="value"></param>
    /// <param name="level"></param>
    public void SetActive(bool value, int level)
    {
        if (value)
        {
            _animator.enabled = true;
            _movementMuted = false;
            _level = level;
        }
        else
        {
            _input = Vector2.zero;
            _flipControllMuted = false;
            _flipped = false;
            _spriteRenderer.flipX = false;
            _animator.SetFloat("velocity", 0);
            _movementMuted = true;
        }
    }

    private void FixedUpdate()
    {
        if (!_movementMuted)
        {
            _rigidbody.MovePosition(_rigidbody.position + _input * _stats.MoveSpeed * MoveIncreaseFactor * Time.fixedDeltaTime);
            _animator.SetFloat("velocity", _input.magnitude);
            if (!_flipControllMuted) TryFlip();
        }
    }
    private void TryFlip()
    {
        if (_flipped && _input.x > 0)
        {
            _flipped = false;
            _spriteRenderer.flipX = _flipped;
            OnFlip.Invoke(_flipped);
        }
        else if (!_flipped && _input.x < 0)
        {
            _flipped = true;
            _spriteRenderer.flipX = _flipped;
            OnFlip.Invoke(_flipped);
        }
    }
    IEnumerator KickProcess()
    {
        while(_rigidbody.velocity.magnitude > 0.5f)
        {
            yield return null;
        }
        if(_animator.enabled)
            _movementMuted = false;
    }
}
