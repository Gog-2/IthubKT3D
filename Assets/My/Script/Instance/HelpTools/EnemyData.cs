
[System.Serializable]
public struct EnemyData 
{
    public TransformData Transform;
    public SpriteRenderData Sprite;

    public EnemyData(TransformData transform, SpriteRenderData spriteRenderer)
    {
        Transform = transform;
        Sprite = spriteRenderer;
    }
}
