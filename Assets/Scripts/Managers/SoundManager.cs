using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    AudioSource audioSource;
    [SerializeField]
    public AudioClip backgroundMusic;
    [SerializeField]
    public AudioClip buttonClickSound;
    [SerializeField]
    public AudioClip petEatingSound;
    [SerializeField]
    public AudioClip ballBounceSound;
    [SerializeField]
    public AudioClip loseGameSound;
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
        }
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();
        audioSource.volume = 0.2f;
    }
    public void buttonClick()
    {
        audioSource.PlayOneShot(buttonClickSound, 1f);
    }
    public void petEating()
    {
        audioSource.PlayOneShot(petEatingSound, 0.7f);
    }
    public void ballBounce()
    {
        audioSource.PlayOneShot(ballBounceSound, 0.7f);
    }
    public void loseGame()
    {
        audioSource.PlayOneShot(loseGameSound, 1f);
    }

}
