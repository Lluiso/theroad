using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Sirenix.OdinInspector;
public class CarResourcesController : MonoBehaviour
{

    //watch
    [SerializeField]
    private TextMeshProUGUI watchText;
    //gas
    [SerializeField]
    private TextMeshProUGUI gasText;

    [SerializeField]
    private Transform gasArrow;

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


            //km



            lastKmCheck = DateTime.Now;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    [Button]
    public void addPassenger()
    {
        ++currentPassengers;
        updatePassengersUI();
    }
    [Button]
    public void kickPassenger()
    {
        --currentPassengers;
        updatePassengersUI();
    }

    void updatePassengersUI()
    {
        for (int i = 0; i < passengerIcons.Length; i++)
        {
            passengerIcons[i].SetActive(i < currentPassengers);
        }
    }


    void gameOver()
    {

    }


}
