using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MainUI : MonoBehaviour
{
    public TextMeshProUGUI byteText;
    public TextMeshProUGUI planetText;
    public TextMeshProUGUI timerText;

    private float elapsedTime = 0f;

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
            // 바이트 출력
            int curValue = GameManager.instance.GetCurByteValue();
            int maxValue = GameManager.instance.GetMaxByteValue();
            byteText.text = "바이트 : " + curValue + " / " + maxValue;

            // 타이머 출력
            elapsedTime += Time.deltaTime;

            int minutes = (int)(elapsedTime / 60);
            int seconds = (int)(elapsedTime % 60);

            timerText.text = string.Format("진행 시간 - {0:00}:{1:00}", minutes, seconds);

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
