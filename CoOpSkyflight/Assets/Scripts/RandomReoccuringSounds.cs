using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomReoccuringSounds : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private float minDelay;

    [SerializeField]
    private float maxDelay;

    private float lastplayedTime;
    private float nextplayTime;

    // Start is called before the first frame update
    void Awake()
    {
        lastplayedTime = Time.realtimeSinceStartup;
        nextplayTime = UnityEngine.Random.Range(minDelay, maxDelay);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup >= lastplayedTime + nextplayTime)
            PlaySound();
    }

    private void PlaySound()
    {
        audioSource.PlayOneShot(audioSource.clip);
        lastplayedTime = Time.realtimeSinceStartup;
        nextplayTime = UnityEngine.Random.Range(minDelay, maxDelay);
    }
}
