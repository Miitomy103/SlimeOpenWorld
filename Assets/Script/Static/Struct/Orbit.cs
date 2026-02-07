using System;

[Serializable]
public struct Orbit
{
    /// <summary>Height relative to target</summary>
    public float m_Height;
    /// <summary>Radius of orbit</summary>
    public float m_Radius;
    /// <summary>Constructor with specific values</summary>
    /// <param name="h">Orbit height</param>
    /// <param name="r">Orbit radius</param>
    public Orbit(float h, float r) { m_Height = h; m_Radius = r; }
}