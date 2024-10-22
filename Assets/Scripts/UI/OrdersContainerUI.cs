using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class OrdersContainerUI : MonoBehaviour
{
    public OrderUI orderPrefab;
    // Start is called before the first frame update

    private List<OrderUI> orders = new List<OrderUI>();
    private IEnumerator ordersTimeReduction;

    public void AddNewOrder(int totalSeconds, Sprite icon, int tableNum)
    {
        OrderUI order = Instantiate(orderPrefab, transform);
        order.transform.GetChild(0).GetComponent<Slider>().value = totalSeconds;
        order.transform.GetChild(1).GetComponent<Image>().sprite = icon;
        order.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "TABLE "+tableNum;

        order.maxSeconds = totalSeconds;
        orders.Add(order);
    }

    IEnumerator ReduceOrdersTimers()
    {
        while(true)
        {
            for(int i=0; i< orders.Count; i++)
            {
                OrderUI order = orders[i];
                order.transform.GetChild(0).GetComponent<Slider>().value -= 1f/order.maxSeconds;

                if(order.transform.GetChild(0).GetComponent<Slider>().value <= 0)
                {
                    orders.Remove(order);
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
