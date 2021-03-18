using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using GoogleMobileAds.Api;

public class GameControls : MonoBehaviour
{
    public GameObject canvasPause;
    public Text resultText;
    public GameObject canvasWait;
    public Text waitText;

    public Text player1Points;
    public Text player2Points;

    private int player1PointsNumber = 0;
    private int player2PointsNumber = 0;

    private bool isPause = false;
    public AIControls ai;
    public PlayerControls player;


    public GameObject colorsPanel;

    private Color[] buttonColors;
    private string[] buttonNames;

    public PlayerControls playerControls;
    public AIControls aiControls;

    public GameObject canvasChooseColor;

    private InterstitialAd interstitial;

    private void RequestInterstitial()
    {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-5553264284629233/6767228293";
        #else
        string adUnitId = "unexpected_platform";
        #endif

        this.interstitial = new InterstitialAd(adUnitId);

        this.interstitial.OnAdClosed += HandleOnAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);
    }

    private void HandleOnAdClosed(object sender, System.EventArgs e)
    {
        interstitial.Destroy();
    }

    public void SetPlayerColor(GameObject button)
    {
        Debug.Log(button.name);
        for (int i = 0; i < buttonNames.Length; i++)
        {
            if (button.name == buttonNames[i]) 
            {
                player.GetComponent<SpriteRenderer>().color = buttonColors[i];
            }
        }

        canvasChooseColor.SetActive(false);
        StartCoroutine(WaitBeforeStart());
    }

    public void SetPlayersPoints()
    {
        player2Points.text = GetPlayer2Points();
        player1Points.text = GetPlayer1Points();
    }

    public void AddPlayer1Point()
    {
        player1PointsNumber++;
    }

    public void AddPlayer2Point()
    {
        player2PointsNumber++;
    }

    public string GetPlayer1Points()
    {
        return player1PointsNumber.ToString();
    }

    public string GetPlayer2Points()
    {
        return player2PointsNumber.ToString();
    }

    public bool CheckIsWinner()
    {
        if (player1PointsNumber == 5)
        {
            resultText.text = "Победа!";
            playerControls.SetPlayMode(false);
            if (this.interstitial.IsLoaded())
            {
                this.interstitial.Show();
            }
            canvasPause.SetActive(true);
            return true;
        }
        if (player2PointsNumber == 5)
        {
            resultText.text = "Поражение!";
            aiControls.SetPlayMode(false);
            if (this.interstitial.IsLoaded())
            {
                this.interstitial.Show();
            }
            canvasPause.SetActive(true);
            return true;
        }
        return false;
    }

    public IEnumerator WaitBeforeStart()
    {
        ai.SetPlayMode(false);
        player.SetPlayMode(false);
        canvasWait.SetActive(true);
        waitText.text = "3";
        yield return new WaitForSeconds(1f);
        waitText.text = "2";
        yield return new WaitForSeconds(1f);
        waitText.text = "1";
        yield return new WaitForSeconds(1f);
        canvasWait.SetActive(false);
        ai.SetPlayMode(true);
        player.SetPlayMode(true);
    }

    private void Awake()
    {
        buttonColors = new Color[9];
        buttonNames = new string[9];
        int k = 0;
        for (int i = 0; i < colorsPanel.transform.childCount; i++)
        {
            for (int j = 0; j < colorsPanel.transform.GetChild(i).childCount; j++)
            {
                buttonNames[k] = colorsPanel.transform.GetChild(i).transform.GetChild(j).name;
                buttonColors[k] = colorsPanel.transform.GetChild(i).transform.GetChild(j).gameObject.GetComponent<Image>().color;
                k++;
            }
        }
        k = Random.Range(0, 8);
        buttonColors[8] = buttonColors[k];
    }

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        this.RequestInterstitial();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            canvasPause.SetActive(isPause);
        }
    }

    public void LoadLobby()
    {
        SceneManager.LoadScene("Menu");
    }
}
