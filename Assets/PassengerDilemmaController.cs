using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class PassengerDilemmaController : MonoBehaviour
{
    [SerializeField] GameObject passengerPrefab;
    [SerializeField] Transform[] outsideSlots;
    [SerializeField] Transform[] insideSlots;
    [SerializeField] GameObject screen;


    [SerializeField] Sprite harveySprite;
    [SerializeField] Sprite carolSprite;
    [SerializeField] Sprite rosieSprite;
    [SerializeField] Sprite salimaSprite;
    [SerializeField] Sprite chuckSprite;
    [SerializeField] Sprite peterSprite;


    private List<GameObject> passengers;

    private void Awake()
    {
        screen.SetActive(false);
        CarEvents.ShowDilemma += SetPassengers;
    }

    [Button]
    public void testPassengers()
    {
        SetPassengers(new List<string> { "Harvey", "Salima" }, new List<string> { "Chuck" });
    }

    public void SetPassengers(List<string> insideCarPassengers, List<string> outsideCarPassengers)
    {
        passengers = new List<GameObject>();

        for (int i = 0; i < insideCarPassengers.Count; i++)
        {
            GameObject newPassenger = GameObject.Instantiate(passengerPrefab, screen.transform);
            newPassenger.transform.DOMove(insideSlots[i].position, 0f);
            newPassenger.GetComponent<Passenger>().setInfo(true, insideCarPassengers[i], getSpriteFromName(insideCarPassengers[i]), GetComponent<Canvas>());
            passengers.Add(newPassenger);
        }

        for (int i = 0; i < outsideCarPassengers.Count; i++)
        {
            GameObject newPassenger = GameObject.Instantiate(passengerPrefab, screen.transform);
            newPassenger.transform.DOMove(outsideSlots[i].position, 0f);
            newPassenger.GetComponent<Passenger>().setInfo(false, outsideCarPassengers[i], getSpriteFromName(outsideCarPassengers[i]), GetComponent<Canvas>());
            passengers.Add(newPassenger);
        }
        screen.SetActive(true);
    }

    public void confirmPressed()
    {
        var Ps = passengers.Select(p => p.GetComponent<Passenger>());
        var inCarPassengers = Ps.Where(p => p.isInsideCar).Select(p => p._name).ToList();
        var outCarPassengers = Ps.Where(p => !p.isInsideCar).Select(p => p._name).ToList();

        CarEvents.MovingOff?.Invoke(inCarPassengers, outCarPassengers);
        //return list somewhere
        for (var i=0; i<passengers.Count; i++) {
            Destroy(passengers[i].gameObject);
        }
        screen.SetActive(false);
    }

    public Sprite getSpriteFromName(string name)
    {
        switch (name)
        {
            case "Harvey": return harveySprite;
            case "Carol": return carolSprite;
            case "Rosie": return rosieSprite;
            case "Salima": return salimaSprite;
            case "Chuck": return chuckSprite;
            case "Peter": return peterSprite;
            default: return harveySprite;
        }
    }
}
