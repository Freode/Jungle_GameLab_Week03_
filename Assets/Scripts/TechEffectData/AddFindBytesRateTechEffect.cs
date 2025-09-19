using UnityEngine;

// ����Ʈ �߰� ��, �߰߷��� �ִ� n% ����
[CreateAssetMenu(fileName = "NewFindBytesRate", menuName = "Tech Tree/Effects/Find Bytes Rate")]
public class AddFindBytesRateTechEffect : BaseTechEffect
{
    public int increaseRate = 0;

    public override void ApplyTechEffect()
    {
        GameManager.instance.AddFindBytesRate(increaseRate);
    }
}
