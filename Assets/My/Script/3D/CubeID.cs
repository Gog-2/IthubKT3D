using My.Script._3D;
using UnityEngine;

public class CubeID : MonoBehaviour, ICanBeActiveted
{
    [SerializeField] private int _layer = 0;
    public int Layer
    { get { return _layer;} set{_layer = value;} }
}
