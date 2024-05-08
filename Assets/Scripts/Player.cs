using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private CharacterController _character;
    private Vector3 _direction;
    private Animator _animator;
    

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

    private void Update()
    {
        _direction += Vector3.down * (gravity * Time.deltaTime);

        if (_character.isGrounded)
        {
            _direction = Vector3.down;


            if (Input.GetButton("Jump"))
            {
                _direction = Vector3.up * jumpForce;
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                _animator.SetBool("isCrouching", true);
                _character.radius = 0.31f;
                _character.height = 0f;
                transform.position = new Vector3(transform.position.x, -0.19f);
            }
            
            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                _animator.SetBool("isCrouching", false);
                _character.radius = 0.44f;
                _character.height = 0.96f;
                transform.position = new Vector3(transform.position.x, 0f);
            }
        }

        _character.Move(_direction * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        _animator.SetBool("isMoving", false);
        if (other.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
        }
    }
}
