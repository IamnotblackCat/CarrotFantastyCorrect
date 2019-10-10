using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPState : MonoBehaviour {

    //public enum PersonState
    //{
    //    Eat,
    //    Work,
    //    Sleep
    //}
    //public PersonState personState;
	// Use this for initialization
	void Start () {
        //personState = PersonState.Work;
        //if (personState==PersonState.Work)
        //{
        //    Debug.Log("正在搬砖");
        //}
        //else if (personState==PersonState.Eat)
        //{
        //    Debug.Log("正在觅食");
        //}
        //else
        //{
        //    Debug.Log("正在被封印");
        //}
        Context context = new Context();
        context.SetState(new Work(context));
        context.Handle();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
public interface IState
{
    void Handle();
}
public class Context
{
    private IState mState;

    public void SetState(IState state)
    {
        mState = state;
    }
    public void Handle()
    {
        mState.Handle();
    }
}
public abstract class BaseState : IState
{
    public Context mContext;

    public BaseState(Context context)//这个状态属于谁，参数是状态机
    {
        mContext = context;
    }
    public virtual void Handle()
    {
        Debug.Log("正在觅食");
    }
}
public class Eat : BaseState
{
    public Eat(Context context) : base(context)
    {
        mContext = context;
    }
    public override void Handle()
    {
        base.Handle();
    }
}
public class Work : BaseState
{
    public Work(Context context) : base(context)
    {
        mContext = context;
    }
    public override void Handle()
    {
        Debug.Log("正在搬砖");
    }
}
public class Sleep : BaseState
{
    public Sleep(Context context) : base(context)
    {
        mContext = context;
    }
    public override void Handle()
    {
        Debug.Log("正在被封印"); 
    }
}
