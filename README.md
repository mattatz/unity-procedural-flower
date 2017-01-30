unity-procedural-flower
=====================

Procedural flower generator for Unity.

![Demo](https://raw.githubusercontent.com/mattatz/unity-procedural-flower/master/Captures/Demo.png)

## Example

### PFTester in Demo.scene

![Petals](https://raw.githubusercontent.com/mattatz/unity-procedural-flower/master/Captures/Petals.gif)

![Height](https://raw.githubusercontent.com/mattatz/unity-procedural-flower/master/Captures/Height.gif)

![Leaves](https://raw.githubusercontent.com/mattatz/unity-procedural-flower/master/Captures/Leaves.gif)

### Garden.scene

ProceduralFlower can animate.

![Garden](https://raw.githubusercontent.com/mattatz/unity-procedural-flower/master/Captures/Garden.gif)

## Usage

### PFShape

ProceduralFlower needs 3 PFShape(ScriptableObject) for petal and leaf shapes.

PFShape has control points and you can design its outline by the editor.

![Editor](https://raw.githubusercontent.com/mattatz/unity-procedural-flower/master/Captures/Editor.gif)

### PFCombine

PFCombine enables to combine meshes of ProceduralFlower into one mesh.

```cs

public ProceduralFlower flower;
void Start () {
    var mesh = PFCombine.Combine(flower);
    GetComponent<MeshFilter>().sharedMesh = mesh;
}

```

## Sources

- The Algorithmic Beauty of Plants - http://algorithmicbotany.org/papers/#abop
