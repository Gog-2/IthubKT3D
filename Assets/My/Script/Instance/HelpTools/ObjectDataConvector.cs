using UnityEngine;

public static class ObjectDataConvector 
{
    public static TransformData TransformToTransformData(Transform transform)
    {
        return new TransformData
        {
            x_position = transform.position.x,
            z_position = transform.position.z,
            y_position = transform.position.y,
            x_rotation = transform.rotation.x,
            y_rotation = transform.rotation.y,
            z_rotation = transform.rotation.z,
            x_scale = transform.localScale.x,
            z_scale = transform.localScale.z,
            y_scale = transform.localScale.y
        };
    }

    public static void ApplyTransformData(TransformData data, ref Transform target)
    {
        target.position = new Vector3(
            data.x_position,
            data.y_position,
            data.z_position
        );
        
        target.rotation = Quaternion.Euler(
            data.x_rotation,
            data.y_rotation,
            data.z_rotation
        );

        target.localScale = new Vector3(
            data.x_scale,
            data.y_scale,
            data.z_scale
        );
    }

    public static SpriteRenderData GetSpriteRenderer(SpriteRenderer spriteRenderer)
    {
        return new SpriteRenderData
        {
            r = spriteRenderer.color.r,
            g = spriteRenderer.color.g,
            b = spriteRenderer.color.b,
            a = spriteRenderer.color.a
        };
    }

    public static void ApplySpriteRenderer(SpriteRenderData data,ref SpriteRenderer spriteRenderer)
    {
        spriteRenderer.color = new Color(data.r,data.g,data.b,data.a);
    }
}
