using UnityEngine;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _ui;
    // Start is called before the first frame update
    void Awake()
    {
        _ui.SetActive(true);
        _button.onClick.AddListener(() => {
            GameEvents.StartGame?.Invoke();
            _ui.SetActive(false);
        });
    }
}
