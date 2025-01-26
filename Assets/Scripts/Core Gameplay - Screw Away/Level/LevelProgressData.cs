using UnityEngine;

public class LevelProgressData
{
    private ScrewData[] _screwsData;
    private ScrewBoxData[] _screwBoxesData;

    public ScrewData[] ScrewsData
    {
        get => _screwsData;
        set => _screwsData = value;
    }

    public ScrewBoxData[] ScrewBoxesData
    {
        get => _screwBoxesData;
        set => _screwBoxesData = value;
    }
}
