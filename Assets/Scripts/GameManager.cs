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

    public bool isTestMode = false;         // 테스트 모드

    public event System.Action<int, int> OnByteTextValueChanged;
    public event System.Action OnPlanetChanged;

    public List<int> reduceBytes;           // 행성에 따른 바이트 소모량 차별화
    public float byteZeroTimeLimit = 15f;   // 바이트가 0으로 유지되는 한계점

    public GameObject planetFloor;          // 행성 바닥
    public List<Material> planetMaterials;  // 행성 바닥 머티리얼

    private Planet stage = Planet.Sylva;    // 현재 스테이지
    private string stageName = "실바";      // 현재 스테이지의 이름
    private int curByteValue = 60;          // 현재 바이트 소유량
    private int maxByteValue = 150;         // 최대 바이트 소유량
    private float byteZeroTime = 0f;        // 바이트가 0으로 유지되는 시간

    private bool isGameStart = false;       // 게임이 시작되었는지 확인
    private bool isGameOver = false;        // 게임이 종료되었는지 확인

    private int reduceBytesCoverValue = 0;  // 현재 감소량 커버할 수 있는 양
    private int increaseBytesValue = 0;     // 매 초마다 증가하는 바이트 양
    private int findBytesRate = 0;          // 바이트 발견 시, 발견량을 최대 n% 증가
    private float findByteMinPeriod = 10f;  // 바이트 최소 발견 주기
    private float findByteMaxPeriod = 30f;  // 바이트 최대 발견 주기
    private int increaseFindBytMinValue = 0;// 바이트 최소 발견량 증가
    private bool canGoDesolo = false;       // 데솔로로 이동 가능 여부
    private bool canGoGlaclo = false;       // 글라시오로 이동 가능 여부
    private bool canGoAtrox = false;        // 아트록스로 이동 가능 여부

    public List<bool> corePurchaseList;     // 코어 구매 목록들
    public GameObject structureParent;      // 구조물의 부모 프리팹
    private List<GameObject> structures;    // 활성화할 구조물 목록들

    private void Awake()
    {
        instance = this;
        SetTestMode();
    }

    // 테스트 모드
    private void SetTestMode()
    {
        if (isTestMode == false)
            return;

        maxByteValue = 300000;
        curByteValue = 300000;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Init());

        findAllStructures();
    }

    // 모든 구조물을 찾기
    private void findAllStructures()
    {
        structures = new List<GameObject>();
        int count = structureParent.transform.childCount;

        for(int i = 0; i < count; i++)
        {
            Transform child = structureParent.transform.GetChild(i);
            structures.Add(child.gameObject);
        }
    }

    // 일정 시간 지난 후에 초기화
    IEnumerator Init()
    {
        yield return new WaitForSeconds(0.3f);

        isGameStart = true;
        OnByteTextValueChanged?.Invoke(curByteValue, maxByteValue);
        StartCoroutine(ReduceByte());
    }

    // 매 1초마다 행성에 맞는 바이트 수 감소
    IEnumerator ReduceByte()
    {
        while (isGameOver == false)
        {
            yield return new WaitForSeconds(1f);

            CalculateAddBytePerSecond();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver == true || isGameStart == false)
            return;

        CheckByteZero();
    }

    private void CalculateAddBytePerSecond()
    {
        int result = 0;
        int stageNum = (int)stage;
        int reduceByStage = reduceBytes[stageNum];

        // 감소량 커버는 0까지 가능
        result = Mathf.Min(-1 * reduceByStage + reduceBytesCoverValue, 0);

        // 매 초 증가량 추가
        result += increaseBytesValue;

        AddCurByteValue(result);
    }

    // 현재 Byte 소유량 설정 및 반환
    public void AddCurByteValue(int val)
    {
        curByteValue += val;

        // 최소량은 0
        if (curByteValue < 0)
            curByteValue = 0;

        // 최대량
        if (curByteValue > maxByteValue)
            curByteValue = maxByteValue;

        // 바이트 변경에 따른 업데이트
        OnByteTextValueChanged?.Invoke(curByteValue, maxByteValue);
    }

    public int GetCurByteValue() { return curByteValue; }

    // 최대 Byte 소유량 설정 및 반환
    public void AddMaxByteValue(int val)
    {
        maxByteValue += val;

        // 바이트 변경에 따른 업데이트
        OnByteTextValueChanged?.Invoke(curByteValue, maxByteValue);
    }

    // 현재 스테이지 설정
    public void SetCurrentPlanet(Planet newPlanet)
    {
        if (stage == newPlanet)
            return;

        SetStage(newPlanet);
        switch(newPlanet)
        {
            case Planet.Sylva:
                stageName = "실바";
                break;
            case Planet.Desolo:
                stageName = "데슬로";
                break;
            case Planet.Glacio:
                stageName = "글라시오";
                break;
            case Planet.Atrox:
                stageName = "아트록스";
                break;
        }

        // 행성 바닥 색상 변경
        MeshRenderer meshRenderer = planetFloor.GetComponent<MeshRenderer>();
        meshRenderer.material = planetMaterials[(int)stage];

        OnPlanetChanged?.Invoke();
    }

    // 코어 구매 전송
    public void PurchaseCore(Planet plaentCore)
    {
        int idx = (int)plaentCore;
        corePurchaseList[idx] = true;

        // 모두 구매했는지 확인
        bool result = true;
        foreach(bool corePurchaseEach in corePurchaseList)
        {
            if(corePurchaseEach == false)
            {
                result = false;
                break;
            }
        }

        // 게임 클리어
        if(result)
        {
            Debug.Log("게임 클리어!");
        }
    }

    // 활성화된 건물 등장 효과 실행
    public void ActiveStructures(GameObject structurePrefab)
    {
        foreach(GameObject structure in structures)
        {
            if (structure.name.Contains(structurePrefab.name) == false)
                continue;

            structure.transform.localScale = Vector3.zero;
            structure.SetActive(true);
            StartCoroutine(StructureScaleUp(structure));
        }
    }

    // 건물 등장 효과 재생
    IEnumerator StructureScaleUp(GameObject structure)
    {
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;

        float elapsedTime = 0f;
        float activeStructureDuration = 1f;

        while(elapsedTime < activeStructureDuration)
        {
            structure.transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / activeStructureDuration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        structure.transform.localScale = endScale;
    }

    // 몇 초간 바이트가 0으로 유지되는지 확인
    private void CheckByteZero()
    {
        // 바이트가 0 이하로 유지되는 시간 측정
        if (curByteValue <= 0)
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
        Debug.Log("게임 오버!");
        isGameOver = true;
        Time.timeScale = 0f;
    }

    // ==============================================
    //                  수치 변경
    // ==============================================

    // 현재 감소량 커버할 수 있는 양
    public void AddReduceBytesCoverValue(int val) { reduceBytesCoverValue += val; }
    // 매 초마다 증가하는 바이트 양
    public void AddIncreaseBytesValue(int val) { increaseBytesValue += val; }

    // 바이트 발견 시, 발견량을 최대 n% 증가
    public void AddFindBytesRate(int val) { findBytesRate += val; }
    // 바이트 최소 발견 주기
    public void AddFindByteMinPeriod(float val) { findByteMinPeriod += val; }
    // 바이트 최대 발견 주기
    public void AddFindByteMaxPeriod(float val) { findByteMaxPeriod += val; }
    // 바이트 최소 발견량 증가
    public void AddIncreaseFindByteMinValue(int val) { increaseFindBytMinValue += val; }

    // ==============================================
    //                Getter && Setter
    // ==============================================

    // 게임 오버 가져오기
    public bool GetGameOver() { return isGameOver; }

    // stage Setter & Getter
    public void SetStage(Planet toStage)
    {
        stage = toStage;
    }

    public Planet GetStage() { return stage; }
    public int GetMaxByteValue() { return maxByteValue; }
    public int GetFindBytesRate() { return findBytesRate; }
    public float GetFindByteMinPeriod() { return findByteMinPeriod; }
    public float GetFindByteMaxPeriod() { return findByteMaxPeriod; }
    public int GetIncreaseFindByteMinValue() { return increaseFindBytMinValue; }

    // 데솔로로 이동 가능 여부
    public void SetCanGoDesolo() { canGoDesolo = true; }
    public bool GetCanGoDesolo() { return canGoDesolo; }
    // 글라시오로 이동 가능 여부
    public void SetCanGoGlaclo() { canGoGlaclo = true; }
    public bool GetCanGoGlaclo() { return canGoGlaclo; }
    // 아트록스로 이동 가능 여부
    public void SetCanGoAtrox() { canGoAtrox = true; }
    public bool GetCanGoAtrox() { return canGoAtrox; }

    // 스테이지 이름을 반환
    public string GetCurStageName() { return stageName; }
}
