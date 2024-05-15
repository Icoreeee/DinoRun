using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private CharacterController _character;
    private Vector3 _direction;
    private Animator _animator;
    public bool isGod;
    public KeyCode jumpBtn;
    public KeyCode crouchBtn;
    

    public float gravity = 9.81f * 2f;
    public float jumpForce = 8f;
    private void Awake()
    {
        _character = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _direction = Vector3.zero;
    }

    public void OnRunning()
    {
        _animator.SetBool("isMoving", true);
    }

    public bool isGrounded;

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, .5f);
    }
    private void Update()
    {
        _direction += Vector3.down * (gravity * Time.deltaTime);

        isGrounded = IsGrounded();
        if (isGrounded)
        {
            if (Input.GetKeyDown(jumpBtn))
            {
                _direction = Vector3.up * jumpForce;
            }

            if (Input.GetKeyDown(crouchBtn))
            {
                _animator.SetBool("isCrouching", true);
                _character.radius = 0.31f;
                _character.height = 0f;
                transform.position = new Vector3(transform.position.x, -0.19f);
            }
            
            if (Input.GetKeyUp(crouchBtn))
            {
                _animator.SetBool("isCrouching", false);
                _character.radius = 0.44f;
                _character.height = 0.96f;
                transform.position = new Vector3(transform.position.x, 0f);
            }
        }

        _character.Move(_direction * Time.deltaTime);
    }

    public void StopAnimation()
    {
        _animator.SetBool("isMoving", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") && isGod == false)
        {
            GameManager.Instance.GameOver();
        }

        if (other.CompareTag("SuperEgg"))
        {
            StartCoroutine(GodMode());
        }
    }

    public void ResetGodMode()
    {
        StopAllCoroutines();
        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.position = new Vector3(transform.position.x, 0f);
        isGod = false;
    }
    
    IEnumerator GodMode()
    {
        GameManager.Instance.timeLeft = 10;
        transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        transform.position = new Vector3(transform.position.x, 0.18f);
        isGod = true;

        yield return new WaitForSeconds(10);

        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.position = new Vector3(transform.position.x, 0f);
        isGod = false;
    }
}
