using System.Linq;
using System.Collections;
using Engine.Lib;
using UnityEngine;

namespace Engine.UI
{
    /// <summary>
    /// 设计安全区域面板（适配iPhone X）
    /// Jeff 2017-12-1,对需要适配的界面挂此脚本
    /// 文件名 SafeAreaPanel.cs
    /// </summary>
    public class SafeAreaPanel : MonoBehaviour
    {
        private RectTransform target;

        public float Margin = 90f;

        public static float CheckDelay = 1f;     // How long to wait until we check again.
        public DeviceOrientation orientation;    // Current Device Orientation
        bool isAlive = true;                     // Keep this script running?

#if UNITY_EDITOR
        [SerializeField] private bool Simulate_X = false;

        private static readonly string[] IPHONEX_SCREENS = new string[] {"2436,1125", "1792,828", "2688,1242"};
#endif

        void Awake()
        {
            target = GetComponent<RectTransform>();
            ApplySafeArea();
            orientation = Input.deviceOrientation;
            StartCoroutine(CheckForChange());
        }

        // bool IsIPhoneX()
        // {
        //     if (Application.platform != RuntimePlatform.IPhonePlayer)
        //         return false;

        //     var generation = UnityEngine.iOS.Device.generation;

        //     return generation == UnityEngine.iOS.DeviceGeneration.iPhoneX ||
        //         generation == UnityEngine.iOS.DeviceGeneration.iPhoneXS ||
        //         generation == UnityEngine.iOS.DeviceGeneration.iPhoneXSMax ||
        //         generation == UnityEngine.iOS.DeviceGeneration.iPhoneXR;
        // }

        void ApplySafeArea()
        {
            var area = Screen.safeArea;
#if UNITY_EDITOR

            /*
            iPhone X 横持手机方向:
            iPhone X 分辨率
            2436 x 1125 px
    
            safe area
            2172 x 1062 px
    
            左右边距分别
            132px
    
            底边距 (有Home条)
            63px
    
            顶边距
            0px
            */

//            float Xwidth = 2436f;
//            float Xheight = 1125f;
//            float Margin = 90f;
//            float InsetsBottom = 63f;

            var hasAny = IPHONEX_SCREENS.Any(p =>
            {
                var str = $"{Screen.width.ToString()},{Screen.height.ToString()}";
                if (p.Equals(str))
                {
                    return true;
                }

                return false;
            });
            Simulate_X = hasAny;

            if (Simulate_X)
            {
                var sizeOffset = new Vector2(Margin, 0);
                area.position = new Vector2(Margin, 0);
                area.size = area.size - sizeOffset;
            }

//            if ((Screen.width == (int)Xwidth && Screen.height == (int)Xheight) 
//                || (Screen.width == 812 && Screen.height == 375))
//            {
//                Simulate_X = true;
//            }
//
//            if (Simulate_X)
//            {
//                var insets = area.width * Margin / Xwidth;
//                var positionOffset = new Vector2(insets, 0);
//                var sizeOffset = new Vector2(insets * 2, 0);
//                area.position = area.position + positionOffset;
//                area.size = area.size - sizeOffset;
//            }
#endif

#if UNITY_IPHONE 
            if (0 != area.position.x)
            {
                if (orientation == DeviceOrientation.LandscapeRight)
                {
                    area.position = new Vector2(0, 0);
                }
                else
                {
                    area.position = new Vector2(Margin, 0);
                }
                area.size = new Vector2(Screen.width - Margin, Screen.height);
            }
#endif
            var anchorMin = area.position;
            var anchorMax = area.position + area.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            target.anchorMin = anchorMin;
            target.anchorMax = anchorMax;
        }

        IEnumerator CheckForChange()
        {
            while (isAlive)
            {
                // Check for an Orientation Change
                switch (Input.deviceOrientation)
                {
                    case DeviceOrientation.LandscapeLeft:             
                    case DeviceOrientation.LandscapeRight:    
                        if (orientation != Input.deviceOrientation)
                        {
                            orientation = Input.deviceOrientation;
                            ApplySafeArea();
                        }         
                        break; 
                    default:    // Ignore
                        break;
                }

                yield return Yielders.GetWaitForSeconds(CheckDelay); 
            }
        }

        void OnDestroy()
        {
            isAlive = false;
        }
    }
}