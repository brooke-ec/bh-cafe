using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class OrdersContainerUI : MonoBehaviour
{
    public OrderUI orderPrefab;
    // Start is called before the first frame update

    private Dictionary<int, OrderUI> orders = new();
    private IEnumerator ordersTimeReduction;

    public void AddNewOrder(int totalSeconds, Sprite icon, int tableNum)
    {
        OrderUI order = Instantiate(orderPrefab, transform);
        order.transform.DOScale(transform.localScale * 1.1f, 0.3f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo);
        order.transform.GetChild(0).GetComponent<Slider>().value = totalSeconds;
        order.transform.GetChild(1).GetComponent<Image>().sprite = icon;
        order.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "TABLE "+tableNum;

        order.maxSeconds = totalSeconds;
        orders.Add(tableNum, order);
    }

    public void RemoveOrder(int tableNum)
    {
        if (!orders.ContainsKey(tableNum)) return;
        Destroy(orders[tableNum].gameObject);
        orders.Remove(tableNum);
    }

    IEnumerator ReduceOrdersTimers()
    {
        while(true)
        {
            foreach (int key in orders.Keys.ToArray())
            {
                OrderUI order = orders[key];
                order.transform.GetChild(0).GetComponent<Slider>().value -= 1f/ order.maxSeconds;

                if(order.transform.GetChild(0).GetComponent<Slider>().value <= 0)
                {
                    orders.Remove(key);
                    Destroy(order.gameObject);
                }
            }

            yield return new WaitForSecondsRealtime(1);
        }
    }

    public void StartLevel()
    {
        ordersTimeReduction = ReduceOrdersTimers();
        StartCoroutine(ordersTimeReduction);
    }

    public void EndLevel()
    {
        StopCoroutine(ordersTimeReduction);
    }
}
