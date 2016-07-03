using UnityEngine;
using System.Collections.Generic;

namespace DNA.FlowerDesigner {

     using UnityEngine;
     using System.Collections;
     using System.Collections.Generic;
     
     /*
     http://www.unifycommunity.com/wiki/index.php?title=Triangulator
     */
         
     public class Triangulator
     {
         private List<Vector2> m_points = new List<Vector2>();

         public List<TempTri> tris = new List<TempTri> ();
         
         public Triangulator (Vector3[] points) {
            m_points.Clear ();
            tris.Clear ();
            List<Vector2> v2points = new List<Vector2> ();
            for (int i = 0; i < points.Length; i += 3) {
                Vector3 side1 = points[i+1] - points[i];
                Vector3 side2 = points[i+2] - points[i];
                Vector3 normal = Vector3.Cross (side1, side2).normalized;
                Quaternion qNormal = Quaternion.Euler (normal);
                Quaternion forward = Quaternion.Euler (Vector3.forward);
                Quaternion direction = Quaternion.Lerp (qNormal, forward, 0.5f);
                Vector3 vDirection = direction.ToVector3 ();
                // Debug.Log (vDirection.x + ", " + vDirection.y);
                tris.Add (new TempTri (new Vector3[] { points[i], points[i+1], points[i+2] }, side1, side2, normal, direction));

                Vector3 p1 = direction * points[i];
                Vector3 p2 = direction * points[i+1];
                Vector3 p3 = direction * points[i+2];

                /*Debug.Log ("--");
                Debug.Log (p1);
                Debug.Log (p2);
                Debug.Log (p3);*/

                v2points.Add (new Vector2 (p1.x, p1.y));
                v2points.Add (new Vector2 (p2.x, p2.y));
                v2points.Add (new Vector2 (p3.x, p3.y));
            }

            m_points = v2points;
            /*m_points = new List<Vector2> ();
            m_points.Add (v2points[6]);
            m_points.Add (v2points[7]);
            m_points.Add (v2points[8]);*/
         }

         public Triangulator (Vector2[] points) {
             m_points = new List<Vector2>(points);
         }
         
         public int[] Triangulate() {
             List<int> indices = new List<int>();
             
             int n = m_points.Count;
             if (n < 3) {
                Debug.LogWarning ("not enough vertices to triangulate vertices");
                return indices.ToArray();
             }
             
             int[] V = new int[n];
             if (Area() > 0) {
                 for (int v = 0; v < n; v++)
                     V[v] = v;
             }
             else {
                 for (int v = 0; v < n; v++)
                     V[v] = (n - 1) - v;
             }
             
             int nv = n;
             int count = 2 * nv;
             for (int m = 0, v = nv - 1; nv > 2; ) {

                 if ((count--) <= 0) {
                    Debug.LogWarning ("cannot triangulate because vertices do not fan");
                    return indices.ToArray();
                 }
                 
                 int u = v;
                 if (nv <= u)
                     u = 0;
                 v = u + 1;
                 if (nv <= v)
                     v = 0;
                 int w = v + 1;
                 if (nv <= w)
                     w = 0;
                 
                 if (Snip(u, v, w, nv, V)) {
                     int a, b, c, s, t;
                     a = V[u];
                     b = V[v];
                     c = V[w];
                     indices.Add(a);
                     indices.Add(b);
                     indices.Add(c);
                     m++;
                     for (s = v, t = v + 1; t < nv; s++, t++)
                         V[s] = V[t];
                     nv--;
                     count = 2 * nv;
                 }
             }
             
             indices.Reverse();
             return indices.ToArray();
         }
         
         private float Area () {
             int n = m_points.Count;
             float A = 0.0f;
             for (int p = n - 1, q = 0; q < n; p = q++) {
                 Vector2 pval = m_points[p];
                 Vector2 qval = m_points[q];
                 A += pval.x * qval.y - qval.x * pval.y;
             }
             return (A * 0.5f);
         }
         
         private bool Snip (int u, int v, int w, int n, int[] V) {
             int p;
             Vector2 A = m_points[V[u]];
             Vector2 B = m_points[V[v]];
             Vector2 C = m_points[V[w]];
             if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x))))
                 return false;
             for (p = 0; p < n; p++) {
                 if ((p == u) || (p == v) || (p == w))
                     continue;
                 Vector2 P = m_points[V[p]];
                 if (InsideTriangle(A, B, C, P))
                     return false;
             }
             return true;
         }
         
         private bool InsideTriangle (Vector2 A, Vector2 B, Vector2 C, Vector2 P) {
             float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
             float cCROSSap, bCROSScp, aCROSSbp;
             
             ax = C.x - B.x; ay = C.y - B.y;
             bx = A.x - C.x; by = A.y - C.y;
             cx = B.x - A.x; cy = B.y - A.y;
             apx = P.x - A.x; apy = P.y - A.y;
             bpx = P.x - B.x; bpy = P.y - B.y;
             cpx = P.x - C.x; cpy = P.y - C.y;
             
             aCROSSbp = ax * bpy - ay * bpx;
             cCROSSap = cx * apy - cy * apx;
             bCROSScp = bx * cpy - by * cpx;
             
             return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
         }

         public void Draw () {

            for (int i = 0; i < tris.Count; i ++) {

                TempTri tri = tris[i];

                Vector3[] points = tri.points;
                Vector3 normal = tri.normal;

                Debug.DrawLine(points[0],points[1],Color.red);
                Debug.DrawLine(points[0],points[2],Color.red);
                Debug.DrawLine(points[1],points[2],Color.red);

                // Debug.DrawLine (tri.side1, tri.side1 * 1.1f, Color.green);
                Debug.DrawLine (normal, normal * 1.2f, Color.white);

                Vector3 dir = tri.direction.ToVector3 ();
                Debug.DrawLine (dir * 1.2f, dir * 1.4f, Color.green);
            }
         }
     }

     public class TempTri {

        public Vector3[] points;
        public Vector3 side1;
        public Vector3 side2;
        public Vector3 normal;
        public Quaternion direction;

        public TempTri (Vector3[] points, Vector3 side1, Vector3 side2, Vector3 normal, Quaternion direction) {
            this.points = points;
            this.side1 = side1;
            this.side2 = side2;
            this.normal = normal;
            this.direction = direction;
        }
     }

}

