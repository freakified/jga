using UnityEngine;
using System.Collections;

public class ImmersiveModeEnabler : MonoBehaviour {

	AndroidJavaObject unityActivity;
	AndroidJavaObject javaObj;
	AndroidJavaClass javaClass;

	void Awake()
	{
		if(!Application.isEditor)
			HideNavigationBar();
	}
	
	void HideNavigationBar()
	{
		#if UNITY_ANDROID
		lock(this)
		{
			using(javaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				unityActivity = javaClass.GetStatic<AndroidJavaObject>("currentActivity");
			}
			
			if(unityActivity == null)
			{
				return;
			}
			
			using(javaClass = new AndroidJavaClass("com.rak24.androidimmersivemode.Main"))
			{
				if(javaClass == null)
				{
					return;
				}
				else
				{
					javaObj = javaClass.CallStatic<AndroidJavaObject>("instance");
					if(javaObj == null)
						return;
					unityActivity.Call("runOnUiThread",new AndroidJavaRunnable(() => 
					                                                           {
						javaObj.Call("EnableImmersiveMode", unityActivity);
					}));
				}
			}
		}
		#endif
	}
}
