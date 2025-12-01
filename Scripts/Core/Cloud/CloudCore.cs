using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public class CloudCore : MonoBehaviour
    {
        [Header("Cloud")]
        [SerializeField] GameObject rain;
        [SerializeField] GameObject mesh;
        [SerializeField] GameObject psDisolveEffect;

        ParticleSystem.ShapeModule shape;
        ParticleSystem.EmissionModule emission;

        private bool isDefaultState;

        public GUID CloudID { get; private set; }

        private void Start()
        {
            CloudID = GUID.Generate();

            isDefaultState = true;
            shape = rain.GetComponent<ParticleSystem>().shape;
            emission = rain.GetComponent<ParticleSystem>().emission;

            GameStateManager.Instance.StateUpdated += Instance_StateUpdated;
        }

        private void Instance_StateUpdated()
        {
            if (GameStateManager.Instance.GameState.Equals(GameState.InGameOver))
            {
                rain.SetActive(false);
                mesh.SetActive(false);
                psDisolveEffect.SetActive(true);
            }
            else if (GameStateManager.Instance.GameState.Equals(GameState.InGame))
            {
                rain.SetActive(true);
                mesh.SetActive(true);
                psDisolveEffect.SetActive(false);
            }
        }

        private void Update()
        {
            if (AbilitiesUI.Instance.IsActive<AbilityOrangeWarning>() && AbilityOrangeWarning.Instance.IsActive)
            {
                shape.scale = new Vector3(1f, 1f, 10f) * IncrementalUpgradesManager.Instance.CloudCurrentSize * 2;
                gameObject.transform.localScale = new Vector3(
                     IncrementalUpgradesManager.Instance.CloudCurrentSize * 2,
                     1f,
                     IncrementalUpgradesManager.Instance.CloudCurrentSize * 2);
                
                emission.rateOverTimeMultiplier = 100;
                isDefaultState = false;
            }
            else if (!isDefaultState)
            {
                shape.scale = new Vector3(1f, 1f, 10f) * IncrementalUpgradesManager.Instance.CloudCurrentSize;
                gameObject.transform.localScale = new Vector3(
                     IncrementalUpgradesManager.Instance.CloudCurrentSize,
                     1f,
                     IncrementalUpgradesManager.Instance.CloudCurrentSize);
                
                emission.rateOverTimeMultiplier = 25;
                isDefaultState = true;
            }
        }
    }
}
