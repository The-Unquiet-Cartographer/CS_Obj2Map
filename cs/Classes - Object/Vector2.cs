using static ExtensionMethods.FloatExtensions;

public struct Vector2 {
    private float _x, _y;
    public Vector2 (float x, float y) {
        this._x = PositiveZero(x);
        this._y = PositiveZero(y);
    }
    public float x {get{return _x;} set{_x = value;}}
    public float y {get{return _y;} set{_y = value;}}


//SUM OF COMPONENTS
    public float sum {get{return _x + _y;}}

//GET MAGNITUDE
    public float magnitude {get{
        return SquareRoot(
            this._x.Squared() + this._y.Squared()
        );
    }}

//NORMALIZE VECTOR (RETURN WITH A MAGNITUDE OF 1)
    public Vector2 normalized {get{
        float _magnitude = this.magnitude;
        return new Vector2 (
            this._x/_magnitude,
            this._y/_magnitude
        );
    }}

//DIRECTION TO OTHER VECTOR
    public Vector2 DirectionTo (Vector2 destination) {
        return new Vector2 (
            destination._x - this._x,
            destination._y - this._y
        );
    }

//DIRECTION FROM OTHER VECTOR
    public Vector2 DirectionFrom (Vector2 origin) {
        return new Vector2 (
            this._x - origin._x,
            this._y - origin._y
        );
    }

//DIRECTION BETWEEN TWO VECTORS
    public static Vector2 GetDirection (Vector2 origin, Vector2 destination) {
        return new Vector2 (
            destination._x - origin._x,
            destination._y - origin._y
        );
    } 

//DOT PRODUCT OF TWO VECTORS
    public static float Dot (Vector2 v1, Vector2 v2) {
        return (v1.x * v2.x) + (v1.y * v2.y);
    }

/*
//CROSS PRODUCT OF TWO VECTORS
    public static Vector3 Cross (Vector3 v1, Vector3 v2) {
        return new Vector3 (
            (v1._y * v2._z) - (v1._z * v2._y),
            (v1._z * v2._x) - (v1._x * v2._z),
            (v1._x * v2._y) - (v1._y * v2._x)
        );
    }
*/

//SIGN VECTOR
    public Vector2 Sign (byte fidelity) {
    //FIDELITY 3: Sign all components.
        if (fidelity >= 3) {
            return new Vector2(
                Math.Sign(this._x),
                Math.Sign(this._y)
            );
        }
    //FIDELITY 1: Only sign the biggest component.
        Vector2 signedVector = new Vector2(0,0);
        if (Math.Abs(this._y) > Math.Abs(this._x)) {
            signedVector._y = Math.Sign(this._y);
        }
        else signedVector._x = Math.Sign(this._x);
        if (fidelity < 2) return signedVector;
    //FIDELITY 2: Get half the size of the biggest component. If the other components are > this value, sign them as well.
    //  Doing it this way means the return may have 2 OR 3 non-zero components, which may be useful when dealing with, for example, vertex or plane normals.
        float halfBig = Math.Abs(Vector2.Dot(this, signedVector))/2;
        if (Math.Abs(this._x) > halfBig) signedVector._x = Math.Sign(this._x);
        if (Math.Abs(this._y) > halfBig) signedVector._y = Math.Sign(this._y);
        return signedVector;
    }

//INVERT VECTOR
    public Vector2 Invert () {
        return new Vector2(
            -this._x,
            -this._y
        );
    }

//ADD TWO VECTORS
    public static Vector2 Add (Vector2 v1, Vector2 v2) {
        return new Vector2 (
            v1._x + v2._x,
            v1._y + v2._y
        );
    }
    
//SCALE (MULTIPLY) BY ANOTHER VECTOR
    public static Vector2 Scale2 (Vector2 v1, Vector2 v2) {
        return new Vector2 (
            v1._x * v2._x,
            v1._y * v2._y
        );
    }

//SCALE BY A FLAT VALUE
    public static Vector2 Scale1 (Vector2 vector, float scalar) {
        return new Vector2 (
            vector._x * scalar,
            vector._y * scalar
        );
    }
    public Vector2 Scale1 (float scalar) {
        return new Vector2 (
            this._x * scalar,
            this._y * scalar
        );
    }

//ROUND VECTOR COMPONENTS
    public Vector2 Round (float rounding) {
        return new Vector2 (
            ExtensionMethods.FloatExtensions.Round(this._x, rounding),
            ExtensionMethods.FloatExtensions.Round(this._y, rounding)
        );
    }

//VALIDATE COMPONENT VALUES
    public bool IsValid () {
        if (!float.IsFinite(this._x) || !float.IsFinite(this._y)) return false;
        return true;
    }

//TO STRING
    public override string ToString () {
        return "("+this._x+", "+this._y+")";
    }

//
//  GET ANGLE BETWEEN VECTORS
//

//UNSIGNED
//  Using law of cosines.
//  Returns value in radians.
    public static float UnsignedAngle (Vector2 v1, Vector2 v2) {
        float mag_0_1 = v1.magnitude;
        float mag_0_2 = v2.magnitude;
        float mag_1_2 = GetDirection(v1, v2).magnitude;
        return CosC(mag_0_1, mag_0_2, mag_1_2);
    }
    public static float UnsignedAngle (Vector2 axis, Vector2 pt1, Vector2 pt2) {
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

/*
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
*/

}