using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

// 건물 활성화 코드
[CreateAssetMenu(fileName = "NewActiveStructures", menuName = "Tech Tree/Effects/Active Structure")]
public class AddActiveStructures : BaseTechEffect
{
    public GameObject structuresPrefab;

    public override void ApplyTechEffect()
    {
        GameManager.instance.ActiveStructures(structuresPrefab);
    }
}
