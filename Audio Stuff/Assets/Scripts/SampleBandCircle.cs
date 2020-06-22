using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class SampleBandCircle : MonoBehaviour
{
    [SerializeField] private GameObject sampleCubePrefab;

    private AudioSource source = null;
    private float[] samples = new float[512];
    private float[] freqSamples = new float[64];
    private GameObject[] sampleCubes = new GameObject[64];
    private Vector3 lastCubePosition = Vector3.zero;

    private void Start()
    {
        source = GetComponent<AudioSource>();

        for (int i = 0; i < 64; i++)
        {
            GameObject sampleInstance = Instantiate(sampleCubePrefab);
            sampleInstance.transform.position = this.transform.position;
            sampleInstance.transform.parent = this.transform;
            sampleInstance.name = "SampleCube " + i;
            this.transform.eulerAngles = new Vector3(0, -5.625f * i, 0);
            sampleInstance.transform.position = Vector3.forward * 60f;
            sampleCubes[i] = sampleInstance;
        }
    }

    private void Update()
    {
        GetSpectrumAudioSource(samples, 0);
        CreateFrequencyBands();

        for (int i = 0; i < 64; i++)
        {
            sampleCubes[i].transform.localScale = new Vector3(sampleCubes[i].transform.localScale.x, freqSamples[i] * 5f, sampleCubes[i].transform.localScale.z);
        }
    }

    private void CreateFrequencyBands()
    {
        int count = 0;
        int sampleCount = 1;
        int power = 0;

        for (int i = 0; i < 64; i++)
        {
            float average = 0;

            if (i == 16 || i == 32 || i == 40 || i == 48 || i == 56)
            {
                power++;
                sampleCount = (int)Mathf.Pow(2, power);
                if (power == 3)
                {
                    sampleCount -= 2;
                }
            }

            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }

            average /= count;
            freqSamples[i] = average * 80;
        }
    }

    private void GetSpectrumAudioSource(float[] samples, int channel)
    {
        source.GetSpectrumData(samples, channel, FFTWindow.Blackman);
    }
}
