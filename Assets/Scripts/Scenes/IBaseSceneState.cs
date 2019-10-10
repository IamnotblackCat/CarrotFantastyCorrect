using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所谓的场景状态，其实就是场景类，是类，不是游戏物体，也不是panel
/// </summary>
public interface IBaseSceneState {

    void EnterScene();
    void ExitScene();
}
