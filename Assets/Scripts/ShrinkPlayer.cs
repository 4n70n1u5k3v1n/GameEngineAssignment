using SojaExiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkPlayer : MonoBehaviour
{
    //variable needed to adjust player's settings after being shrunk
    [SerializeField] float shrinkSize = 0.2f;
    [SerializeField] float shrinkOffset = 0.2f;
    [SerializeField] float shrinkSpeed = 3f;
    [SerializeField] float shrinkJumpSpeed = 3f;
    [SerializeField] float shrinkGravity = -2f;
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
        other.GetComponent<CharacterMovement1>().speed = shrinkSpeed;
        other.GetComponent<CharacterMovement1>().jumpSpeed = shrinkJumpSpeed;
        other.GetComponent<CharacterMovement1>().gravity = shrinkGravity;
    }

    IEnumerator CutScene(Collider other)
    {
        other.GetComponent<CharacterMovement1>().enabled = false;
        other.GetComponent<MouseLook>().enabled = false;
        camMain = Camera.main;
        camMain.gameObject.SetActive(false);
        camCutScene.SetActive(true);
        yield return new WaitForSeconds(shrinkDuration+2);
        exitBlocker.SetActive(true);
        camCutScene.SetActive(false);
        camMain.gameObject.SetActive(true);
        other.GetComponent<CharacterMovement1>().enabled = true;
        other.GetComponent<MouseLook>().enabled = true;
        yield return new WaitForSeconds(5);
        mouse.SetActive(true);

        //disable this script
        GetComponent<ShrinkPlayer>().enabled = false;
    }
}
