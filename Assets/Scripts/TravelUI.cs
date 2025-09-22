using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TravelUI : MonoBehaviour
{
    public TextMeshProUGUI TextCurStage;
    public Button Exit;
    public Button Sylva;
    public Button Desolo;
    public Button Glacio;
    public Button Atrox;
    public Image imageBlock;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Sylva.onClick.AddListener(OnSylvaClicked);
        Desolo.onClick.AddListener(OnDesoloClicked);
        Glacio.onClick.AddListener(OnGlacioClicked);
        Atrox.onClick.AddListener(OnAtroxClicked);
        Exit.onClick.AddListener(OnExitTravelUI);

        GameManager.instance.OnPlanetChanged += ChangeCurStageText;
    }

    private void OnDestroy()
    {
        GameManager.instance.OnPlanetChanged -= ChangeCurStageText;
    }

    // 우주 여행 UI 활성화
    public void OnActiveTravelUI()
    {
        gameObject.SetActive(true);

        // 행성 연구에 따라 갈 수 있는 행성 정해짐
        ChangeCurStageText();
        bool canGoDesolo = GameManager.instance.GetCanGoDesolo();
        Desolo.interactable = canGoDesolo;

        bool canGoGlacio = GameManager.instance.GetCanGoGlaclo();
        Glacio.interactable = canGoGlacio;

        bool canGoAtrox = GameManager.instance.GetCanGoAtrox();
        Atrox.interactable= canGoAtrox;
    }

    // 현재 위치를 알려주는 글자 설정
    public void ChangeCurStageText()
    {
        string planetName = GameManager.instance.GetCurStageName();
        TextCurStage.text = "현재 위치 : " + planetName;
    }

    // 우주 여행 UI 닫기
    public void OnExitTravelUI()
    {
        imageBlock.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    // 우주 여행 버튼 누름
    public void OnTravelEvent(Planet planet)
    {
        // 현재 행성과 같은 행성으로 여행할 경우, 무시
        Planet curPlanet = GameManager.instance.GetStage();
        if (curPlanet == planet)
            return;

        imageBlock.gameObject.SetActive(true);
        GameManager.instance.SetCurrentPlanet(planet);
        ChangeCurStageText();
        OnExitTravelUI();
        GameManager.instance.InactiveDemonstration();
    }

    // 버튼 클릭
    public void OnSylvaClicked()
    {
        OnTravelEvent(Planet.Sylva);
    }

    public void OnDesoloClicked()
    {
        OnTravelEvent(Planet.Desolo);
    }

    public void OnGlacioClicked()
    {
        OnTravelEvent(Planet.Glacio);
    }

    public void OnAtroxClicked()
    {
        OnTravelEvent(Planet.Atrox);
    }
}
