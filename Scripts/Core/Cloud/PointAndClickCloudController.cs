using Assets.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointAndClickCloudController : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject markerPrefab;
    [SerializeField] private CloudCore controllableCloud;

    private Camera mainCamera;
    private GameObject cloudMarker;
    private Vector3 targetPosition;
    private bool isMoving;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void HandleCloudMovement()
    {
        if (isMoving)
        {
            if (DragAndDropCloudController.Instance.IsCloudSelected)
            {
                isMoving = false;
                cloudMarker.GetComponent<CloudNavigationMarker>().Disolve();
                cloudMarker = null;
                return;
            }

            controllableCloud.transform.position = Vector3.MoveTowards(
                controllableCloud.transform.position,
                targetPosition,
                IncrementalUpgradesManager.Instance.CloudCurrentSpeed * Time.deltaTime);

            if (Vector3.Distance(controllableCloud.transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
                cloudMarker.GetComponent<CloudNavigationMarker>().Disolve();
                cloudMarker = null;

                controllableCloud.transform.position = targetPosition;
                isMoving = false;
            }
        }
    }

    private void HandleMarkerPositioning()
    {
        //if (Mouse.current.leftButton.wasPressedThisFrame && GameStateManager.Instance.GameState.Equals(GameState.InWave))
        if (Input.GetMouseButtonDown(0) && WaveStateManager.Instance.WaveState.Equals(WaveState.InWave))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject() || DragAndDropCloudController.Instance.IsCloudSelected)
            {
            }
            else
            {
                Vector2 mousePos = Input.mousePosition;
                Ray ray = mainCamera.ScreenPointToRay(mousePos);

                if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundLayer))
                {
                    targetPosition = hit.point;
                    targetPosition.y = controllableCloud.transform.position.y;
                    isMoving = true;
                    cloudMarker?.GetComponent<CloudNavigationMarker>().Disolve();
                    cloudMarker = Instantiate(markerPrefab, targetPosition, Quaternion.identity);
                }
            }
        }
    }
   
    private void Update()
    {
        HandleMarkerPositioning();
        HandleCloudMovement();
    }
}
