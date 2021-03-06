﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {
    public float genarateItemTime;//過幾秒產生
    public int generateNum;//一次產生幾個
    public int initialNum;//一開始要幾個
    [System.Serializable]
    public struct ItemList
    {
        public GameObject item;
        public int chance;       
    }
    public ItemList[] _items;
    float generatetimer;
    public List<GameObject> ItemsList = new List<GameObject>();//Item 



    // Use this for initialization
    void Start () {
        //開始先產生n個 統一交給gameManager控制
        generatetimer = 0;
        InvokeRepeating("generateTimer", 0, Time.deltaTime);
    }
	
	// Update is called once per frame
	void Update () {
       
       
	}
    public  void gameOver()
    {
        CancelInvoke("generateTimer");
    }
    void generateTimer()
    {
        generatetimer += Time.deltaTime;
        if (generatetimer > genarateItemTime) { //每隔幾秒隨機產生
            generatetimer = 0;
            randomGenerate(generateNum);
        }
        
    }
    public void randomGenerate(int n)
    {

        int mother = 0;
        foreach (ItemList _item in _items)
        {
            mother += _item.chance;
        }

        for (int i = 0; i < n; i++)
        {
            int son = Random.Range(1, mother + 1);
            int index = 999;
            int tmp;
            if (son <= _items[0].chance) index = 0;
            else
            {
                tmp = _items[0].chance;
                for (int j = 1; j <= _items.Length; j++)
                {
                    if (son > tmp && son <= tmp + _items[j].chance)
                    {
                        index = j;
                        break;
                    }
                    else
                    {
                        tmp += _items[j].chance;
                    }
                }
            }
            
            GameObject[] _planes = GameObject.FindGameObjectsWithTag("Ground");


            int _random = Random.Range(0, _planes.Length - 1);

            if (ItemsList.Count == 0)
            {
                for (int g = 0; g < _planes.Length; g++)
                {
                    ItemsList.Add(null);
                }
            }

            if (ItemsList[_random] != null)
            {
                break;
            }

            foreach (GameObject _plane in _planes)
            {
                if (!_planes[_random].GetComponent<PlaneData>().hasItem)
                {
                    Vector3 pos = _planes[_random].transform.position + new Vector3(0, _items[index].item.GetComponent<ItemData>().yOffsst, 0);//set item y offset
                    Vector3 _s = _planes[_random].transform.localScale;
                    _items[index].item.transform.localScale = new Vector3(1 / _s.x, 1 / _s.y, 1 / _s.z); //set scale to world == 1

                    GameObject go = Instantiate(_items[index].item, pos, transform.rotation, _planes[_random].transform);
                    ItemsList[_random] = go;
                    _planes[_random].GetComponent<PlaneData>().hasItem = true;
                    break;
                }
            }
        }
    }
    public void generateSpecial(int _type, int num)//產生第幾種物品
    {
        for (int i = 0; i < num; i++)
        {          
            //TODO:產生位置
            Instantiate(_items[_type].item, transform.position, transform.rotation);
        }

    }
}
