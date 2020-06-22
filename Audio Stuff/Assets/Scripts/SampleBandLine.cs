using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SampleBandLine : MonoBehaviour
{
    private AudioSource source = null;
    private LineRenderer lineRenderer = null;
    private float[] samples = new float[512];
    private float[] freqSamples = new float[64];
    private Vector3 offset = Vector3.zero;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 64;
        lineRenderer.numCornerVertices = 5;
        lineRenderer.numCapVertices = 5;

        for (int i = 0; i < 64; i++)
        {
            lineRenderer.SetPosition(i, transform.position + offset);
            offset.x += 5f;
        }
    }

    private void Update()
    {
        GetSpectrumAudioSource(samples, 0);
        CreateFrequencyBands();

        for (int i = 0; i < 64; i++)
        {
            Vector3 temp = new Vector3(lineRenderer.GetPosition(i).x, freqSamples[i] * 5f, lineRenderer.GetPosition(i).z);
            lineRenderer.SetPosition(i, temp);
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
