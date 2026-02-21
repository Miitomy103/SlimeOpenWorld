using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイコン種別ごとのスプライトと色を管理する ScriptableObject
/// Assets > Create > Map > IconConfig から作成
/// </summary>
[CreateAssetMenu(menuName = "Map/IconConfig", fileName = "MapIconConfig")]
public class MapIconConfig : ScriptableObject
{
    [System.Serializable]
    public class IconEntry
    {
        public MapIconType type;
        public Sprite      sprite;
        [ColorUsage(false, false)]
        public Color       color  = Color.white;
        [TextArea(1, 2)]
        public string      description;
    }

    [SerializeField]
    private List<IconEntry> entries = new List<IconEntry>
    {
        new IconEntry { type = MapIconType.Custom,     color = new Color(0.85f, 0.85f, 0.85f) },
        new IconEntry { type = MapIconType.Quest,      color = new Color(1.00f, 0.85f, 0.10f) },
        new IconEntry { type = MapIconType.Enemy,      color = new Color(0.90f, 0.20f, 0.20f) },
        new IconEntry { type = MapIconType.Treasure,   color = new Color(0.95f, 0.75f, 0.10f) },
        new IconEntry { type = MapIconType.Town,       color = new Color(0.30f, 0.75f, 0.95f) },
        new IconEntry { type = MapIconType.Dungeon,    color = new Color(0.60f, 0.20f, 0.80f) },
        new IconEntry { type = MapIconType.Waypoint,   color = new Color(0.20f, 0.90f, 0.50f) },
        new IconEntry { type = MapIconType.NPC,        color = new Color(0.40f, 0.80f, 0.40f) },
        new IconEntry { type = MapIconType.FastTravel, color = new Color(0.20f, 0.60f, 1.00f) },
        new IconEntry { type = MapIconType.Danger,     color = new Color(1.00f, 0.40f, 0.10f) },
    };

    private Dictionary<MapIconType, IconEntry> _cache;

    public IconEntry GetEntry(MapIconType type)
    {
        if (_cache == null)
        {
            _cache = new Dictionary<MapIconType, IconEntry>();
            foreach (var e in entries) _cache[e.type] = e;
        }
        return _cache.TryGetValue(type, out var entry) ? entry : null;
    }
}
