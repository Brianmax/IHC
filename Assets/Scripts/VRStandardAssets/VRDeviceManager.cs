using UnityEngine;
using UnityEngine.XR;

namespace VRStandardAssets.Utils
{
    // This class exists to setup the device on a per platform basis.
    // The class uses the singleton pattern so that only one object exists.
    public class VRDeviceManager : MonoBehaviour
    {
        [SerializeField] private float m_RenderScale = 1.4f;


        private static VRDeviceManager s_Instance;


        public static VRDeviceManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindObjectOfType<VRDeviceManager> ();
                    DontDestroyOnLoad (s_Instance.gameObject);
                }

                return s_Instance;
            }
        }


        private void Awake ()
        {
            if (s_Instance == null)
            {
                s_Instance = this;
                DontDestroyOnLoad (this);
            }
            else if (this != s_Instance)
            {
                Destroy (gameObject);
            }

            SetupVR ();
        }


        private void SetupVR ()
        {
            //Gear VR does not currently support renderScale
#if !UNITY_ANDROID
            //XRSettings.renderScale = m_RenderScale;
#endif

#if UNITY_STANDALONE
            //XRSettings.loadedDevice = XRDeviceType.Oculus;
#endif
            
#if UNITY_PS4 && !UNITY_EDITOR
		    //XRSettings.loadedDevice = XRDeviceType.Morpheus;
#endif

            //XRSettings.enabled = true;
        }
    }
}