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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Sylva.onClick.AddListener(OnSylvaClicked);
        Desolo.onClick.AddListener(OnDesoloClicked);
        Glacio.onClick.AddListener(OnGlacioClicked);
        Atrox.onClick.AddListener(OnAtroxClicked);
        Exit.onClick.AddListener(OnExitTravelUI);
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
        gameObject.SetActive(false);
    }

    // 우주 여행 버튼 누름
    public void OnTravelEvent(Planet planet)
    {
        GameManager.instance.SetCurrentPlanet(planet);
        ChangeCurStageText();
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
