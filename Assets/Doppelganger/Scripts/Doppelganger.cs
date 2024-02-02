using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace MomomaAssets.Udon
{
    public class Doppelganger : UdonSharpBehaviour
    {
        [SerializeField]
        Camera m_DoppelCamera = null;
        [SerializeField]
        Camera m_PlayerCamera = null;
        [SerializeField]
        Transform m_Center = null;
        [SerializeField]
        Transform m_PlayerScale = null;
        [SerializeField]
        Slider m_ScaleSlider = null;
        [SerializeField]
        Toggle m_FixedModeToggle = null;
        [SerializeField]
        Transform m_EditorCamera = null;

        Transform m_DoppelParent = null;
        Transform m_PlayerParent = null;
        Transform m_Parent = null;

        void Start()
        {
            if ((Networking.LocalPlayer == null && m_EditorCamera == null) || m_DoppelCamera == null || m_PlayerCamera == null || m_Center == null || m_PlayerScale == null || m_ScaleSlider == null || m_FixedModeToggle == null)
            {
                this.enabled = false;
                return;
            }
            m_DoppelParent = m_DoppelCamera.transform.parent;
            m_PlayerParent = m_PlayerCamera.transform.parent;
            m_Parent = transform.parent;
            if (m_DoppelParent == null || m_PlayerParent == null || m_Parent == null)
            {
                this.enabled = false;
                return;
            }
            m_DoppelCamera.gameObject.SetActive(true);
            m_DoppelCamera.enabled = true;
            m_PlayerCamera.gameObject.SetActive(true);
            m_PlayerCamera.enabled = true;
        }

        void LateUpdate()
        {
            Vector3 trackingPosition;
            Quaternion trackingRotation;
            var offset = m_Parent.position;
            var centerPosition = m_Center.position;
            var centerRotation = m_Center.rotation;
            var localPlayer = Networking.LocalPlayer;
            var scaleFactor = m_ScaleSlider.value;
            if (localPlayer != null)
            {
                var trackingData = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
                trackingPosition = trackingData.position;
                trackingRotation = trackingData.rotation;
                if (m_FixedModeToggle.isOn)
                {
                    centerRotation = Quaternion.Inverse(centerRotation) * localPlayer.GetRotation();
                    centerPosition = localPlayer.GetPosition() + centerRotation * (trackingPosition - centerPosition) / scaleFactor;
                }
                else
                {
                    centerPosition = offset + (centerPosition + centerRotation * trackingPosition) / scaleFactor;
                }
            }
            else
            {
                trackingPosition = m_EditorCamera.position;
                trackingRotation = m_EditorCamera.rotation;
                centerPosition = offset + (centerPosition + centerRotation * trackingPosition) / scaleFactor;
            }
            m_PlayerParent.rotation = trackingRotation * Quaternion.Inverse(m_PlayerCamera.transform.localRotation);
            m_PlayerParent.position = trackingPosition - m_PlayerParent.rotation * m_PlayerCamera.transform.localPosition;
            m_DoppelParent.rotation = centerRotation * trackingRotation * Quaternion.Inverse(m_DoppelCamera.transform.localRotation);
            m_DoppelParent.position = centerPosition - m_DoppelParent.rotation * m_DoppelCamera.transform.localPosition;
            var sclae = m_PlayerScale.localScale;
            sclae.x = 1f / sclae.x;
            sclae.y = 1f / sclae.y;
            sclae.z = 1f / sclae.z;
            m_PlayerCamera.transform.localScale = sclae;
            m_DoppelCamera.transform.localScale = sclae / scaleFactor;
            m_DoppelCamera.nearClipPlane = m_PlayerCamera.nearClipPlane / scaleFactor;
            m_DoppelCamera.farClipPlane = m_PlayerCamera.farClipPlane / scaleFactor;
        }
    }
}
