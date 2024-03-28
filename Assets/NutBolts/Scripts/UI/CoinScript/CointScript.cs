using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CointScript : MonoBehaviour
{
    public int Coin;
    private Text textCoin;
    // Start is called before the first frame update
    void Start()
    {
        textCoin = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Coin = UserData.Instance.CGameData.TotalCoin;
        textCoin.text = Coin.ToString("N0");
    }
}
