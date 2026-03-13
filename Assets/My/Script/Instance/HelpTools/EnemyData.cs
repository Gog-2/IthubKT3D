using UnityEngine;

public class EnemyData 
{
    public EnemyData(TransformData transform, SpriteRenderData spriteRenderer)
    {
        Transform = transform;
        Sprite = spriteRenderer;
    }
    public readonly TransformData Transform;
    public readonly SpriteRenderData Sprite;
}
