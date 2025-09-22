using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct SpawnInfo
{
    public GameObject spawnPrefab;      // 스폰할 프리팹
    public int spawnRate;               // 스폰 가중치
}


public class ResearchSpawnPoint : MonoBehaviour
{
    public List<SpawnInfo> spawnLists;
    public TextMeshProUGUI valueText;
    public Image imageValue; 

    private int totalRate = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 모든 가중치 합 구하기
        foreach(var spawnlist in spawnLists)
        {
            totalRate += spawnlist.spawnRate;
        }

        // 30% 확률로 스폰
        int firstSpawnRate = Random.Range(0, 100);
        if (firstSpawnRate <= 30)
            SpawnResearchResource();

        InitValueText();
        StartCoroutine(WaitToRemove());
    }

    // 값 텍스트 출력 초기 설정
    void InitValueText()
    {
        Camera mainCamera = Camera.main;

        Vector3 worldPosition = transform.position;
        Vector2 screentPosition = mainCamera.WorldToScreenPoint(worldPosition);

        RectTransform rectTransform = imageValue.GetComponent<RectTransform>();
        rectTransform.position = screentPosition;
    }

    // 스폰 활성화까지 확인
    IEnumerator WaitToRemove()
    {
        while(true)
        {
            yield return null;

            int validObject = transform.childCount;

            // 자식이 text밖에 없으면, 스폰 대기에 추가
            if (validObject == 1)
                break;
        }

        StartCoroutine(WaitToSpawn());
    }

    // 스폰 대기까지 활성화
    IEnumerator WaitToSpawn()
    {
        float curMinSpawnTime = GameManager.instance.GetFindByteMinPeriod();
        float curMaxSpawnTime = GameManager.instance.GetFindByteMaxPeriod();

        float spawnTime = Random.Range(curMinSpawnTime, curMaxSpawnTime);
        
        yield return new WaitForSeconds(spawnTime);

        SpawnResearchResource();
        StartCoroutine(WaitToRemove());
    }

    // 스폰
    private void SpawnResearchResource()
    {
        // 스폰 프리팹 선택하기
        int select = Random.Range(1, totalRate + 1);
        SpawnInfo selectInfo = new SpawnInfo();

        foreach(var spawnlist in spawnLists)
        {
            select -= spawnlist.spawnRate;
            // 가중치가 0 이하면, 해당 프리팹을 선택
            if(select <= 0)
            {
                selectInfo = spawnlist;
                break;
            }
        }

        // 프리팹 스폰하기
        GameObject resource = Instantiate(selectInfo.spawnPrefab, transform);
        SpawnableObject spawnableObject = resource.GetComponent<SpawnableObject>();
        spawnableObject.OnObjectClicked += PrintValue;
        resource.transform.localPosition = Vector3.zero;
    }

    // 얻는 값을 화면에 출력
    private void PrintValue(int value)
    {
        string inputText;
        
        if(value > 0)
        {
            inputText = "+" + value.ToString();
            valueText.color = Color.green;
        }
        else
        {
            inputText = value.ToString();
            valueText.color= Color.red;
        }

        valueText.text = inputText;

        StartCoroutine(PrintValueDuration());
    }

    // 값을 출력하는 시간
    IEnumerator PrintValueDuration()
    {
        float timer = 0.5f;
        float cur = 0f;
        imageValue.gameObject.SetActive(true);

        while(cur <= timer)
        {
            cur += Time.deltaTime;
            yield return null;
        }

        imageValue.gameObject.SetActive(false);
    }
}
