using UnityEngine;
using UnityEngine.EventSystems;

public class DebugAddScoreUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int increment;

    public void OnPointerClick(PointerEventData eventData)
    {
        for (int i = 0; i < increment; i++)
            HUD.Instance.UpdateScore();
    }
}