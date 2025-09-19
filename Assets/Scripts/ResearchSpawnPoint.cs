using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public struct SpawnInfo
{
    public GameObject spawnPrefab;      // 스폰할 프리팹
    public int spawnRate;               // 스폰 가중치
}


public class ResearchSpawnPoint : MonoBehaviour
{
    public List<SpawnInfo> spawnLists;

    private int totalRate = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 모든 가중치 합 구하기
        foreach(var spawnlist in spawnLists)
        {
            totalRate += spawnlist.spawnRate;
        }

        StartCoroutine(WaitToRemove());
    }

    // 스폰 활성화까지 확인
    IEnumerator WaitToRemove()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);

            int validObject = transform.childCount;

            // 자식이 없으면, 스폰 대기에 추가
            if (validObject == 0)
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
        resource.transform.localPosition = Vector3.zero;
    }
}
