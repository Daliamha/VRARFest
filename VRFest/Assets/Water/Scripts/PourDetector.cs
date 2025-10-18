using System;
using UnityEngine;

public class PourDetector : MonoBehaviour
{
    public int pourThreshold = 45;
    public Transform origin = null;
    public GameObject streamPrefab = null;
    
    private bool _isPouring = false;
    private Stream currentStream = null;

    private void Update()
    {
        bool pourCheck = CalculatePourAngle() < pourThreshold;

        if (_isPouring != pourCheck)
        {
            _isPouring = pourCheck;

            if (_isPouring)
            {
                StartPour();
            }
            else
            {
                EndPour();
            }
        }
    }

    private void StartPour()
    {
        currentStream = CreateStream();
        currentStream.Begin();
    }

    private void EndPour()
    {
        currentStream.End();
        currentStream = null;
    }

    private float CalculatePourAngle()
    {
        return transform.forward.y * Mathf.Rad2Deg;
    }

    private Stream CreateStream()
    {
        GameObject streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);
        return streamObject.GetComponent<Stream>();
    }
}