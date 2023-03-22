private void OnDrawGizmos()
{

    var t = this.transform;
    
    Matrix4x4 oldMatrix = Gizmos.matrix;
    Gizmos.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
    
    var tMat = Matrix4x4.TRS( new Vector3(0,0,length), Quaternion.Euler(Vector3.right * 90), new Vector3(1, 0.01f, 1));

    Gizmos.matrix = math.mul(t.localToWorldMatrix, tMat);
    Gizmos.DrawSphere(Vector3.zero, radius);
    Gizmos.matrix = oldMatrix;
    
    
    Gizmos.color = Color.red;
    
    var l1 = math.mul(t.localToWorldMatrix, Matrix4x4.TRS(new Vector3(0,radius,length),Quaternion.identity, Vector3.one));
    var l2 = math.mul(t.localToWorldMatrix, Matrix4x4.TRS(new Vector3(radius,0,length),Quaternion.identity, Vector3.one));
    var l3 = math.mul(t.localToWorldMatrix, Matrix4x4.TRS(new Vector3(0,-radius,length),Quaternion.identity, Vector3.one));
    var l4 = math.mul(t.localToWorldMatrix, Matrix4x4.TRS(new Vector3(-radius,0,length),Quaternion.identity, Vector3.one));
    
    Gizmos.DrawLine(t.position,l1.c3.xyz);
    Gizmos.DrawLine(t.position,l2.c3.xyz);
    Gizmos.DrawLine(t.position,l3.c3.xyz);
    Gizmos.DrawLine(t.position,l4.c3.xyz);
}