using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : InteractObject
{
    public enum BuildState
    {
        blueprint,
        built
    }

    public BuildState State = BuildState.blueprint;
}
