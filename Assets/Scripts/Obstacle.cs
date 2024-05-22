using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float _leftEdge;
    public bool isPlayer2;

    private void Start()
    {
        if (Camera.main != null) _leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
    }

    private void Update()
    {
        transform.position += Vector3.left * ((isPlayer2 ? GameManager.Instance.GameSpeed2 : GameManager.Instance.GameSpeed) * Time.deltaTime);

        if (transform.position.x < _leftEdge)
        {
            Destroy(gameObject);
        }
    }
}