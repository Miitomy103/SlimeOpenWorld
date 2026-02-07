using UnityEngine;

public class MiniMapCameraFogDisable : MonoBehaviour
{
    bool prevFog;

    void OnPreRender()
    {
        prevFog = RenderSettings.fog;
        RenderSettings.fog = false;
    }

    void OnPostRender()
    {
        RenderSettings.fog = prevFog;
    }
}
