using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoreNodeUI : MonoBehaviour
{
    public Planet planet;
    public Button unlockButton;
    public TextMeshProUGUI textMesh;
    public TextMeshProUGUI textByte;
    public BaseTechEffect purchaseEffect;


    private Outline buttonOutline;
    private bool isPurchase = false;

    public void Awake()
    {
        buttonOutline = gameObject.GetComponent<Outline>();
    }

    private void Start()
    {
        unlockButton.onClick.AddListener(OnCoreClicked);
    }

    // 코어 구매 요청
    private void OnCoreClicked()
    {
        // 이미 구매했다면, 무시
        if (isPurchase)
            return;

        // 구매 조건 만족 여부 검사
        int curByte = GameManager.instance.GetCurByteValue();
        int purchaseByte = int.Parse(textByte.text);

        if (curByte < purchaseByte)
            return;

        // 구매 진행
        isPurchase = true;
        unlockButton.interactable = false;
        buttonOutline.effectColor = Color.green;

        // 바이트 차감
        GameManager.instance.AddCurByteValue(-1 * purchaseByte);

        // 코어 구매 효과 적용
        purchaseEffect.ApplyTechEffect();
    }

    private void OnEnable()
    {
        // 이미 구매했다면, 무시
        if (isPurchase)
            return;

        // 현재 행성과 일치한다면, 구매 가능
        Planet curPlanet = GameManager.instance.GetStage();
        if (curPlanet == planet)
        {
            int curByte = GameManager.instance.GetCurByteValue();
            int purchaseByte = int.Parse(textByte.text);
            // 구매 가능 여부에 따라 다른 색상으로 출력
            if (curByte >= purchaseByte)
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
        // 현재 행성과 불일치면, 구매 불가능
        else
        {
            buttonOutline.effectColor = Color.gray;
            unlockButton.interactable = false;
        }

    }
}
