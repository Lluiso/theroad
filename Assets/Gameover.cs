using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class Gameover : MonoBehaviour
{
	public static Action OnTryAgain;
	[SerializeField] private GameObject _container;
	[SerializeField] private TextMeshProUGUI _reasonText;

	private void Awake()
	{
		_container.SetActive(false);
		CarResourcesController.OnGameOver += ShowGameOver;
	}

	[Button("Force game over")]
	void ForceGameOver()
	{
		ShowGameOver("forced");
	}

	void ShowGameOver(string reason)
	{
		_reasonText.text = reason;
		_container.SetActive(true);
	}

	public void TryAgainPressed()
	{
		OnTryAgain?.Invoke();
		_container.SetActive(false);
	}
}