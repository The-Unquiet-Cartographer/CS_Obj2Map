using System.Reflection;
using System.ComponentModel.DataAnnotations;
public abstract class Settings {
/*
    public virtual bool MatchAll (Settings otherPreset) {
        FieldInfo[] FI = typeof(Settings).GetFields();
        foreach (FieldInfo f in FI) {
            if (f.GetValue(this) != f.GetValue(otherPreset)) return false;
        }
        return true;
    }
*/
}

public class GeneralSettings : Settings {
    [Range(0.0000001f, float.MaxValue)]public float rounding = 1f;
    public enum OutputFormat {map}
    public OutputFormat outputFormat = OutputFormat.map;
    public enum Optimise_MAP {WorldCraft, Radiant}
    public Optimise_MAP optimiseMap = Optimise_MAP.WorldCraft;
    public bool singleOutput = true;
    [Range(1, int.MaxValue)]public int brushThickness = 8;

    public GeneralSettings () {
        this.rounding = 1f;
        this.outputFormat = OutputFormat.map;
        this.optimiseMap = Optimise_MAP.WorldCraft;
        this.singleOutput = true;
        this.brushThickness = 8;
    }
}

public class GameSettings : Settings {
    public float scaleFactor = 1f;
    public bool reverseVertexOrder = false;
    public bool invertNormals = false;
    public bool subdivideFaces = false;
    public bool swapYZCoordinates = false;
    public bool marathonCeilingFix = false;
    public List<string> excludeTextures = new List<string>();
    public GameSettings () {
        this.scaleFactor = 1f;
        this.reverseVertexOrder = false;
        this.invertNormals = false;
        this.subdivideFaces = false;
        this.swapYZCoordinates = false;
        this.marathonCeilingFix = false;
        this.excludeTextures = new List<string>();
    }
    public /*override*/ bool MatchAll (Settings otherPreset) {
        GameSettings _otherPreset = (GameSettings)otherPreset;
        if (
            this.scaleFactor == _otherPreset.scaleFactor
        &&  this.reverseVertexOrder == _otherPreset.reverseVertexOrder
        &&  this.invertNormals == _otherPreset.invertNormals
        &&  this.swapYZCoordinates == _otherPreset.swapYZCoordinates
        &&  this.marathonCeilingFix == _otherPreset.marathonCeilingFix
        ) {
            return true;
        }
        return false;
    }
}

/*
    public float rounding {get{return _rounding;} set{_rounding = value < 1 ? 1f : value;}}
    public int brushThickness {get{return _brushThickness;} set{_brushThickness = value < 1 ? 1 : value;}}
    public float scaleFactor {get{return _scaleFactor;} set{_scaleFactor = value <= 0 ? 1f : value;}}
*/