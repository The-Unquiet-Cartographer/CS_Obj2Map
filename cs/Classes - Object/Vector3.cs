using static ExtensionMethods.FloatExtensions;

public struct Vector3 {
    private float _x, _y, _z;
    public Vector3 (float x, float y, float z) {
        this._x = PositiveZero(x);
        this._y = PositiveZero(y);
        this._z = PositiveZero(z);
    }
    public float x {get{return _x;} set{_x = value;}}
    public float y {get{return _y;} set{_y = value;}}
    public float z {get{return _z;} set{_z = value;}}



//SUM OF COMPONENTS
    public float sum {get{return _x + _y + _z;}}

//GET MAGNITUDE
    public float magnitude {get{
        return SquareRoot(
            this._x.Squared() + this._y.Squared() + this._z.Squared()
        );
    }}

//NORMALIZE VECTOR (RETURN WITH A MAGNITUDE OF 1)
    public Vector3 normalized {get{
        float _magnitude = this.magnitude;
        return new Vector3 (
            this._x/_magnitude,
            this._y/_magnitude,
            this._z/_magnitude
        );
    }}

//DIRECTION TO OTHER VECTOR
    public Vector3 DirectionTo (Vector3 destination) {
        return new Vector3 (
            destination._x - this._x,
            destination._y - this._y,
            destination._z - this._z
        );
    }

//DIRECTION FROM OTHER VECTOR
    public Vector3 DirectionFrom (Vector3 origin) {
        return new Vector3 (
            this._x - origin._x,
            this._y - origin._y,
            this._z - origin._z
        );
    }

//DIRECTION BETWEEN TWO VECTORS
    public static Vector3 GetDirection (Vector3 origin, Vector3 destination) {
        return new Vector3 (
            destination._x - origin._x,
            destination._y - origin._y,
            destination._z - origin._z
        );
    } 

//DOT PRODUCT OF TWO VECTORS
    public static float Dot (Vector3 v1, Vector3 v2) {
        return (v1.x * v2.x) + (v1.y * v2.y) + (v1.z * v2.z);
    }

//CROSS PRODUCT OF TWO VECTORS
    public static Vector3 Cross (Vector3 v1, Vector3 v2) {
        return new Vector3 (
            (v1._y * v2._z) - (v1._z * v2._y),
            (v1._z * v2._x) - (v1._x * v2._z),
            (v1._x * v2._y) - (v1._y * v2._x)
        );
    }

//SIGN VECTOR
    public Vector3 Sign (byte fidelity) {
    //FIDELITY 3: Sign all components.
        if (fidelity >= 3) {
            return new Vector3(
                Math.Sign(this._x),
                Math.Sign(this._y),
                Math.Sign(this._z)
            );
        }
    //FIDELITY 1: Only sign the biggest component.
        Vector3 signedVector = new Vector3(0,0,0);
        if (Math.Abs(this._y) > Math.Abs(this._x)) {
            if (Math.Abs(this._z) > Math.Abs(this._y)) {
                signedVector._z = Math.Sign(this._z);
            }
            else signedVector._y = Math.Sign(this._y);
        }
        else if (Math.Abs(this._z) > Math.Abs(this._x)) {
            signedVector._z = Math.Sign(this._z);
        }
        else signedVector._x = Math.Sign(this._x);
        if (fidelity < 2) return signedVector;
    //FIDELITY 2: Get half the size of the biggest component. If the other components are > this value, sign them as well.
    //  Doing it this way means the return may have 2 OR 3 non-zero components, which may be useful when dealing with, for example, vertex or plane normals.
        float halfBig = Math.Abs(Vector3.Dot(this, signedVector))/2;
        if (Math.Abs(this._x) > halfBig) signedVector._x = Math.Sign(this._x);
        if (Math.Abs(this._y) > halfBig) signedVector._y = Math.Sign(this._y);
        if (Math.Abs(this._z) > halfBig) signedVector._z = Math.Sign(this._z);
        return signedVector;
    }

//INVERT VECTOR
    public Vector3 Invert () {
        return new Vector3(
            -this._x,
            -this._y,
            -this._z
        );
    }

//ADD TWO VECTORS
    public static Vector3 Add (Vector3 v1, Vector3 v2) {
        return new Vector3 (
            v1._x + v2._x,
            v1._y + v2._y,
            v1._z + v2._z
        );
    }
    
//SCALE (MULTIPLY) BY ANOTHER VECTOR
    public static Vector3 Scale3 (Vector3 v1, Vector3 v2) {
        return new Vector3 (
            v1._x * v2._x,
            v1._y * v2._y,
            v1._z * v2._z
        );
    }

//SCALE BY A FLAT VALUE
    public static Vector3 Scale1 (Vector3 vector, float scalar) {
        return new Vector3 (
            vector._x * scalar,
            vector._y * scalar,
            vector._z * scalar
        );
    }
    public Vector3 Scale1 (float scalar) {
        return new Vector3 (
            this._x * scalar,
            this._y * scalar,
            this._z * scalar
        );
    }

//ROUND VECTOR COMPONENTS
    public Vector3 Round (float rounding) {
        return new Vector3 (
            ExtensionMethods.FloatExtensions.Round(this._x, rounding),
            ExtensionMethods.FloatExtensions.Round(this._y, rounding),
            ExtensionMethods.FloatExtensions.Round(this._z, rounding)
        );
    }

//VALIDATE COMPONENT VALUES
    public bool IsValid () {
        if (!float.IsFinite(this._x) || !float.IsFinite(this._y) || !float.IsFinite(this._z)) return false;
        return true;
    }

//TO STRING
    public override string ToString () {
        return "("+this._x+", "+this._y+", "+this._z+")";
    }

//
//  GET ANGLE BETWEEN VECTORS
//

//UNSIGNED
//  Using law of cosines.
//  Returns value in radians.
    public static float UnsignedAngle (Vector3 v1, Vector3 v2) {
        float mag_0_1 = v1.magnitude;
        float mag_0_2 = v2.magnitude;
        float mag_1_2 = GetDirection(v1, v2).magnitude;
        return CosC(mag_0_1, mag_0_2, mag_1_2);
    }
    public static float UnsignedAngle (Vector3 axis, Vector3 pt1, Vector3 pt2) {
        float mag_0_1 = GetDirection(axis, pt1).magnitude;
        float mag_0_2 = GetDirection(axis, pt2).magnitude;
        float mag_1_2 = GetDirection(pt1, pt2).magnitude;
        return CosC(mag_0_1, mag_0_2, mag_1_2);
    }
//  Get angle C
    private static float CosC (float lengthA, float lengthB, float lengthC) {
        return MathF.Acos(
            (lengthA.Squared() + lengthB.Squared() - lengthC.Squared())
            / (2 * lengthA * lengthB)
        );
    }

//SIGNED  
//  Using atan2.
//  Returns value in radians.
//  Left-handed and right-handed variants.
    public static float SignedAngle_R (Vector3 axis, Vector3 v1, Vector3 v2) {
        return MathF.Atan2(Vector3.Dot(Vector3.Cross(v1, v2), axis), Vector3.Dot(v1, v2));
    }
    public static float SignedAngle_L (Vector3 axis, Vector3 v1, Vector3 v2) {
        return MathF.Atan2(Vector3.Dot(Vector3.Cross(v2, v1), axis), Vector3.Dot(v1, v2));
    }

}