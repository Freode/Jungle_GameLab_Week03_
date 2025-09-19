using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MainUI : MonoBehaviour
{

    public TextMeshProUGUI byteText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(UpdateByte());
    }

    IEnumerator UpdateByte()
    {
        while(GameManager.instance.GetGameOver() == false)
        {
            int curValue = GameManager.instance.GetCurByteValue();
            int maxValue = GameManager.instance.GetMaxByteValue();
            byteText.text = "바이트 : " + curValue + " / " + maxValue;
            yield return new WaitForFixedUpdate();
        }
    }
}
