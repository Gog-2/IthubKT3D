
using UnityEngine;

public class SpawnOnClick : MonoBehaviour
{
    private GameObject _prefabActive;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Camera cam;
    [SerializeField] private TMPro.TMP_Text text;
    [SerializeField] private NavMeshUpdater navMeshUpdater;

    void Start()
    {
        navMeshUpdater.UpdateNavMesh();
        if (cam == null) cam = Camera.main;
        ChangeNextPrefab(0);
    }

    void Update()
    {
        if (Input.GetKeyDown("1"))ChangeNextPrefab(0);
        if (Input.GetKeyDown("2"))ChangeNextPrefab(1);
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Instantiate(_prefabActive, new Vector3(hit.point.x,0,hit.point.z), Quaternion.identity);
            navMeshUpdater.UpdateNavMesh();
        }
    }
    private void ChangeNextPrefab(int id)
    {
        _prefabActive = prefabs[id];
        text.text = _prefabActive.name;
    }
}