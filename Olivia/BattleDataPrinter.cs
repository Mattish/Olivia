namespace Olivia
{
    public static class BattleDataPrinter
    {
        public static void Print(NetworkBattleReceiver.ReceiveData data)
        {
            Logger.AppendLine($"{data.dataUri}");
            if (data.playActions != null && data.playActions.Count > 0)
            {
                Logger.AppendLine("playerActions:~");
                foreach (var playAction in data.playActions)
                {
                    Logger.AppendLine(playAction.ToString());
                }
            }
            if (data.oppoActions != null && data.oppoActions.Count > 0)
            {
                Logger.AppendLine("OppoActions:~");
                foreach (var oppoAction in data.oppoActions)
                {
                    Logger.AppendLine(oppoAction.ToString());
                }
            }
        }
    }
}