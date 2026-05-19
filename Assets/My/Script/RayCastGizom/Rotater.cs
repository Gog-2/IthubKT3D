using UnityEngine;
using DG.Tweening;
public class Rotater : MonoBehaviour
{
    [SerializeField] private float speedRotate;
    [SerializeField] private Transform rotaterObject;

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (horizontal != 0 || vertical != 0)
        {
            rotaterObject.DOKill();
            Vector3 targetRotation = new Vector3(vertical * speedRotate, horizontal * speedRotate, 0);
            rotaterObject.DORotate(targetRotation, 0.1f, RotateMode.LocalAxisAdd)
                .OnUpdate(() =>
                {
                    Vector3 currentAngles = rotaterObject.localEulerAngles;
                    currentAngles.z = 0;
                    rotaterObject.localEulerAngles = currentAngles;
                });
        }
    }
}
