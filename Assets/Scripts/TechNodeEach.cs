using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewTechNode", menuName ="Tech Tree/Tech Node")]
public class TechNodeEach : ScriptableObject
{
    public string techName;                     // 기술 이름
    public string techNamePrint;                // 출력용 기술 이름
    public string description;                  // 기술 설명
    public int requiredByteValue;               // 해금을 위한 필요 바이트 수
    public List<TechNodeEach> preRequisites;    // 선행 기술 목록
    public List<TechNodeEach> nextSkills;       // 다음 기술 목록
    public int lineModify;                      // 꺽인 선 위치 조절
    public Vector2 positionUI;                  // UI에서 위치
    public List<BaseTechEffect> effects;        // 해금 시, 적용할 효과
}
