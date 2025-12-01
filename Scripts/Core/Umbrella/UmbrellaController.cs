using UnityEngine;

public class UmbrellaController: MonoBehaviour
{
	[SerializeField] GameObject scorePoint;
	public bool IsOpened {  get; private set; }

    public void Open()
	{
		if (IsOpened) return;
		
		IsOpened = true;

		HUD.Instance.UpdateScore();
		Instantiate(scorePoint, transform.position, Quaternion.Euler(Vector3.zero));
        GetComponent<AudioSource>()?.Play();
	}
}