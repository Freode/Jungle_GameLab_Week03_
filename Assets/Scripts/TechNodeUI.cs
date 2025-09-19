using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class TechNodeUI : MonoBehaviour
{
    public Button unlockButton;
    public TextMeshProUGUI textMesh;
    public TextMeshProUGUI textByte;
    private Outline buttonOutline;

    // 현재 UI가 가지고 있는 테크 데이터
    private TechNodeEach techData;

    private bool isUnlocked = false;
    private bool canUnlock = false;
    // TechTreeView가 UI에 데이터를 주입하는 함수

    public void Awake()
    {
        buttonOutline = gameObject.GetComponent<Outline>();
    }

    private void Update()
    {
        UpdateCanLockVisualNode();
    }

    public void SetData(TechNodeEach tech)
    {
        techData = tech;

        // 객체 이름 설정
        name = $"NodeUI_{tech.name}";
        textMesh.text = tech.techNamePrint;
        textByte.text = tech.requiredByteValue.ToString();
        unlockButton.interactable = false;

        unlockButton.onClick.AddListener(OnNodeClicked);
        UpdateVisuals();
    }

    // 버튼 클릭 시, 잠금 해제 요청
    private void OnNodeClicked()
    {
        bool checkUnlock = TechNodeManager.instance.CanUnlockTech(techData);

        // 잠금 해제 불가능하면, 무시
        if (checkUnlock == false)
            return;

        // 현재 보유한 바이트보다 요구량이 더 많으면, 무시
        int needByteValue = int.Parse(textByte.text);
        int curByteValue = GameManager.instance.GetCurByteValue();
        if (needByteValue > curByteValue)
            return;

        // 연구 해제 완료
        GameManager.instance.AddCurByteValue(-1 * needByteValue);

        isUnlocked = true;
        canUnlock = false;
        TechNodeManager.instance.UnlockTech(techData);
        UnlockVisualNode();
    }       

    // 연구 가능 상태로 변경
    public void SetCanLock()
    {
        canUnlock = true;
    }

    // 연구 가능 상태로 변경되었을 때, 바이트 양에 따라 구매 가능 여부를 색상으로 표현
    private void UpdateCanLockVisualNode()
    {
        if (canUnlock == false)
            return;

        int curByte = GameManager.instance.GetCurByteValue();

        if (curByte >= techData.requiredByteValue)
        {
            buttonOutline.effectColor = Color.yellow;
            unlockButton.interactable = true;
        }
        else
        {
            buttonOutline.effectColor = Color.red;
            unlockButton.interactable = false;
        }
    }

    // 노드의 상태에 따라 시각적 요소를 업데이트
    public void UpdateVisuals()
    {
        bool isUnlockedL = TechNodeManager.instance.unlockedNodes.ContainsKey(techData.name);
        bool canUnlockL = TechNodeManager.instance.CanUnlockTech(techData);

        if (buttonOutline == null)
            return;

        // 활성화 여부에 따라 외곽선 다른 효과
        if (isUnlocked != isUnlockedL)
        {
            isUnlocked = isUnlockedL;
            UnlockVisualNode();
            return;
        }

        // 활성화가 되지 않았을 때, 해금 가능 여부에 따라 다른 효과
        if(canUnlock != canUnlockL)
        {
            canUnlock = canUnlockL;
            buttonOutline.effectColor = Color.yellow;
            unlockButton.interactable = true;
        }
    }

    public void UnlockVisualNode()
    {
        buttonOutline.effectColor = Color.green;
        unlockButton.interactable = false;
    }
}
