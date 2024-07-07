using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class typewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI displayText;  
    public string fullText;
    private float delay = 0.05f;
    private bool isEffectActive = false;

    private void Update()
    {
        if (displayText.gameObject.activeInHierarchy && !isEffectActive)
        {
            isEffectActive = true;
            Time.timeScale = 1;
            StartCoroutine(ShowText());
        }
        else if (!displayText.gameObject.activeInHierarchy)
        {
            isEffectActive = false;
        }
    }

    private IEnumerator ShowText()
    {
        displayText.text = "";
        for (int i = 0; i <= fullText.Length; i++)
        {
            displayText.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(delay);
        }
    }
}
