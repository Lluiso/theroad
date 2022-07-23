using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechEffects : MonoBehaviour
{
    [SerializeField] private List<GameObject> _effectsPrefabs;
    [SerializeField] private Transform _spawnPoint;
    private Dictionary<string, GameObject> _fxMap;

    private void Awake()
    {
        _fxMap = new Dictionary<string, GameObject>();
        foreach (var effect in _effectsPrefabs)
        {
            _fxMap[effect.name] = effect;
        }
    }

    public void DoEffect(string effectName)
    {
        var fx = Instantiate(_fxMap[effectName], _spawnPoint);
        fx.transform.localScale = Vector3.zero;
    }
}
