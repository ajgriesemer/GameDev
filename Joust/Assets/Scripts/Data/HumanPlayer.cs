
using UnityEngine;

public class HumanPlayer
{
    public int DeviceId { get; private set; }
    public int Number { get; set; }
    public bool IsReady { get; set; }
    public GameObject Bee { get; set; }

    public HumanPlayer(int deviceId)
    {
        this.DeviceId = deviceId;
        this.IsReady = false;
    }
}
