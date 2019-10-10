using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPFactory : MonoBehaviour {

	// Use this for initialization
	void Start () {
        IPhone8Factory factory8 = new IPhone8Factory();
        factory8.CreateIPhone();
        factory8.CreateIPCharger();
        IPhoneXFactory xFactory = new IPhoneXFactory();
        xFactory.CreateIPCharger();
        
	}
	
}
public class IPhone
{
    public IPhone()
    {

    }
}
public class IPhone8 : IPhone
{
    public IPhone8()
    {
    }
}
public class IPhoneX : IPhone
{
    public IPhoneX()
    {
    }
}
public interface IPhoneFactory
{//返回值是苹果手机
    IPhone CreateIPhone();
    IPCharger CreateIPCharger();
}
public class IPhone8Factory : IPhoneFactory
{
    public IPCharger CreateIPCharger()
    {
        return new IP8Charger();
    }

    public IPhone CreateIPhone()
    {
        return new IPhone8();
    }
}
public class IPhoneXFactory : IPhoneFactory
{
    public IPCharger CreateIPCharger()
    {
        return new IPXCharger();
    }

    public IPhone CreateIPhone()
    {
        return new IPhoneX();
    }

}
public interface IPCharger
{

}
public class IP8Charger : IPCharger
{
    public IP8Charger()
    {
        Debug.Log("IP8Charger");
    }
}
public class IPXCharger : IPCharger
{
    public IPXCharger()
    {
        Debug.Log("IPXCharger");
    }
}
//public class Bullet : MonoBehaviour
//{
//    private AudioClip audioClip;
//    private AudioSource audioSource;

//    private void Start()
//    {
//        audioSource = GetComponent<AudioSource>();
//        audioClip = Resources.Load<AudioClip>("****");//指定路径加载
//        audioSource.clip = audioClip;
//        Destroy(gameObject,4);
//    }
//}
