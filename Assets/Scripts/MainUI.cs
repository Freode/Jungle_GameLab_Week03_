using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MainUI : MonoBehaviour
{

    public TextMeshProUGUI byteText;
    public TextMeshProUGUI planetText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(UpdateByte());
        GameManager.instance.OnPlanetChanged += OnPlanetChanged;
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

    // 행성 이름 변경
    private void OnPlanetChanged()
    {
        string name = GameManager.instance.GetCurStageName();
        planetText.text = "현재 행성 : " + name;
    }
}
