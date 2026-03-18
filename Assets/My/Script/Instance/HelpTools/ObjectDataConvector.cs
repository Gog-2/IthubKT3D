using UnityEngine;

public static class ObjectDataConvector 
{
    public static TransformData TransformToTransformData(Transform transform)
    {
        Vector3 rot = transform.localEulerAngles; 
        Vector3 pos = transform.position;
        Vector3 scale = transform.localScale;

        return new TransformData
        {
            x_position = pos.x, y_position = pos.y, z_position = pos.z,
            x_rotation = rot.x, y_rotation = rot.y, z_rotation = rot.z,
            x_scale = scale.x, y_scale = scale.y, z_scale = scale.z
        };
    }
    public static TransformData TransformToTransformData(RectTransform transform)
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

    public static void ApplyTransformData(TransformData data, ref RectTransform target)
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
    public static void ApplyTransformData(TransformData data, Transform target)
    {
        if (target == null) return;

        target.position = new Vector3(data.x_position, data.y_position, data.z_position);
        
        target.localRotation = Quaternion.Euler(data.x_rotation, data.y_rotation, data.z_rotation);
        
        float sx = data.x_scale == 0 ? 1 : data.x_scale;
        float sy = data.y_scale == 0 ? 1 : data.y_scale;
        float sz = data.z_scale == 0 ? 1 : data.z_scale;
        target.localScale = new Vector3(sx, sy, sz);
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

    public static void ApplySpriteRenderer(SpriteRenderData data, SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("ApplySpriteRenderer: spriteRenderer is null!");
            return;
        }
        spriteRenderer.color = new Color(data.r,data.g,data.b,data.a);
    }
}
