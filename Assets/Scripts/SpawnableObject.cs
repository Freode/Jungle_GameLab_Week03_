﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnType
{ 
    Resource = 0,
    Structure,
    Danger
}


public class SpawnableObject : MonoBehaviour
{
    public SpawnType spawnType;                 // 현재 스폰될 객체의 타입
    public List<int> interactByteValue;         // 상호작용 시, 획득할 바이트의 양

    public Vector3 baseScale;                   // 기준이 되는 스케일 크기

    public float growthDuration = 0.5f;         // 생성되는데 걸리는 시간
    private bool canInteract = false;           // 생성 완료 후, 상호작용 가능으로 변경

    private void Awake()
    {
        
        transform.localScale = Vector3.zero;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnAnimate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 스폰된 객체 성장하는 애니메이션
    IEnumerator SpawnAnimate()
    {
        float elapsedTime = 0f;
        Vector3 targetScale = Vector3.one;
        // 부모 스케일로 인해서 스케일 크기가 변하는 것을 조정
        if (transform.parent != null)
        {
            Vector3 parentScale = transform.parent.lossyScale;
            targetScale = new Vector3(
                baseScale.x / Mathf.Max(parentScale.x, float.Epsilon),
                baseScale.y / Mathf.Max(parentScale.y, float.Epsilon),
                baseScale.z / Mathf.Max(parentScale.z, float.Epsilon)
            );
        }

        // 스케일 키우기
        while (elapsedTime < growthDuration)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsedTime / growthDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        canInteract = true;

        // 위험 요소가 아닐때는 무시
        if (spawnType == SpawnType.Danger)
            StartCoroutine(AfterSpawn());
    }

    // 마우스 클릭 함수
    private void OnMouseDown()
    {
        if (canInteract == false)
            return;

        int stageNum = (int)GameManager.instance.GetStage();
        switch (spawnType)
        {
            // 자원 - 바이트 획득
            case SpawnType.Resource:
                // 공식
                // 최소 바이트 = (행성 바이트 + 바이트 추가 획득) * 바이트 추가 획득 / 2
                // 최대 바이트 = (행성 바이트 + 바이트 추가 획득) * 바이트 추가 획득
                int percent = GameManager.instance.GetFindBytesRate();
                int minValue = interactByteValue[stageNum] + GameManager.instance.GetIncreaseFindByteMinValue();
                int maxValue = minValue + (minValue * percent / 100);
                minValue = (minValue + maxValue) / 2;
                int addValue = Random.Range(minValue, maxValue + 1);
                GameManager.instance.AddCurByteValue(addValue);
                break;

            // 구조물 - 정보 확인?
            case SpawnType.Structure:
                return;

            // 위험 - 바이트 소멸
            case SpawnType.Danger:
                GameManager.instance.AddCurByteValue(-1 * interactByteValue[stageNum]);
                break;
        }
  
        Destroy(gameObject);
    }

    IEnumerator AfterSpawn()
    {
        float curWaitTime = 0f;
        float limitWaitTime = 10f;

        // 위험 요소는 일정 시간이 지난 후에 자동으로 제거
        while(curWaitTime < limitWaitTime)
        {
            curWaitTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
