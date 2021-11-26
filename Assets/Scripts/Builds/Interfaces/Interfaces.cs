using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConnectedBuild
{
    void SetConnections();
    void ConnectModel(bool right, bool left, bool forward, bool backward);
    void InactiveModels();
}
