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
    private MouseLook[] allMouseLookScripts;

    void Start()
    {
        pauseMenu.SetActive(false);
        allAnimators = FindObjectsOfType<Animator>();
        allCharacterControllers = FindObjectsOfType<CharacterController>();
        allMouseLookScripts = FindObjectsOfType<MouseLook>();
    }

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
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
