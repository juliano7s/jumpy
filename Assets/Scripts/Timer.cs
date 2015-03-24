using UnityEngine;
public class Timer : MonoBehaviour
{
	public float currentTime;
	float startTime; 
	float roundedRestSeconds;
	float restSeconds;
	string displaySeconds;
	string displayMinutes;
	
	void Start () {
		startTime = Time.time;
	}
		
	void Update () {
		currentTime = Time.time - startTime;
		displaySeconds = (currentTime % 60).ToString("00");
		displayMinutes = Mathf.Floor(currentTime / 60).ToString("00"); 
		
		string text = displayMinutes + ":" + displaySeconds; 
		GetComponent<GUIText>().text = text;
		GameObject.Find("timer/timerShadow").GetComponent<GUIText>().text = text;
	}
}


