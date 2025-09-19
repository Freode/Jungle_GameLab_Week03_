using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class TechNodeViewer : MonoBehaviour
{
    // TechNodeUI 프리팹
    public GameObject techNodeUIPrefab;

    // 생성된 노드 UI들이 위치할 부모 Transform
    public Transform techNodesParent;

    // 연결선 라인 표시기
    public GameObject ArrowBodyPrefab;

    // 연결선 방향 표시기
    public GameObject ArrowHeadPrefab;

    // Byte 표시
    public TextMeshProUGUI TextByte;

    // 테크트리 닫기 화면
    public Button ExitButton;

    // 다른 UI 상호작용 방지 블럭 생성
    public Image ImageBlock;

    // 프로젝트에 있는 모든 TechNodeEach 에셋들을 담을 리스트
    public List<TechNodeEach> allTechNodes;

    // 테크트리 이름별로 가지는 UI 노드들 저장
    private Dictionary<string, TechNodeUI> techNodesUI = new Dictionary<string, TechNodeUI>();


    void Start()
    {
        GenerateTree();
        TechNodeManager.instance.OnTechUnlocked += OnTechUnlocked;
        GameManager.instance.OnByteTextValueChanged += SetByte;
        ExitButton.onClick.AddListener(OnExitButtonClicked);
    }

    void OnDestroy()
    {
        TechNodeManager.instance.OnTechUnlocked -= OnTechUnlocked;
        GameManager.instance.OnByteTextValueChanged -= SetByte;
    }

    // 1. 모든 노드 UI 생성
    void GenerateTree()
    {
        // 모든 노드 UI 인스턴스화
        foreach (var techNodeData in allTechNodes)
        {
            GameObject nodeGameObject = Instantiate(techNodeUIPrefab, techNodesParent);

            // 위치 지정
            nodeGameObject.GetComponent<RectTransform>().anchoredPosition = techNodeData.positionUI;

            TechNodeUI nodeUI = nodeGameObject.GetComponent<TechNodeUI>();
            nodeUI.SetData(techNodeData);

            techNodesUI.Add(techNodeData.name, nodeUI);
        }

        // 2. 모든 연결선 그리기
        foreach (var nodeData in allTechNodes)
        {
            foreach (var prerequisite in nodeData.preRequisites)
            {
                DrawConnectionLine(prerequisite, nodeData);
            }
        }
    }

    // 두 노드 사이에 선 그리기
    void DrawConnectionLine(TechNodeEach from, TechNodeEach to)
    {
        TechNodeUI fromNodeUI = techNodesUI[from.name];
        TechNodeUI toNodeUI = techNodesUI[to.name];

        RectTransform fromRect = fromNodeUI.GetComponent<RectTransform>();
        RectTransform toRect = toNodeUI.GetComponent<RectTransform>();

        // 시작 지점과 끝 지점 구하기
        Vector3 diff = fromNodeUI.transform.localPosition - toNodeUI.transform.localPosition;
        Vector3 startLoc = fromNodeUI.transform.localPosition + new Vector3(fromRect.rect.width / 2 + 5f, 0f, 0f);
        Vector3 endLoc = toNodeUI.transform.localPosition - new Vector3(toRect.rect.width / 2 + 5f, 0f, 0f);

        float width = Mathf.Abs(endLoc.x - startLoc.x);
        // 직선 화살표
        if (diff.y == 0)
        {
            // 직선 화살표 몸통 설정
            GameObject line = Instantiate(ArrowBodyPrefab, techNodesParent);
            RectTransform lineRect = line.GetComponent<RectTransform>();

            lineRect.sizeDelta = new Vector2(width, lineRect.sizeDelta.y);
            lineRect.localPosition = (startLoc + endLoc) / 2;

            // 직선 화살표 머리 설정
            GameObject head = Instantiate(ArrowHeadPrefab, techNodesParent);
            RectTransform headRect = head.GetComponent<RectTransform>();

            headRect.localPosition = endLoc - new Vector3(5f, 0f, 0f);
        }
        // 꺾인 화살표
        else
        {
            // 중앙 화살표 설정
            GameObject midLine = Instantiate(ArrowBodyPrefab, techNodesParent);
            RectTransform midLineRect = midLine.GetComponent<RectTransform>();

            float bendPointX = (to.lineModify == 0) ? endLoc.x - 50f : endLoc.x - to.lineModify;
            float height = Mathf.Abs(endLoc.y - startLoc.y);

            midLineRect.sizeDelta = new Vector2(midLineRect.sizeDelta.x, height);
            midLineRect.localPosition = new Vector3(bendPointX, (startLoc.y + endLoc.y) / 2f, 0f);

            // 처음 화살표 설정
            GameObject firstLine = Instantiate(ArrowBodyPrefab, techNodesParent);
            RectTransform firstLineRect = firstLine.GetComponent<RectTransform>();

            float firstX = (startLoc.x + bendPointX) / 2f;
            float firstWidth = Mathf.Abs(bendPointX - startLoc.x);
            firstLineRect.sizeDelta = new Vector2(firstWidth, firstLineRect.sizeDelta.y);
            firstLineRect.localPosition = new Vector3(firstX, startLoc.y, 0f);

            // 마지막 화살표 설정
            GameObject lastLine = Instantiate(ArrowBodyPrefab, techNodesParent);
            RectTransform lastLineRect = lastLine.GetComponent<RectTransform>();

            float lastX = (endLoc.x + bendPointX) / 2f;
            float lastWidth = Mathf.Abs(endLoc.x - bendPointX);
            lastLineRect.sizeDelta = new Vector2(lastWidth, lastLineRect.sizeDelta.y);
            lastLineRect.localPosition = new Vector3(lastX, endLoc.y, 0f);

            // 화살표 머리 설정
            GameObject head = Instantiate(ArrowHeadPrefab, techNodesParent);
            RectTransform headRect = head.GetComponent<RectTransform>();

            headRect.localPosition = endLoc - new Vector3(5f, 0f, 0f);
        }
    }

    // 기술이 해금될 때 호출되는 함수
    private void OnTechUnlocked(TechNodeEach unlockedTech)
    {
        // 해금된 노드와 이제 해금 가능해진 노드들의 시각적 상태 업데이트
        foreach (var techUINode in unlockedTech.nextSkills)
        {
            techNodesUI[techUINode.name].UpdateVisuals();
        }
    }

    public void SetByte(int curValue, int maxValue)
    {
        TextByte.text = "바이트 : " + curValue.ToString() + " / " + maxValue.ToString();
    }

    // 연구창 닫기
    public void OnExitButtonClicked()
    {
        gameObject.SetActive(false);
        ImageBlock.gameObject.SetActive(false);
    }

    // 연구창 열기
    public void OnOpenButtonClikcked()
    {
        gameObject.SetActive(true);
        ImageBlock.gameObject.SetActive(true);
    }
}
