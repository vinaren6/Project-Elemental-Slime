using UnityEngine;

public class DrawColliderGizmo : MonoBehaviour
{
    #region Variables
    
    [SerializeField] private Color colliderWireColor;
    [SerializeField] private Color colliderSolidColor;
    [SerializeField] private bool drawColliders;
    [SerializeField] private bool findCollidersInChildren;
    [SerializeField] private bool drawDisabledColliders;

    private Transform _transform;
    private Collider[] _colliders;
    
    #endregion

    #region Start Methods

    private void Awake()
    {
        Initialize();
    }
    
    private void Reset()
    {
        colliderSolidColor = new Color(0f, 1f, 0f, 0.5f);
        colliderWireColor = new Color(1f, 0f, 0f, 1f);
        
        Initialize();
    }

    private void OnValidate()
    {
        Initialize();
    }

    private void Initialize()
    {
        GetAllComponents();
    }
    
    private void GetAllComponents()
    {
        _transform = transform;

        if (findCollidersInChildren)
            _colliders = GetComponentsInChildren<Collider>();
        else
            _colliders = GetComponents<Collider>();
    }
    
    #endregion

    #region Draw Methods

    private void DrawCollider(Collider col)
    {
        Vector3 position = col.bounds.center - col.transform.position;
        Gizmos.matrix = _transform.localToWorldMatrix;
        
        if (col.transform != _transform)
            position = col.transform.localPosition;
        
        if (col.GetType() == typeof(BoxCollider))
        {
            BoxCollider boxCollider = (BoxCollider) col;

            Gizmos.color = colliderSolidColor;
            Gizmos.DrawCube(position, boxCollider.size);

            Gizmos.color = colliderWireColor;
            Gizmos.DrawWireCube(position, boxCollider.size);
        }
        else if (col.GetType() == typeof(SphereCollider))
        {
            SphereCollider sphereCollider = (SphereCollider) col;

            Gizmos.color = colliderSolidColor;
            Gizmos.DrawSphere(position + sphereCollider.center, sphereCollider.radius);

            Gizmos.color = colliderWireColor;
            Gizmos.DrawWireSphere(position + sphereCollider.center, sphereCollider.radius);
        }
    }

    #endregion
    
    private void OnDrawGizmos()
    {
        if (!drawColliders)
            return;
        
        if (_colliders == null)
            GetAllComponents();
        
        if (_colliders == null || _colliders.Length <= 0)
            return;

        foreach (Collider col in _colliders)
        {
            if (!drawDisabledColliders && !col.enabled)
                continue;
            
            DrawCollider(col);
        }
    }
}
