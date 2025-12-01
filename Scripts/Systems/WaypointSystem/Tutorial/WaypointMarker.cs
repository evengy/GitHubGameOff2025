using UnityEngine;

public class WaypointMarker : MonoBehaviour
{
    [SerializeField] private GameObject mesh;

    public void Disolve()
    {
        Destroy(gameObject);
    }

    public void FlipMesh()
    {
        mesh.transform.localEulerAngles = new Vector3(0, 180, 0);
    }
}