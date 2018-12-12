using System;
using UnityEngine;
using System.Collections;
using Windows.Kinect;
using static Utils;

public class DepthManager : MonoBehaviour
{
    private KinectSensor _Sensor;
    private DepthFrameReader _Reader;
    private ushort[] _Data;
    private byte[] _RawData;
    public ushort minDist;
    public ushort maxDist;
    private ushort[,] _DataArray;

    public int camWidth;
    public int camHeight;

    public ushort backgroundDepth;

    public bool debugCam = true;

    // I'm not sure this makes sense for the Kinect APIs
    // Instead, this logic should be in the VIEW
    private Texture2D _Texture;

    public Texture2D GetInfraredTexture()
    {
        return _Texture;
    }

    void Start()
    {
        InitiateSensor();
    }

    void Update()
    {
        UpdateDepthTexture();


    }


    void OnApplicationQuit()
    {
        if (_Reader != null)
        {
            _Reader.Dispose();
            _Reader = null;
        }

        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }

            _Sensor = null;
        }
    }

    public void UpdateLimits()
    {
        minDist = _Sensor.DepthFrameSource.DepthMinReliableDistance;
        maxDist = _Sensor.DepthFrameSource.DepthMaxReliableDistance;
    }

    public void InitiateSensor()
    {
        _Sensor = KinectSensor.GetDefault();

        if (_Sensor != null)
        {
            _Reader = _Sensor.DepthFrameSource.OpenReader();
            var frameDesc = _Sensor.DepthFrameSource.FrameDescription;
            _Data = new ushort[frameDesc.LengthInPixels];
            _RawData = new byte[frameDesc.LengthInPixels * 4];
            camWidth = frameDesc.Width;
            camHeight = frameDesc.Height;
            _Texture = new Texture2D(camWidth, camHeight, TextureFormat.BGRA32, false);
            _DataArray = new ushort [camWidth,camHeight];

            UpdateLimits();


            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }
    }

    public void UpdateDepthTexture()
    {
        if (_Reader != null)
        {
            var frame = _Reader.AcquireLatestFrame();
            //Debug.Log(frame != null);
            if (frame != null)
            {
                frame.CopyFrameDataToArray(_Data);

                for (int i = 0; i < _Data.Length; i++)
                {
                    //byte intensity = (byte) _Data[i];

                    _DataArray[i % camWidth, Mathf.FloorToInt(i / camWidth)] = _Data[i];
                }

                


                if (debugCam)
                {
                    int index = 0;
                    foreach (var ir in _Data)
                    {
                        byte intensity = (byte)Remap(minDist, maxDist, 255, 0, ir);
                        _RawData[index++] = intensity;
                        _RawData[index++] = intensity;
                        _RawData[index++] = intensity;
                        _RawData[index++] = 255; // Alpha
                    }
                }

                else
                {
                    int index = 0;
                    foreach (var ir in _Data)
                    {
                        byte intensity = (byte) (ir >= backgroundDepth ? 0 : 255);
                        _RawData[index++] = intensity;
                        _RawData[index++] = intensity;
                        _RawData[index++] = intensity;
                        _RawData[index++] = 255; // Alpha
                    }
                }

                if (backgroundDepth == 0)
                    SetDisplayDepth();


                /*int niktarace = 0;
                for (int i = 0; i < camWidth; i++)
                {
                    for (int j = 0; j < camHeight; j++)
                    {
                        float value = Remap(minDist, maxDist, 0f, 1f, _DataArray[i, j]);
                        Color newPixel = new Color(value, value, value, 1);
                        _Texture.SetPixel(i, j, newPixel);
                    }
                }*/

                
                _Texture.LoadRawTextureData(_RawData);
                _Texture.Apply();

                frame.Dispose();
                frame = null;
            }
        }
    }


    public void SetDisplayDepth()
    {
        backgroundDepth = _DataArray[camWidth / 2, camHeight / 2];
        Debug.Log(_Texture.GetPixel(camWidth / 2, camHeight / 2));
    }

}
