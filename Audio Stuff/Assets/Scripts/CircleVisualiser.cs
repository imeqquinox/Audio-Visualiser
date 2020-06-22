using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class CircleVisualiser : MonoBehaviour
{
    [SerializeField] private GameObject sampleCubePrefab;

    private AudioSource source = null;
    private float[] samples = new float[512];
    private float maxScale = 1000f;
    private GameObject[] sampleCubes = new GameObject[512];

    private void Start()
    {
        source = GetComponent<AudioSource>();

        for (int i = 0; i < 512; i++)
        {
            GameObject sampleInstance = Instantiate(sampleCubePrefab);
            sampleInstance.transform.position = this.transform.position;
            sampleInstance.transform.parent = this.transform;
            sampleInstance.name = "SampleCube " + i;
            this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            sampleInstance.transform.position = Vector3.forward * 100f;
            sampleCubes[i] = sampleInstance;
        }
    }

    private void Update()
    {
        GetSpectrumAudioSource();

        for (int i = 0; i < 512; i++)
        {
            if (sampleCubes[i] != null)
            {
                sampleCubes[i].transform.localScale = new Vector3(1f, (samples[i] * maxScale) + 2, 1f);
            }
        }
    }

    private void GetSpectrumAudioSource()
    {
        source.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }
}
