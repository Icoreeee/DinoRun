using UnityEngine;

public class Ground : MonoBehaviour
{
    public bool isPlayer2;
    private MeshRenderer _meshRenderer;
    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (isPlayer2)
        {
            float speed2 = GameManager.Instance.GameSpeed2 / transform.localScale.x;
            _meshRenderer.material.mainTextureOffset += Vector2.right * (speed2 * Time.deltaTime);
        }
        else
        {
            float speed = GameManager.Instance.GameSpeed / transform.localScale.x;
            _meshRenderer.material.mainTextureOffset += Vector2.right * (speed * Time.deltaTime);
        }
    }
}