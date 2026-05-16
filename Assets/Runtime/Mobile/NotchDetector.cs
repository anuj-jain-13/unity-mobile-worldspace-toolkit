using UnityEngine;

public static class NotchDetector {

    public static bool DeviceHasNotch { get; private set; }

    static NotchDetector() {
        SetDeviceHasNotch();
    }

    private static void SetDeviceHasNotch() {
        // Check if the safe area is different from the full screen size
        Rect safeArea = Screen.safeArea;
        // Check if the safe area is smaller than the full screen width OR height
        if (safeArea.x > 0 || safeArea.y > 0 || safeArea.width < Screen.width || safeArea.height < Screen.height) {
            DeviceHasNotch = true;
            Debug.Log("Device has a notch/cutout.");
        } else {
            DeviceHasNotch = false;
            Debug.Log("Device does not have a notch/cutout.");
        }
    }
}