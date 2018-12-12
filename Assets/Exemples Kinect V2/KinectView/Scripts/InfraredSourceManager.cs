using System;
using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class InfraredSourceManager : MonoBehaviour
{
    private KinectSensor _Sensor;
    private DepthFrameReader _Reader;
    private ushort[] _Data;
    private byte[] _RawData;
    private ushort minDist;
    private ushort maxDist;

    // I'm not sure this makes sense for the Kinect APIs
    // Instead, this logic should be in the VIEW
    private Texture2D _Texture;

    public Texture2D GetInfraredTexture()
    {
        return _Texture;
    }

    void Start()
    {
        _Sensor = KinectSensor.GetDefault();
        if (_Sensor != null)
        {
            _Reader = _Sensor.DepthFrameSource.OpenReader();
            var frameDesc = _Sensor.DepthFrameSource.FrameDescription;
            _Data = new ushort[frameDesc.LengthInPixels];
            _RawData = new byte[frameDesc.LengthInPixels * 4];
            _Texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.BGRA32, false);
            
            UpdateLimits();

            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }
    }

    void Update()
    {
        if (_Reader != null)
        {
            var frame = _Reader.AcquireLatestFrame();
            //Debug.Log(frame != null);
            if (frame != null)
            {
                frame.CopyFrameDataToArray(_Data);

                int index = 0;
                foreach (var ir in _Data)
                {
                    byte intensity = (byte) Remap(minDist, maxDist, 255, 0, ir);
                    _RawData[index++] = intensity;
                    _RawData[index++] = intensity;
                    _RawData[index++] = intensity;
                    _RawData[index++] = 255; // Alpha
                }

                _Texture.LoadRawTextureData(_RawData);
                _Texture.Apply();

                frame.Dispose();
                frame = null;
            }
        }
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

    public static ushort Remap(ushort minOld, ushort maxOld, ushort minNew, ushort maxNew, ushort value)
    {
        return (ushort)(minNew + (value - minOld) * (maxNew - minNew) / (maxOld - minOld));
    }

    public void UpdateLimits()
    {
        minDist = _Sensor.DepthFrameSource.DepthMinReliableDistance;
        maxDist = _Sensor.DepthFrameSource.DepthMaxReliableDistance;
    }
}
