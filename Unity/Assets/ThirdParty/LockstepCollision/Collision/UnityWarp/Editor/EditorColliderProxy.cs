#if UNITY_5_3_OR_NEWER
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DebugUnityColliderProxy))]
public class EditorColliderProxy : Editor
{
    private DebugUnityColliderProxy owner;

    Vector3 posOffset = new Vector3(0, 0, 0);
    float GizmoSize = 0.4f;
    Transform _goParent;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        owner = (DebugUnityColliderProxy) target;

    }


    void OnSceneGUI()
    {
        
        owner = (DebugUnityColliderProxy) target;
        //DrawPathHeap();
    }
    
    /// <summary>
    /// 绘制寻路堆栈
    /// </summary>
    void DrawPathHeap()
    {
        foreach (var col in owner.allCollider)
        {
            var _colType = col.ColType;
            switch (_colType)
            {
                // case   EColType. Sphere: {var _col = col as AABB; Handles.DrawSphere(_col.c.ToVector3(), _col.r.ToVector3* GizmoSize * 10f);break;}
                // case  EColType. AABB: {var _col = col as AABB; Handles.DrawWireCube(_col.c.ToVector3(), _col.r.ToVector3* GizmoSize * 10f);break;}
                // case  EColType. Capsule: {var _col = col as AABB; Handles.DrawWireCube(_col.c.ToVector3(), _col.r.ToVector3* GizmoSize * 10f);break;}
                // case  EColType. OBB: {var _col = col as AABB; Handles.DrawWireCube(_col.c.ToVector3(), _col.r.ToVector3* GizmoSize * 10f);break;}
                // case  EColType. Plane: {var _col = col as AABB; Handles.DrawWireCube(_col.c.ToVector3(), _col.r.ToVector3* GizmoSize * 10f);break;}
                // case  EColType. Rect: {var _col = col as AABB; Handles.DrawWireCube(_col.c.ToVector3(), _col.r.ToVector3* GizmoSize * 10f);break;}
                // case  EColType. Segment: {var _col = col as AABB; Handles.DrawWireCube(_col.c.ToVector3(), _col.r.ToVector3* GizmoSize * 10f);break;}
                // case  EColType. Polygon: {var _col = col as AABB; Handles.DrawWireCube(_col.c.ToVector3(), _col.r.ToVector3* GizmoSize * 10f);break;}
            }
        }
        //Gizmos.DrawCube();
    }
}
#endif