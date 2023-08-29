using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LoadingText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI loadingText;
    [SerializeField] float updateTime;

    int periods = 0;
    bool increase = true;

    readonly string displayText0 = "Loading";
    readonly string displayText1 = "Loading.";
    readonly string displayText2 = "Loading..";
    readonly string displayText3 = "Loading...";

    private void Start()
    {
        StartCoroutine(UpdateDisplayText());
    }

    private IEnumerator UpdateDisplayText()
    {
        increase = true;

        while (true)
        {
            yield return new WaitForSeconds(updateTime);

            if (increase)
                periods++;
            else
                periods--;

            if (periods >= 3 || periods <= 0)
                increase = !increase;

            string text = periods switch
            {
                1 => displayText1,
                2 => displayText2,
                3 => displayText3,
                _ => displayText0,
            };

            loadingText.text = text;
        }
        
    }
}
