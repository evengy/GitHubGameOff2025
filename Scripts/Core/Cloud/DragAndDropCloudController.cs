using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class DragAndDropCloudController : Singleton<DragAndDropCloudController>
    {
        [SerializeField] private CloudCore controllableCloud;
        public bool IsCloudSelected { get; private set; }

        private Camera mainCamera;
        private Mouse mouse;
        private Vector3 dragOffset; 
        private Plane dragPlane;

        private void Start()
        {
            mainCamera = Camera.main;
            mouse = Mouse.current;
        }

        private void Update()
        {
            if (mouse == null || mainCamera == null) // TODO also check if upgrades UI is opened
                return;

            HandleSelect();
            HandleDrag();
            HandleDrop();
        }

        private void HandleSelect()
        {
            //if (!mouse.leftButton.wasPressedThisFrame)
            if (!Input.GetMouseButtonDown(0))
                return;

            Ray ray = mainCamera.ScreenPointToRay(mouse.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out CloudCore cloud))
                {
                    if (!cloud.CloudID.Equals(controllableCloud.CloudID))
                    {
                        return;
                    }

                    IsCloudSelected = true;

                    dragPlane = new Plane(Vector3.up, cloud.transform.position);

                    if (dragPlane.Raycast(ray, out float enter))
                    {
                        Vector3 hitPoint = ray.GetPoint(enter);
                        dragOffset = controllableCloud.transform.position - hitPoint;
                    }

                    Debug.Log("Cloud selected");
                }
            }
        }

        private void HandleDrag()
        {
            if (!IsCloudSelected)
                return;

            Ray ray = mainCamera.ScreenPointToRay(mouse.position.ReadValue());

            if (dragPlane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance) + dragOffset;
                controllableCloud.transform.position = hitPoint;
            }
        }

        private void HandleDrop()
        {
            if (mouse.leftButton.wasReleasedThisFrame && IsCloudSelected)
            {
                IsCloudSelected = false;
                Debug.Log("Cloud released");
            }
        }
    }
}
