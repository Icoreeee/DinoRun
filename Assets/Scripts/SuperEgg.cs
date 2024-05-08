using UnityEngine;

public class SuperEgg : MonoBehaviour
{
    private float _leftEdge;

    private void Start()
    {
        if (Camera.main != null) _leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
    }

    private void Update()
    {
        transform.position += Vector3.left * (GameManager.Instance.GameSpeed * Time.deltaTime);

        if (transform.position.x < _leftEdge)
        {
            Destroy(gameObject);
        }
    }
}