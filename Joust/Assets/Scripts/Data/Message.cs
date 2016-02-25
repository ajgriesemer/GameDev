
public class Message<T> : Message
{
    public T Data { get; private set; }

    public Message(string type, T data)
        : base(type)
    {
        this.Data = data;
    }
}

public class Message
{
    public string Type { get; private set; }

    public Message(string type)
    {
        this.Type = type;
    }
}
