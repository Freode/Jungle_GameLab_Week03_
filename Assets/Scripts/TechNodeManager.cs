using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TechNodeManager : MonoBehaviour
{
    public static TechNodeManager instance;

    // key : techName, Value : TechNodeEach data
    public Dictionary<string, TechNodeEach> unlockedNodes = new Dictionary<string, TechNodeEach>();

    // 테크트리 해제 이벤트 호출
    public event System.Action<TechNodeEach> OnTechUnlocked;

    private void Awake()
    {
        instance = this;
    }

    // 특정 기술 해금 가능한지 검사
    public bool CanUnlockTech(TechNodeEach tech)
    {
        // 이미 해금 시, 무시
        if (unlockedNodes.ContainsKey(tech.name))
            return true;

        // 선행 기술이 없는 경우
        if (tech.preRequisites.Count == 0)
            return true;

        // 모든 선행 기술이 해금되었는지 확인
        foreach(var preTech in tech.preRequisites)
        {
            if (unlockedNodes.ContainsKey(preTech.name) == true)
                return true;
        }

        return false;
    }

    // 기술 해금
    public void UnlockTech(TechNodeEach tech)
    {
        if (CanUnlockTech(tech) == false)
            return;

        // TODO : 재화 차감 로직

        // 해금 목록에 추가
        unlockedNodes.Add(tech.name, tech);

        // 태크트리 UI 갱신
        OnTechUnlocked?.Invoke(tech);
    }

}
