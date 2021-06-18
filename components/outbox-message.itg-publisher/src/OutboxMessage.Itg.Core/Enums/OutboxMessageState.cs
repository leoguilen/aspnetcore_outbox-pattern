namespace OutboxMessage.Itg.Core.Enums
{
    public enum OutboxMessageState
    {
        ReadyToSend = 1,
        SendToQueue = 2,
        Completed = 3,
    }
}
