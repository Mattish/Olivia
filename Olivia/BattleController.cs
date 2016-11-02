namespace Olivia
{
    public class BattleController
    {
        private readonly BattleManagerWatcher _battleManagerWatcher;
        private readonly DataReceiver _dataReceiver;
        private DataMgr.BattleType _battleType = DataMgr.BattleType.NONE;

        public BattleController()
        {
            _dataReceiver = DynamicTypeGenerator.GetNewType<DataReceiver>();
            _dataReceiver.OnBid += bid =>
            {
                Logger.AppendLine($"Got Bid:{bid}");
                var networkUserInfoData = GameMgr.GetIns().GetNetworkUserInfoData();
                Logger.AppendLine($"OppoId:{networkUserInfoData.GetOpponentUserID()} - OppoName:{networkUserInfoData.GetOpponentName()}");
            };
            _dataReceiver.CheckEventBinding();

            _battleManagerWatcher = DynamicTypeGenerator.GetNewType<BattleManagerWatcher>();
            _battleManagerWatcher.OnWin += () =>
            {
                Logger.AppendLine($"Got Win:{GameMgr.DataMgrIns().m_BattleType}");
            };

            _battleManagerWatcher.OnLose += () =>
            {
                Logger.AppendLine($"Got Lose:{GameMgr.DataMgrIns().m_BattleType}");
            };

            _battleManagerWatcher.OnStart += () =>
            {
                Logger.AppendLine($"Got Start:{GameMgr.DataMgrIns().m_BattleType}");
            };
        }

        public void CheckBattle()
        {
            _battleManagerWatcher.Check();
            _dataReceiver.CheckEventBinding();
            var battleType = GameMgr.DataMgrIns()?.m_BattleType;
            
            if (battleType != null)
            {
                if (_battleType != battleType)
                {
                    _battleType = battleType.Value;
                    Logger.AppendLine($"BattleType is now:{battleType}");
                }
            }
        }

        public void Destroy()
        {
            _battleManagerWatcher.Destroy();
            _dataReceiver.Destroy();
        }
    }
}