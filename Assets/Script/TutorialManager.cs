using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TMP_Text tutorialText;
    
    public string tutorialText1 = "Click and drag LMB to slice! \n\n";
    public string tutorialText2 = "Hold RMB to use super perception and A/D to move around.\n\n";
    public string tutorialText3 = "Avoid the obstacles to survive! \n\n";

    void Update()
    {
        int currentLevel = LevelManager.instance.currentLevel;
        tutorialText.rectTransform.parent = LevelManager.instance.obstacles[0].transform;
        tutorialText.rectTransform.localPosition = new Vector3(0f, 5f, 0f);

        if(currentLevel == 0){
            tutorialText.text = tutorialText1;
        }
        else if(currentLevel == 1){
            tutorialText.text = tutorialText2;
        }
        else if(currentLevel == 2){
            tutorialText.text = tutorialText3;
        }else{
            tutorialText.gameObject.SetActive(false);
            enabled = false;
        }            
    }
}
