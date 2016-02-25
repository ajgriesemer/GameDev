
public class HumanPlayer
{
    public int DeviceId { get; private set; }
    public bool IsReady { get; set; }

    public HumanPlayer(int deviceId)
    {
        this.DeviceId = deviceId;
        this.IsReady = false;
    }
}
