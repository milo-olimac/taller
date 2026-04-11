using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Referencia al Timer original")]
    public Timer timer; // Arrastra aqui el objeto que tiene Timer.cs

    [Header("Duracion del juego")]
    public float gameDuration = 60f;
    public int targetScore = 100;
    public int minScoreLimit = -30;

    [Header("UI - HUD")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI tiempoRestanteText; // Muestra el tiempo que queda
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI feedbackText;

    [Header("UI - Panel Final")]
    public GameObject panelFinal;
    public TextMeshProUGUI resultTitleText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI luciernagsAtrapText;
    public TextMeshProUGUI insectosAtrapText;
    public TextMeshProUGUI lucierEscapadasText;
    public TextMeshProUGUI tiempoFinalText;

    // Estado del juego
    private int score = 0;
    private bool gameActive = false;
    private int lives = 3;

    // Estadisticas
    private int luciernagsAtrapadas = 0;
    private int insectosAtrapados = 0;
    private int luciernagsEscapadas = 0;

    // Tiempo interno
    private float gameStartTime = 0f;
    private float tiempoTranscurrido = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (panelFinal != null) panelFinal.SetActive(false);
        if (feedbackText != null) feedbackText.gameObject.SetActive(false);
        StartGame();
    }

    public void StartGame()
    {
        score = 0;
        lives = 3;
        luciernagsAtrapadas = 0;
        insectosAtrapados = 0;
        luciernagsEscapadas = 0;
        tiempoTranscurrido = 0f;
        gameActive = true;
        gameStartTime = Time.time;

        if (panelFinal != null) panelFinal.SetActive(false);

        UpdateScoreUI();
        UpdateLivesUI();

        // Iniciar el Timer original
        if (timer != null)
        {
            timer.TimerReset();
            timer.TimerStart();
        }

        // Reiniciar el spawner
        Spawner spawner = FindObjectOfType<Spawner>();
        if (spawner != null) spawner.ResetSpawner();
    }

    void Update()
    {
        if (!gameActive) return;

        // Calcular cuanto tiempo ha pasado desde que inicio el juego
        tiempoTranscurrido = Time.time - gameStartTime;
        float tiempoRestante = Mathf.Max(0f, gameDuration - tiempoTranscurrido);

        // Mostrar tiempo restante en el HUD
        if (tiempoRestanteText != null)
            tiempoRestanteText.text = "Tiempo: " + Mathf.CeilToInt(tiempoRestante).ToString();

        // Se acabo el tiempo
        if (tiempoRestante <= 0f)
            EndGame(score >= targetScore);
    }

    public void AddPoints(int points)
    {
        if (!gameActive) return;
        score += points;
        UpdateScoreUI();

        if (score >= targetScore) { EndGame(true);  return; }
        if (score <= minScoreLimit)  EndGame(false);
    }

    public void LoseLife()
    {
        if (!gameActive) return;
        lives--;
        UpdateLivesUI();
        if (lives <= 0) EndGame(false);
    }

    public void RegisterLuciernagaAtrapada() => luciernagsAtrapadas++;
    public void RegisterInsectoAtrapado()    => insectosAtrapados++;
    public void RegisterLuciernagaEscapada() => luciernagsEscapadas++;
    public bool IsGameActive()               => gameActive;

    void EndGame(bool victoria)
    {
        gameActive = false;

        // Detener el Timer original
        if (timer != null)
            timer.TimerStop();

        // Destruir objetos en pantalla
        foreach (var obj in GameObject.FindGameObjectsWithTag("Luciernaga"))
            Destroy(obj);
        foreach (var obj in GameObject.FindGameObjectsWithTag("Insecto"))
            Destroy(obj);

        float tiempoRestante = Mathf.Max(0f, gameDuration - tiempoTranscurrido);
        ShowFinalPanel(victoria, tiempoRestante);
        SaveResultsJSON(victoria, tiempoRestante);
    }

    void ShowFinalPanel(bool victoria, float tiempoRestante)
    {
        if (panelFinal == null) return;
        panelFinal.SetActive(true);

        if (resultTitleText     != null) resultTitleText.text     = victoria ? "¡GANASTE!" : "¡PERDISTE!";
        if (finalScoreText      != null) finalScoreText.text      = score.ToString();
        if (luciernagsAtrapText != null) luciernagsAtrapText.text = luciernagsAtrapadas.ToString();
        if (insectosAtrapText   != null) insectosAtrapText.text   = insectosAtrapados.ToString();
        if (lucierEscapadasText != null) lucierEscapadasText.text = luciernagsEscapadas.ToString();
        if (tiempoFinalText     != null) tiempoFinalText.text     = Mathf.FloorToInt(tiempoRestante).ToString() + " seg";
    }

    public void OnReiniciarClicked()
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag("Luciernaga"))
            Destroy(obj);
        foreach (var obj in GameObject.FindGameObjectsWithTag("Insecto"))
            Destroy(obj);

        StartGame();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Puntos: " + score;
    }

    void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "Vidas: " + lives;
    }

    public void ShowFeedback(string msg, float duration = 0.8f)
    {
        if (feedbackText == null) return;
        StopAllCoroutines();
        StartCoroutine(FeedbackCoroutine(msg, duration));
    }

    IEnumerator FeedbackCoroutine(string msg, float duration)
    {
        feedbackText.text = msg;
        feedbackText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        feedbackText.gameObject.SetActive(false);
    }

    void SaveResultsJSON(bool victoria, float tiempoRestante)
    {
        GameResult result = new GameResult
        {
            victoria                       = victoria,
            puntajeTotal                   = score,
            luciernagsAtrapadas            = luciernagsAtrapadas,
            insectosNegativosSeleccionados = insectosAtrapados,
            luciernagsEscapadas            = luciernagsEscapadas,
            tiempoFinal                    = Mathf.RoundToInt(gameDuration - tiempoRestante)
        };

        string json = JsonUtility.ToJson(result, true);

        if (!Directory.Exists(Application.streamingAssetsPath))
            Directory.CreateDirectory(Application.streamingAssetsPath);

        string path = Application.streamingAssetsPath + "/resultado_partida.json";
        File.WriteAllText(path, json);
        Debug.Log("JSON guardado en: " + path);
    }

    [System.Serializable]
    public class GameResult
    {
        public bool victoria;
        public int  puntajeTotal;
        public int  luciernagsAtrapadas;
        public int  insectosNegativosSeleccionados;
        public int  luciernagsEscapadas;
        public int  tiempoFinal;
    }
}
