using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip musicaNormal;
    public AudioClip musicaBoss;
    private AudioSource audioSource;

    [Header("Volumen inicial (0-1)")]
    [Range(0f, 1f)] public float volumenMusica = 0.5f;
    [Range(0f, 1f)] public float volumenSFX = 0.7f;

    [Header("Sliders (opcional)")]
    public Slider sliderMusica;
    public Slider sliderSFX;

    private static float _volumenSFX;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        _volumenSFX = volumenSFX;
    }

    void Start()
    {
        audioSource.volume = volumenMusica;

        if (sliderMusica != null)
        {
            sliderMusica.value = volumenMusica;
            sliderMusica.onValueChanged.AddListener(v => audioSource.volume = v);
        }

        if (sliderSFX != null)
        {
            sliderSFX.value = volumenSFX;
            sliderSFX.onValueChanged.AddListener(v => _volumenSFX = v);
        }

        if (musicaNormal != null)
        {
            audioSource.clip = musicaNormal;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public static void PlaySFX(AudioClip clip, Vector3 posicion)
    {
        if (clip == null) return;
        AudioSource.PlayClipAtPoint(clip, posicion, _volumenSFX);
    }

    public void CambiarAMusicaBoss()
    {
        if (audioSource.clip != musicaBoss && musicaBoss != null)
        {
            audioSource.clip = musicaBoss;
            audioSource.Play();
        }
    }

    public void CambiarAMusicaNormal()
    {
        if (audioSource.clip != musicaNormal && musicaNormal != null)
        {
            audioSource.clip = musicaNormal;
            audioSource.Play();
        }
    }
}

