using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryManager  {

    public Dictionary<FactoryType, IBaseFactory> factoryDict = new Dictionary<FactoryType, IBaseFactory>();
    public AudioClipFactory audioClipFactory;
    public SpriteFactory spriteFactory;
    public RuntimeAnimatorControllerFactory runtimeAnimatorControllerFactory;

    public FactoryManager()
    {
        factoryDict.Add(FactoryType.UIFactory,new UIFactory());
        factoryDict.Add(FactoryType.UIPanelFactory,new UIPanelFactory());
        factoryDict.Add(FactoryType.GameFactory,new GameFactory());
        audioClipFactory = new AudioClipFactory();
        spriteFactory = new SpriteFactory();
        runtimeAnimatorControllerFactory = new RuntimeAnimatorControllerFactory();
    }
}
