using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameEnd : MonoBehaviour
{

    public Text gameOver;
    [SerializeField] AudioClip _hit;
    private void OnTriggerEnter(Collider other)
    {
        playerController PlayerController = other.GetComponent<playerController>();
        if (PlayerController != null) {
            PlayerController.gameObject.SetActive(false);
            AudioSource audioSource = hitAudio.PlayClip2D(_hit, 1);
            gameOver.text = "Game Over";
        } 
    }
}
