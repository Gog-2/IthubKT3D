using System.Collections.Generic;

[System.Serializable]
public class TotalData
{
    public VolumeData volumeData = new VolumeData(); 
    public List<EnemyData> Enemies = new List<EnemyData>();
    public TotalData() { }

    public TotalData(float volume, List<EnemyData> enemies)
    {
        this.volumeData = new VolumeData { Volume = volume };
        this.Enemies = enemies;
    }
}
