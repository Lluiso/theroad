using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using DG.Tweening;
public class CarResourcesController : MonoBehaviour
{

    //watch
    [SerializeField]
    private TextMeshProUGUI watchText;
    //gas
    [SerializeField]
    private TextMeshProUGUI gasText;

    [SerializeField]
    private Image gasIndicator;

    //speed
    [SerializeField]
    private TextMeshProUGUI speedText;

    //passengers
    [SerializeField]
    private GameObject[] passengerIcons;

    [SerializeField]
    private Transform speedArrow;


    public int totalTimeInMinutes = 5;
    public int totalKmToEnd = 60;
    public float totalGasInSeconds = 600;

    public float gasMultiplierPerPerson = 0.3f;
    public float startSpeedInKmH = 100;

    private DateTime endTime;
    private DateTime startTime;

    private DateTime lastKmCheck;

    private int kmDone;

    private double gasConsumed;


    private int currentPassengers = 0;



    private void Start()
    {
        startGame();
    }
    void startGame()
    {
        DateTime now = DateTime.Now;
        endTime = now.AddMinutes(totalTimeInMinutes);
        startTime = DateTime.Now;
        lastKmCheck = DateTime.Now;
        gasConsumed = 0;
        kmDone = 0;
        currentPassengers = 0;
        StartCoroutine(Countdown());
        updatePassengersUI();
        updateGasIndicator();

    }

    IEnumerator Countdown()
    {
        while (true)
        {
            Debug.Log("countdown step");
            //watch
            TimeSpan timeToEnd = endTime - DateTime.Now;
            string timeString = string.Format("{0:00}:{1:00}", timeToEnd.Minutes, timeToEnd.Seconds);
            watchText.text = timeString;
            if (DateTime.Compare(endTime, DateTime.Now) <= 0)
            {
                watchText.text = "00:00";
                gameOver();
                yield break;
            }

            //gas
            TimeSpan timeSinceLastCheck = DateTime.Now - lastKmCheck;
            double secondsPassed = timeSinceLastCheck.TotalSeconds;
            gasConsumed += secondsPassed + (currentPassengers * gasMultiplierPerPerson);
            double percentageGas = (totalGasInSeconds - gasConsumed) / totalGasInSeconds;
            gasText.text = Mathf.Round((float)percentageGas * 100) + "%";

            if (totalGasInSeconds - gasConsumed <= 0)
            {
                gasText.text = "0%";
                gameOver();
                yield break;
            }
            //km



            lastKmCheck = DateTime.Now;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    [Button]
    public void addPassenger()
    {
        if (currentPassengers == passengerIcons.Length) return;
        ++currentPassengers;
        updatePassengersUI();
        updateGasIndicator();
    }
    [Button]
    public void kickPassenger()
    {
        if (currentPassengers == 0) return;
        --currentPassengers;
        updatePassengersUI();
        updateGasIndicator();
    }

    void updatePassengersUI()
    {
        for (int i = 0; i < passengerIcons.Length; i++)
        {
            passengerIcons[passengerIcons.Length - 1 - i].GetComponent<Image>().enabled = i < currentPassengers;
        }
    }

    void updateGasIndicator()
    {
        if (currentPassengers <= 1)
        {
            gasIndicator.color = Color.green;

        }
        else if (currentPassengers <= 3)
        {
            gasIndicator.color = Color.yellow;
        }
        else
        {
            gasIndicator.color = Color.red;
        }
    }


    void gameOver()
    {

    }


}
