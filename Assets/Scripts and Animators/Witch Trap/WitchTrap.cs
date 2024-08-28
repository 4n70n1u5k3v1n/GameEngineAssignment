using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchTrap : MonoBehaviour
{
    //variable needed to adjust player's settings after being shrunk
    [SerializeField] private float shrinkSize = 0.2f;
    [SerializeField] private float shrinkOffset = 0.2f;
    [SerializeField] private float shrinkSpeed = 3f;
    [SerializeField] private float shrinkJumpSpeed = 3f;
    [SerializeField] private float shrinkGravity = -2f;
    private float shrinkDuration = 4f;

    //variable needed to disable main entrance after being shrunk
    [SerializeField] private GameObject glassDoor;
    [SerializeField] private GameObject exitBlocker;

    //variable needed to enable mouse AI after being shrunk
    [SerializeField] private GameObject mouse;

    //variable needed for shrink cutscene
    private Camera camMain;
    [SerializeField] private GameObject camCutScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //activate particle system
            transform.GetChild(0).gameObject.SetActive(true);

            //shrink player
            StartCoroutine(Shrinking(other));

            //deactivating door
            glassDoor.GetComponent<Animator>().enabled = false;
            glassDoor.GetComponent<opencloseDoor>().enabled = false;

            //cutscene
            StartCoroutine(CutScene(other));
        }
    }

    IEnumerator Shrinking(Collider other)
    {
        Vector3 originalScale = other.transform.localScale;
        float timePassed = 0f;
        while (timePassed <= shrinkDuration)
        {
            other.transform.localScale = Vector3.Lerp(originalScale, originalScale*shrinkSize, timePassed/shrinkDuration);
            timePassed += Time.deltaTime;
            yield return null;
        }

        //ensure player is resized
        other.transform.localScale = originalScale*shrinkSize;

        other.GetComponent<CharacterController>().stepOffset = shrinkOffset;
        other.GetComponent<CharacterMovement>().speed = shrinkSpeed;
        other.GetComponent<CharacterMovement>().jumpSpeed = shrinkJumpSpeed;
        other.GetComponent<CharacterMovement>().gravity = shrinkGravity;

        //disable box collider
        GetComponent<Collider>().enabled = false;
    }

    IEnumerator CutScene(Collider other)
    {
        other.GetComponent<CharacterMovement>().enabled = false;
        other.GetComponent<MouseLook>().enabled = false;
        other.GetComponent<PauseMenu>().enabled = false;
        camMain = Camera.main;
        camMain.gameObject.SetActive(false);
        camCutScene.SetActive(true);
        yield return new WaitForSeconds(shrinkDuration+2);
        exitBlocker.SetActive(true);
        camCutScene.SetActive(false);
        camMain.gameObject.SetActive(true);
        other.GetComponent<CharacterMovement>().enabled = true;
        other.GetComponent<MouseLook>().enabled = true;
        other.GetComponent<PauseMenu>().enabled = true;
        yield return new WaitForSeconds(5);
        mouse.SetActive(true);

        //disable this script
        GetComponent<WitchTrap>().enabled = false;
    }
}
