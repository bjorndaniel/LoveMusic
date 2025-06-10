namespace LoveMusic;

public class MessageEventArgs : EventArgs
{
    public List<string> Messages { get; set; } = new List<string>();
    public UIUpdateType Type { get; set; } = UIUpdateType.Processing;
}