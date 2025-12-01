using UnityEngine;

public class ScorePoint : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    private void Start()
    {
        canvas.worldCamera = Camera.main;
    }

    public void Disolve()
    {
        Destroy(gameObject);
    }
}