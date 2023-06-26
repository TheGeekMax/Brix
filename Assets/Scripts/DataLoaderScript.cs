using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DataLoaderScript : MonoBehaviour
{
    int width = 5;
    int steps = 15;
    bool executed = false;

    public float time = -1f;
    public string lvl = "";

    //les temps a prendre en compte
    public float easyTime = 0f;
    public float mediumTime = 0f;
    public float hardTime = 0f;
    public float harderTime = 0f;
    public float impossibleTime = 0f;

    [Header("UI")]
    public TextMeshProUGUI titleName;
    public TextMeshProUGUI titleTime;

    public static DataLoaderScript instance;
    // Start is called before the first frame update
    void Awake(){
        if(instance == null){
            instance = this;
            //on load les données
            easyTime = PlayerPrefs.GetFloat("TIME_EASY",-1f);
            mediumTime = PlayerPrefs.GetFloat("TIME_MEDIUM",-1f);
            hardTime = PlayerPrefs.GetFloat("TIME_HARD",-1f);
            harderTime = PlayerPrefs.GetFloat("TIME_HARDER",-1f);
            impossibleTime = PlayerPrefs.GetFloat("TIME_IMPOSSIBLE",-1f);
        }else{
            //on save les données et on les transmets a cet objets
            if(instance.time != -1f){

                Debug.Log(instance.lvl);
                //un temps a été record, on peut le save
                switch(instance.lvl){
                    case "easy":
                        if(instance.easyTime == -1f|| instance.time < instance.easyTime){
                            PlayerPrefs.SetFloat("TIME_EASY",instance.time);
                            instance.easyTime = instance.time;
                        }
                        break;

                    case "medium":
                        if(instance.mediumTime == -1f|| instance.time < instance.mediumTime){
                            PlayerPrefs.SetFloat("TIME_MEDIUM",instance.time);
                            instance.mediumTime = instance.time;
                        }
                        break;

                    case "hard":
                        if(instance.hardTime == -1f|| instance.time < instance.hardTime){
                            PlayerPrefs.SetFloat("TIME_HARD",instance.time);
                            instance.hardTime = instance.time;
                        }
                        break;

                    case "harder":
                        if(instance.harderTime == -1f|| instance.time < instance.harderTime){
                            PlayerPrefs.SetFloat("TIME_HARDER",instance.time);
                            instance.harderTime = instance.time;
                        }
                        break;

                    case "impossible":
                        if(instance.impossibleTime == -1f|| instance.time < instance.impossibleTime){
                            PlayerPrefs.SetFloat("TIME_IMPOSSIBLE",instance.time);
                            instance.impossibleTime = instance.time;
                        }
                        break;

                        
                }
            }

            //on transmet les données
            easyTime = instance.easyTime;
            mediumTime = instance.mediumTime;
            hardTime = instance.hardTime;
            harderTime = instance.harderTime;
            impossibleTime = instance.impossibleTime;

            Destroy(instance.gameObject);
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start(){
        Easy();
    }

    void Update(){
        if(executed){return;}
        //on test si GameManager existe
        if(GameManager.instance != null){
            executed = true;
            if(lvl !="endless"){
                GameManager.instance.generate(width,steps);
            }
        }
    }

    public void startWithData(int wi, int st,float time,string level){
        width = wi;
        steps = st;
        
        titleName.text = level;
        if(level == "endless"){
            titleTime.text = "∞";
        }else if(time == -1f){
            titleTime.text = "--:--:--";
        }else {
            titleTime.text = TimeManager.formatTime(time);
        }
    }

    public void RunLevel(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void Easy(){
        lvl = "easy";
        startWithData(5,5,easyTime,"easy");
    }

    public void Medium(){
        lvl = "medium";
        startWithData(5,10,mediumTime,"medium");
    }

    public void Hard(){
        lvl = "hard";
        startWithData(5,15,hardTime,"hard");
    }

    public void Harder(){
        lvl = "harder";
        startWithData(10,40,harderTime,"harder");
    }

    public void Impossible(){
        lvl = "impossible";
        startWithData(10,70,impossibleTime,"impossible");
    }
    public void Infinite(){
        lvl = "endless";
        startWithData(10,100,-1f,"endless");
    }
}
