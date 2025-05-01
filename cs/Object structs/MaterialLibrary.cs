public class MaterialLibrary {

    public static Material defaultMaterial_WorldCraft = new Material("#DEFAULT", "", "", 128, 128);
    public static Material defaultMaterial_Radiant = new Material("common/caulk", "", "", 64, 64);

    private List<Material> materials = new List<Material>();    
    public MaterialLibrary (in List<InputManager.FileData> fileData) {
        this.materials = new List<Material>();
        foreach(InputManager.FileData fD in fileData) {
            string[] splitContents;
            switch (fD.fileType) {
            //.obj
                case InputManager.FileType.obj:
                splitContents = fD.fileContents.Split("usemtl ");
                if (splitContents.Length < 2) break;
                for (int j = 1; j < splitContents.Length; j++) {
                    string material = splitContents[j].Substring(0, splitContents[j].IndexOf('\n')).Trim();
                    if (!this.ContainsMaterial(material)) {
                        this.AddNewMaterial(material);
                        //ConsoleManager.WriteColoredLine(ConsoleColor.Yellow, "Adding material \""+material+"\"");
                    }
                }
                break;
            //.mtl
                case InputManager.FileType.mtl:
                splitContents = fD.fileContents.Split("newmtl ");
                if (splitContents.Length < 2) break;
                for (int j = 1; j < splitContents.Length; j++) {
                    string material = splitContents[j].Substring(0, splitContents[j].IndexOf('\n')).Trim();
                    int textureIndex = splitContents[j].IndexOf("map_Kd ")+7;
                    string texture = splitContents[j].Substring(textureIndex, splitContents[j].IndexOf('\n', textureIndex)-textureIndex).Trim();
                    string textureExtension = "";
                    {
                        int s = texture.LastIndexOf('.');
                        if (s > 0) {
                            textureExtension = texture.Substring(s+1);
                            texture = texture.Remove(s);
                        }
                    }
                    if (!this.ContainsMaterial(material)) {
                        this.AddNewMaterial(material, texture, textureExtension);
                        //ConsoleManager.WriteColoredLine(ConsoleColor.Green, "Adding material \""+material+"\" \""+texture+"\"");
                    }
                    else {
                        this.AddToExistingMaterials(material, texture, textureExtension);
                        //ConsoleManager.WriteColoredLine(ConsoleColor.Green, "Updating material \""+material+"\" \""+texture+"\"");
                    }
                }
                break;
            }
        }
    //.bmp
    //Do these last so that we can add width/height to texture references
        foreach(InputManager.FileData fD in fileData) {            
            if (fD.fileType == InputManager.FileType.bmp) {
                string texture = fD.fileName;
                string textureExtension = fD.fileExtension;
                int width, height;
                FileReader.ReadBitmapSize(fD, out width, out height);
                if (!this.ContainsTexture(texture)) {
                    this.AddNewMaterial("", texture, textureExtension, width, height);
                    //ConsoleManager.WriteColoredLine(ConsoleColor.Magenta, "Adding texture \""+texture+"\" \""+width+"\" \""+height+"\"");
                }
                else {
                    this.AddToExistingTextures(texture, width, height);
                    //ConsoleManager.WriteColoredLine(ConsoleColor.Magenta, "Updating material \""+texture+"\" \""+width+"\" \""+height+"\"");
                }
            }
        }
        //foreach (Material m in this.materials) Console.WriteLine(m.ToString());
    }



    public void AddMaterial (Material material) {
        materials.Add(material);
    }
    public void AddNewMaterial (string material) {
        materials.Add(new Material(material));
    }
    public void AddNewMaterial (string material, string texture, string textureExtension) {
        materials.Add(new Material(material, texture, textureExtension));
    }
    public void AddNewMaterial (string material, string texture, string textureExtension, int width, int height) {
        materials.Add(new Material(material, texture, textureExtension, width, height));
    }
    public void AddToExistingMaterials (string searchMaterial, string addTexture, string addExtension) {
        for (int i = 0; i < materials.Count; i++) {
            if (materials[i].name == searchMaterial && materials[i].texture == "") {
                materials[i] = new Material(materials[i].name, addTexture, addExtension, materials[i].width, materials[i].height);
            }
        }
    }
    public void AddToExistingTextures (string searchTexture, int addWidth, int addHeight) {
        for (int i = 0; i < materials.Count; i++) {
            if (materials[i].texture == searchTexture) {
                materials[i] = new Material(materials[i].name, materials[i].texture, materials[i].textureExtension, addWidth, addHeight);
            }
        }
    }
    public bool ContainsMaterial (string name) {
        for (int i = 0; i < materials.Count; i++) if (materials[i].name == name) return true;
        return false;
    }
    public bool ContainsTexture (string texture) {
        for (int i = 0; i < materials.Count; i++) if (materials[i].texture == texture) return true;
        return false;
    }
    public int IndexOfMaterial (string name) {
        for (int i = 0; i < materials.Count; i++) if (materials[i].name == name) return i;
        return -1;
    }
    public int IndexOfTexture (string texture) {
        for (int i = 0; i < materials.Count; i++) if (materials[i].texture == texture) return i;
        return -1;
    }
    public int[] IndicesOfTexture (string texture) {
        List<int> indices = new List<int>();
        for (int i = 0; i < materials.Count; i++) if (materials[i].texture == texture) indices.Add(i);
        return indices.ToArray();
    }
    public Material RetrieveMaterial (int index) {
        return materials[index];
    }
    public Material? FindMaterial (string searchTerm) {                         //<== Change this function to return a pointer?
        int materialIndex = IndexOfMaterial(searchTerm);
        if (materialIndex == -1) materialIndex = IndexOfTexture(searchTerm);
        if (materialIndex != -1) return materials[materialIndex];
        else return null;
    }
}



public struct Material {
    public string name;
    public string texture;
    public string textureExtension;
    public int width;
    public int height;
    public Material (string name) {
        this.name = name;
        this.texture = "";
        this.textureExtension = "";
        this.width = 0;
        this.height = 0;
    }
    public Material (string name, string texture, string textureExtension) {
        this.name = name;
        this.texture = texture;
        this.textureExtension = textureExtension;
        this.width = 0;
        this.height = 0;
    }
    public Material (string name, string texture, string textureExtension, int width, int height) {
        this.name = name;
        this.texture = texture;
        this.textureExtension = textureExtension;
        this.width = width;
        this.height = height;
    }
    public string fullTextureName {get{return texture+"."+textureExtension;}}
    public override string ToString() {
        return this.name+" "+this.texture+" "+this.textureExtension+" "+this.width+" "+this.height;
    }
}