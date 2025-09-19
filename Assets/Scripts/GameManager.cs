using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Planet
{ 
    Sylva = 0,
    Desolo,
    Glacio,
    Atrox
}


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public event System.Action<int> OnByteTextValueChanged;

    public List<int> reduceBytes;           // 행생에 따른 바이트 소모량 차별화
    public float byteZeroTimeLimit = 15f;     // 바이트가 0으로 유지되는 한계점

    private Planet stage = Planet.Sylva;    // 현재 스테이지
    private int byteValue = 60;             // 현재 바이트 소유량
    private float byteZeroTime = 0f;        // 바이트가 0으로 유지되는 시간

    private bool isGameStart = false;       // 게임이 시작되었는지 확인
    private bool isGameOver = false;        // 게임이 종료되었는지 확인

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Init());
    }

    // 일정 시간 지난 후에 초기화
    IEnumerator Init()
    {
        yield return new WaitForSeconds(0.3f);

        isGameStart = true;
        OnByteTextValueChanged?.Invoke(byteValue);
        StartCoroutine(ReduceByte());
    }

    // 매 1초마다 행성에 맞는 바이트 수 감소
    IEnumerator ReduceByte()
    {
        int stageNum = (int)stage;
        while(isGameOver == false)
        {
            yield return new WaitForSeconds(1f);

            AddByteValue(-1 * reduceBytes[stageNum]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver == true || isGameStart == false)
            return;

        CheckByteZero();
    }

    // stage Setter & Getter
    public void SetStage(Planet toStage)
    {
        stage = toStage;
    }

    public Planet GetStage()
    {
        return stage;
    }

    // Byte 소유량 설정 및 반환
    public void AddByteValue(int val)
    {
        byteValue += val;
        //if (byteValue < 0)
        //    byteValue = 0;

        // 바이트 변경에 따른 업데이트
        OnByteTextValueChanged?.Invoke(byteValue);
    }

    public int GetByteValue()
    {
        return byteValue;
    }

    // 몇 초간 바이트가 0으로 유지되는지 확인
    private void CheckByteZero()
    {
        // 바이트가 0 이하로 유지되는 시간 측정
        if (byteValue <= 0)
            byteZeroTime += Time.deltaTime;
        else
            byteZeroTime = 0f;

        // 한계값보다 오래 지속되면, 게임 오버
        if (byteZeroTime >= byteZeroTimeLimit)
            GameOver();
    }

    // 게임 오버
    private void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
    }

}
