// Copyright (c) 2020 Soichiro Sugimoto.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VolumetricStreamingLite.Client
{
    public class ReceiverControllerForHololens : MonoBehaviour
    {
        [SerializeField] ReceiverService _ReceiverService;
        [SerializeField] PointCloudRenderer _PointCloudRenderer;

        [SerializeField] TextMeshProUGUI _ClientIdText;
        [SerializeField] TMP_InputField _ServerAddress;
        [SerializeField] TMP_InputField _ServerPort;
        [SerializeField] Button _Connect;
        [SerializeField] Button _Disconnect;
        [SerializeField] TMP_InputField _StreamingClientId;
        [SerializeField] Button _RegisterReceiver;
        [SerializeField] Button _UnregisterReceiver;

        int _PreviousStreamingClientId;
        Texture2D _DepthImageTexture;
        MeshRenderer _DepthMeshRenderer;
        Texture2D _ColorImageTexture;
        MeshRenderer _ColorMeshRenderer;

        void Start()
        {
            _ReceiverService.OnReceivedCalibration += OnReceivedCalibration;

            _Connect.onClick.AddListener(OnClickConnect);
            _Disconnect.onClick.AddListener(OnClickDisconnect);
            _RegisterReceiver.onClick.AddListener(OnClickRegisterReceiver);
            _UnregisterReceiver.onClick.AddListener(OnClickUnregisterReceiver);
        }

        void Update()
        {
            _ClientIdText.text = "Client ID : " + _ReceiverService.ClientId;

            if (_ReceiverService.DepthImageRawData != null)
            {
                _PointCloudRenderer.UpdateDepthTexture(_ReceiverService.DepthImageRawData);
            }
            if (_ReceiverService.ColorImageData != null)
            {
                _PointCloudRenderer.UpdateColorTextureImageByteArray(_ReceiverService.ColorImageData);
            }
        }

        void OnReceivedCalibration(K4A.CalibrationType calibrationType, K4A.Calibration calibration)
        {
            Debug.Log("CalibrationType: " + calibrationType);
            
            K4A.CalibrationCamera depthCalibrationCamera = calibration.DepthCameraCalibration;
            K4A.CalibrationCamera colorCalibrationCamera = calibration.ColorCameraCalibration;

            Debug.Log("Calibration.DepthImage: " + depthCalibrationCamera.resolutionWidth + "x" + depthCalibrationCamera.resolutionHeight);
            Debug.Log("Calibration.ColorImage: " + colorCalibrationCamera.resolutionWidth + "x" + colorCalibrationCamera.resolutionHeight);
            
            _PointCloudRenderer.GenerateMesh(calibration, calibrationType);
        }

        void OnClickConnect()
        {
            _ReceiverService.Connect(_ServerAddress.text, int.Parse(_ServerPort.text));
        }

        void OnClickDisconnect()
        {
            _ReceiverService.Disconnect();
        }

        void OnClickRegisterReceiver()
        {
            _ReceiverService.UnregisterReceiverClient(_PreviousStreamingClientId);

            int streamingClientId = int.Parse(_StreamingClientId.text);
            _ReceiverService.RegisterReceiverClient(streamingClientId);

            _PreviousStreamingClientId = streamingClientId;

            _ReceiverService.StartReceiving();
        }

        void OnClickUnregisterReceiver()
        {
            _ReceiverService.StopReceiving();

            int streamingClientId = int.Parse(_StreamingClientId.text);
            _ReceiverService.UnregisterReceiverClient(streamingClientId);
        }
    }
}
