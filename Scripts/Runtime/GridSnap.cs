using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class GridSnap : MonoBehaviour
{
#if UNITY_EDITOR
    public float gridStep = 1;
    public bool x;
    public bool y;
    public bool z;

    public Vector3 offset;

    private void Reset()
    {
        offset = transform.localPosition;
    }

    private void Awake()
    {
        if (Application.isPlaying)
            enabled = false;
        offset = transform.localPosition;
    }

    private void Update()
    {
        if (Application.isPlaying) return;
        if (transform.hasChanged && Selection.Contains(gameObject))
        {
            Undo.RecordObject(gameObject, "Transform Change");
            var xPos = transform.localPosition.x; var yPos = transform.localPosition.y; var zPos = transform.localPosition.z;
            if (x) xPos = Mathf.RoundToInt((xPos - offset.x) / gridStep) * gridStep + offset.x;
            if (y) yPos = Mathf.RoundToInt((yPos - offset.y) / gridStep) * gridStep + offset.y;
            if (z) zPos = Mathf.RoundToInt((zPos - offset.z) / gridStep) * gridStep + offset.z;
            transform.localPosition = new Vector3(xPos, yPos, zPos);
            Physics2D.SyncTransforms();
            Physics.SyncTransforms();
        }
    }

    [CustomEditor(typeof(GridSnap))]
    [CanEditMultipleObjects]
    public class GridSnapEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var gridSnap = (GridSnap)target;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("RotateLeft")) gridSnap.transform.Rotate(0, 0, 90);
            if (GUILayout.Button("RotateRight")) gridSnap.transform.Rotate(0, 0, -90);
            GUILayout.EndHorizontal();

        }
    }

    [ContextMenu("Set Offset")]
    public void SetOffset()
    {
        offset = transform.localPosition;
    }
#endif
}
