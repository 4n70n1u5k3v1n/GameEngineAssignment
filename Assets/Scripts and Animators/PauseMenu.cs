using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused;

    private AudioSource[] allAudioSources;
    private Animator[] allAnimators;
    private CharacterController[] allCharacterControllers;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        // Find all AudioSource, Animator, and CharacterController components in the scene
        allAudioSources = FindObjectsOfType<AudioSource>();
        allAnimators = FindObjectsOfType<Animator>();
        allCharacterControllers = FindObjectsOfType<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Debug.Log("Game Paused");
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

        // Show and unlock the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Pause all audio sources
        foreach (AudioSource audio in allAudioSources)
        {
            if (audio.isPlaying)
            {
                audio.Pause();
            }
        }

        // Pause all animators
        foreach (Animator animator in allAnimators)
        {
            animator.speed = 0f;
        }

        // Disable all character controllers
        foreach (CharacterController controller in allCharacterControllers)
        {
            controller.enabled = false;
        }

        isPaused = true;
    }

    public void ResumeGame()
    {
        Debug.Log("Game Resumed");
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Resume all audio sources
        foreach (AudioSource audio in allAudioSources)
        {
            if (audio != null) // Check to avoid null reference errors
            {
                audio.UnPause();
            }
        }

        // Resume all animators
        foreach (Animator animator in allAnimators)
        {
            animator.speed = 1f;
        }

        // Enable all character controllers
        foreach (CharacterController controller in allCharacterControllers)
        {
            controller.enabled = true;
        }

        isPaused = false;
    }


public void QuitGame()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
