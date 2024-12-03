using UnityEngine;

public class DebugCollider : MonoBehaviour
{
    private BoxCollider boxCollider;

    public void Initialize(BoxCollider boxCollider)
    {
        this.boxCollider = boxCollider;
    }

    private void OnDrawGizmos()
    {
        if (boxCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
        }
    }
}