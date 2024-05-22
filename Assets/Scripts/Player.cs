using System.Collections;
using Unity.VisualScripting;
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
    public bool isGrounded;
    
    public float timeleft = 10;
    public int lives;
    
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
        lives = 3;
        if(GameManager.Instance.livesPlayer2) GameManager.Instance.livesPlayer2.text = lives.ToString();
        if(GameManager.Instance.livesPlayer1) GameManager.Instance.livesPlayer1.text = lives.ToString();
    }

    

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, isGod ? 1f : .5f);
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
        if (other.CompareTag("SuperEgg"))
        {
            StopAllCoroutines();
            StartCoroutine(GodMode());
            return;
        }
        
        if (!GameManager.Instance.isMultiplayer)
        {
            if (other.CompareTag("Obstacle") && isGod == false)
            {
                GameManager.Instance.GameOver();
            }
        }
        else
        {
            if (other.CompareTag("Obstacle") && lives != 0 && !isGod)
            {
                lives--;
                if(other.GetComponent<Obstacle>().isPlayer2)
                {
                    GameManager.Instance.GameSpeed2 = GameManager.Instance.initialGameSpeed;
                    GameManager.Instance.livesPlayer2.text = lives.ToString();
                }
                else
                {
                    GameManager.Instance.GameSpeed = GameManager.Instance.initialGameSpeed;
                    GameManager.Instance.livesPlayer1.text = lives.ToString();
                }
            }
            
            else if(!isGod)
            {
                GameManager.Instance.GameOver();
            }
            
            if (!isGod)
            {
                Destroy(other.gameObject);
            }
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
        timeleft = 10;
        transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        transform.position = new Vector3(transform.position.x, 0.18f);
        isGod = true;

        yield return new WaitForSeconds(10);

        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.position = new Vector3(transform.position.x, 0f);
        isGod = false;
    }
}
