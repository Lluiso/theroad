using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetText : MonoBehaviour
{
	[SerializeField] private TextMeshPro _text;

	public void Set(string newText)
	{
		_text.text = newText;
	}
}