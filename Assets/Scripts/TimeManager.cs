using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public void TogglePause(bool on)
    {
        Time.timeScale = on ? 0f : 1f;
    }
}