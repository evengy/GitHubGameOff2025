using UnityEngine;

// TODO add transaction back to menu on LMB
public class CreditsScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 150f;

    void Update()
    {
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        if (transform.position.y > + 4500)
        {
            GetComponent<Animator>().SetTrigger("Transition");
        }
    }

    public void TransitionBackToMenu()
    {
        GetComponent<Animator>().ResetTrigger("Transition");
        MenuUIManager.Instance.ShowMenu();
    }

    public void ResetPosition()
    {
        transform.position = Vector3.zero;
    }
}
