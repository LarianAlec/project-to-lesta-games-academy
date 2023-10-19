using UnityEngine;

using MovementState = PlayerMovement.MovementState;

public class AnimationControls : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Rigidbody _rb;

    [SerializeField]
    private PlayerMovement _stateManager;

    private static readonly int IsRunning = Animator.StringToHash("is-running");
    private static readonly int IsWalking = Animator.StringToHash("is-walking");
    private static readonly int IsSprinting = Animator.StringToHash("is-sprinting");
    private static readonly int IsInAir = Animator.StringToHash("is-in-air");
    private static readonly int IsMoving = Animator.StringToHash("is-moving");
    private static readonly int YVelocity = Animator.StringToHash("Y-velocity");
    private static readonly int IsWin = Animator.StringToHash("is-win");

    private void Update()
    {
        float yVelocity = _rb.velocity.y;
        _animator.SetFloat(YVelocity, yVelocity);

        // Update animator states
        MovementState state = _stateManager.GetMovementState();
        _animator.SetBool(IsRunning, state == MovementState.running);
        _animator.SetBool(IsWalking, state == MovementState.walking);
        _animator.SetBool(IsSprinting, state == MovementState.sprinting);
        _animator.SetBool(IsInAir, state == MovementState.air);

        _animator.SetBool(IsMoving, _stateManager.IsMoving());

    }

    public void PlayWinDance()
    {
        _animator.SetTrigger(IsWin);
    }
}
