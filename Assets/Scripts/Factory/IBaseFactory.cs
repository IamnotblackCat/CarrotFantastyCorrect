using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseFactory {

    GameObject GetItem(string name);
    void PushItem(string name,GameObject item);
	
}
