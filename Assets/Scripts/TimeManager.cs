using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    float time;
    public TextMeshProUGUI timeShow;

    public static TimeManager instance;
    // Start is called before the first frame update
    void Awake(){
        time = 0f;

        instance = this;
    }

    // Update is called once per frame
    void Update(){
        if(!WinManager.instance.winned() && DataLoaderScript.instance.lvl != "endless"){
            time += Time.deltaTime;
            timeShow.text = getFormattedStr();
        }else if(DataLoaderScript.instance.lvl == "endless"){
            //on montre le level en cours
            timeShow.text = "Level "+(GameManager.instance.level+1);
        }
    }

    public float getTime(){
        return time;
    }

    public static string formatTime(float timeCode){
        if(timeCode == -1f){
            return "00:00:00";
        }
        //on get les minutes, secondes et centiemes :
        int minutes = ((int)timeCode)/60;
        int sec = ((int)timeCode)%60;
        int mill = ((int)(timeCode*100))%100;

        //formatage sous forme MM:SS:CC
        string formatedData = "";
        if(minutes < 10){
            formatedData += "0"+minutes+":";
        }else{
            formatedData += ""+minutes+":";
        }

        if(sec < 10){
            formatedData += "0"+sec+":";
        }else{
            formatedData += ""+sec+":";
        }

        if(mill < 10){
            formatedData += "0"+mill;
        }else{
            formatedData += ""+mill;
        }
        return formatedData;
    }

    public string getFormattedStr(){
        return TimeManager.formatTime(time);
    }
}
