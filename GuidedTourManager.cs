using UnityEngine;

[System.Serializable]
public struct TourStop
{
    [Tooltip("Ponto para onde o jogador será teletransportado.")]
    public Transform Waypoint;
    
    [Tooltip("Áudio de narração deste ponto.")]
    public AudioClip Narration;
    
    [Tooltip("Painel 3D de informações deste ponto.")]
    public GameObject InfoPanel;
}

[RequireComponent(typeof(AudioSource))]
public class GuidedTourManager : MonoBehaviour
{
    [Header("Configurações do Tour")]
    [SerializeField, Tooltip("O objeto raiz do jogador (ex: XROrigin ou Camera).")]
    private Transform _playerTransform;
    
    [SerializeField, Tooltip("Lista de todas as paradas do tour na ordem em que devem ocorrer.")]
    private TourStop[] _tourStops;

    private AudioSource _audioSource;
    private int _currentStopIndex = 0;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (_tourStops == null || _tourStops.Length == 0)
        {
            Debug.LogWarning("Nenhuma parada configurada no GuidedTourManager!");
            return;
        }

        // Desativa todos os painéis inicialmente para garantir o estado limpo
        foreach (var stop in _tourStops)
        {
            if (stop.InfoPanel != null)
                stop.InfoPanel.SetActive(false);
        }

        // Inicia o tour no primeiro ponto
        ActivateStop(_currentStopIndex);
    }

    public void GoToNextStop()
    {
        // Desativa o painel atual e para o áudio
        if (_tourStops[_currentStopIndex].InfoPanel != null)
        {
            _tourStops[_currentStopIndex].InfoPanel.SetActive(false);
        }
        _audioSource.Stop();

        _currentStopIndex++;

        // Verifica se chegamos ao fim do tour
        if (_currentStopIndex >= _tourStops.Length)
        {
            Debug.Log("Experiência de 5 minutos concluída! Fim do tour.");
            // Impede que o índice saia dos limites se chamado novamente
            _currentStopIndex = _tourStops.Length - 1; 
            return;
        }

        // Ativa a próxima parada
        ActivateStop(_currentStopIndex);
    }

    private void ActivateStop(int index)
    {
        TourStop currentStop = _tourStops[index];

        // 1. Move o jogador
        if (currentStop.Waypoint != null && _playerTransform != null)
        {
            _playerTransform.position = currentStop.Waypoint.position;
            _playerTransform.rotation = currentStop.Waypoint.rotation;
        }

        // 2. Ativa o painel 3D
        if (currentStop.InfoPanel != null)
        {
            currentStop.InfoPanel.SetActive(true);
        }

        // 3. Toca a narração
        if (currentStop.Narration != null)
        {
            _audioSource.clip = currentStop.Narration;
            _audioSource.Play();
        }
    }
}
