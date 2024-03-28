using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour
{
    public Image imgIcon;
    public Text textNumber;
    private RewardObj rewardObj;
    public void InitData(RewardObj reward)
    {
        this.rewardObj = reward;
        imgIcon.sprite = Resources.Load<Sprite>("Sprite/" + reward.rewardType.ToString());
        textNumber.text = reward.amount.ToString();
    }
}
