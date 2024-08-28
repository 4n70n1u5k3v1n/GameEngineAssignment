using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsMenu; // Reference to the settings menu
    public bool isPaused;

    private AudioSource[] allAudioSources;
    private Animator[] allAnimators;
    private CharacterController[] allCharacterControllers;
    private MouseLook[] allMouseLookScripts;

    void Start()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false); // Ensure settings menu is not active initially
        allAnimators = FindObjectsOfType<Animator>();
        allCharacterControllers = FindObjectsOfType<CharacterController>();
        allMouseLookScripts = FindObjectsOfType<MouseLook>();
    }

    void Update()
    {
        // Check if Escape is pressed and the settings menu is not active
        if (Input.GetKeyDown(KeyCode.Escape) && !settingsMenu.activeSelf)
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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Dynamically find all audio sources every time the game is paused
        allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in allAudioSources)
        {
            if (audio.isPlaying)
            {
                audio.Pause();
            }
        }

        foreach (Animator animator in allAnimators)
        {
            animator.speed = 0f;
        }

        foreach (CharacterController controller in allCharacterControllers)
        {
            controller.enabled = false;
        }

        foreach (MouseLook mouseLook in allMouseLookScripts)
        {
            mouseLook.enabled = false;
        }

        isPaused = true;
    }

    public void ResumeGame()
    {
        Debug.Log("Game Resumed");
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false); // Make sure to close the settings menu
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        foreach (AudioSource audio in allAudioSources)
        {
            if (audio != null)
            {
                audio.UnPause();
            }
        }

        foreach (Animator animator in allAnimators)
        {
            animator.speed = 1f;
        }

        foreach (CharacterController controller in allCharacterControllers)
        {
            controller.enabled = true;
        }

        foreach (MouseLook mouseLook in allMouseLookScripts)
        {
            mouseLook.enabled = true;
        }

        isPaused = false;
    }

    public void QuitGame()
    {
        Debug.Log("MainMenu");
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenSettings()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }
}
