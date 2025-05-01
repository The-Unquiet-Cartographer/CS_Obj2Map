public struct Plane {
    private Vector3 _planeNormal;
    private float _d;                   //Distance from origin
    private string _texture;
    public Plane (Vector3 pt1, Vector3 pt2, Vector3 pt3, string texture = "") {
        Vector3 side1 = pt1.DirectionTo(pt2);
        Vector3 side2 = pt2.DirectionTo(pt3);
        Vector3 crossProduct = Vector3.Cross(side1, side2).normalized;
        Vector3 aX_bY_cZ = Vector3.Scale3(crossProduct, pt1);
        //Vector3 aX_bY_cZ = Vector3.Scale(crossProduct, p2);
        //Vector3 aX_bY_cZ = Vector3.Scale(crossProduct, p3);                       //<== In theory this should do the same thing.
        this._planeNormal = crossProduct;
        this._d = aX_bY_cZ.sum * -1;
        this._texture = texture;
    }
    public float a {get{return _planeNormal.x;}}
    public float b {get{return _planeNormal.y;}}
    public float c {get{return _planeNormal.z;}}
    public float d {get{return _d;}}
    public Vector3 normal {get{return _planeNormal.normalized;}}
    public string texture {get{return _texture;}}

//////////////////////////////////////////////////
//
//  FUNCTIONS
//
//////////////////////////////////////////////////

    public static Vector3 Intersection (Plane plane1, Plane plane2, Plane plane3) {
        Plane[] planes = new Plane[] {plane1, plane2, plane3};
        int[] planeOrder = SortPlanes(planes);
        float[][] augmentedMatrix = new float[][] {
            new float[] {planes[planeOrder[0]].a, planes[planeOrder[0]].b, planes[planeOrder[0]].c, planes[planeOrder[0]].d},
            new float[] {planes[planeOrder[1]].a, planes[planeOrder[1]].b, planes[planeOrder[1]].c, planes[planeOrder[1]].d},
            new float[] {planes[planeOrder[2]].a, planes[planeOrder[2]].b, planes[planeOrder[2]].c, planes[planeOrder[2]].d}
        };
        //Is there a stock matrix class and can I use that?

        augmentedMatrix[1] = GaussianElimination(augmentedMatrix[1], 0, augmentedMatrix[0]);
        augmentedMatrix[2] = GaussianElimination(augmentedMatrix[2], 0, augmentedMatrix[0]);
        augmentedMatrix[2] = GaussianElimination(augmentedMatrix[2], 1, augmentedMatrix[1]);
        float z = augmentedMatrix[2][3] / augmentedMatrix[2][2];
        float y = (augmentedMatrix[1][3] - augmentedMatrix[1][2] * z) / augmentedMatrix[1][1];
        float x = (augmentedMatrix[0][3] - augmentedMatrix[0][2] * z - augmentedMatrix[0][1] * y) / augmentedMatrix[0][0];
        return new Vector3(-x, -y, -z);
    }

    public int ContainsPt (Vector3 pt) {
        return Math.Sign(this.a * pt.x + this.b * pt.y + this.c * pt.z + this.d);
    }

    public override string ToString() {
        return this.a+"x + "+this.b+"y + "+this.c+"z = "+this._d;
    }



//Sort planes by how many non-zero properties they have, starting with a. Our resultant matrix might look something like this:
//  a   b   c   d
//  32  8   16  N/A
//  0   12  16  N/A
//  0   0   8   N/A
//Required so that we can progmatically carry out our Gaussian Elimination when we look for an intersection.
    private static int[] SortPlanes (Plane[] _planes) {
    //Count the zeros
        int[] zeros = new int[] {0, 0, 0};
        for (int i = 0; i < 3; i++) {
            if (_planes[i].a == 0) {
                zeros[i]++;
                if (_planes[i].b == 0) {
                    zeros[i]++;
                    if (_planes[i].c == 0) {
                        zeros[i]++;
                    }
                }
            }
        }
        int[] sortedOrder = new int[3];
        int minThreshold = -1;
        for (int i = 0; i < 3; i ++) {
            int smallestValue = int.MaxValue;
            for (int j = 0; j < 3; j++) {
                if (zeros[j] < smallestValue && zeros[j] > minThreshold) {
                    smallestValue = zeros[j];
                    sortedOrder[i] = smallestValue;
                }
            }
            minThreshold = smallestValue;
        }
        return sortedOrder;
    }

//Refactor the matrix by zeroing the target value t of row rX, using row0 as a basis.
//Look, it helps to calculate the intersection point of a trio of planes, okay?
    private static float[] GaussianElimination (float[] rX, int t, float[] r0) {
        float[] newRow = new float[4];
        if (rX[t] != 0) {
        //Multiplication method
            if (r0[t] != 0) {
                float mult_r0 = rX[t]/r0[t];
                for (int i = 0; i < 4; i++) {
                    newRow[i] = r0[i] * mult_r0 - rX[i];
                }
                return newRow;
            }
        //Addition method
            else {
                float add_r0 = rX[t]-r0[t];
                for (int i = 0; i < 4; i++) {
                    newRow[i] = r0[i] + add_r0 - rX[i];
                }
                return newRow;
            }
        }
        else {
            for (int i = 0; i < 4; i++) {
                newRow[i] = rX[i];
            }
            return newRow;
        }
    }

}