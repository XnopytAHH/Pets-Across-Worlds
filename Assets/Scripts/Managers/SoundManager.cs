/*
* Author: Lim En Xu Jayson
* Date: 9 November 2025
* Description: Manages game audio including background music and sound effects.
*/
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the SoundManager.
    /// </summary>
    public static SoundManager instance;
    /// <summary>
    /// The AudioSource used to play sounds.
    /// </summary>
    AudioSource audioSource;
    /// <summary>
    /// Background music clip.
    /// </summary>
    [SerializeField]
    public AudioClip backgroundMusic;
    /// <summary>
    /// Button click sound effect.
    /// </summary>
    [SerializeField]
    public AudioClip buttonClickSound;
    /// <summary>
    /// Pet eating sound effect.
    /// </summary>
    [SerializeField]
    public AudioClip petEatingSound;
    /// <summary>
    /// Ball bounce sound effect.
    /// </summary>
    [SerializeField]
    public AudioClip ballBounceSound;
    /// <summary>
    /// Lose game sound effect.
    /// </summary>
    [SerializeField]
    public AudioClip loseGameSound;
    /// <summary>
    /// Initializes the SoundManager singleton and the AudioSource.
    /// </summary>
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
    /// <summary>
    /// Starts background music playback.
    /// </summary>
    void Start()
    {
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();
        audioSource.volume = 0.2f;
    }
    /// <summary>
    /// Plays the button click sound.
    /// </summary>
    public void buttonClick()
    {
        audioSource.PlayOneShot(buttonClickSound, 1f);
    }
    /// <summary>
    /// Plays the pet eating sound.
    /// </summary>
    public void petEating()
    {
        audioSource.PlayOneShot(petEatingSound, 0.7f);
    }
    /// <summary>
    /// Plays the ball bounce sound.
    /// </summary>
    public void ballBounce()
    {
        audioSource.PlayOneShot(ballBounceSound, 0.7f);
    }
    /// <summary>
    /// Plays the lose game sound.
    /// </summary>
    public void loseGame()
    {
        audioSource.PlayOneShot(loseGameSound, 1f);
    }

}
