using System;

namespace Olivia
{
    public class NetworkBattleManagerController
    {
        public Action OnLose = () => { };
        public Action OnWin = () => { };

        private NetworkBattleManagerBase _networkBase;
        public void Check()
        {
            var battleMgrBase = BattleMgrBase.GetIns();
            if (!ReferenceEquals(battleMgrBase, _networkBase))
            {
                Logger.AppendLine("NetworkBattleManagerController is Diff");
                if (battleMgrBase == null)
                {
                    Logger.AppendLine("battleMgrBase is null");
                    _networkBase = null;
                    return;
                }
                var networkBattleManagerBase = battleMgrBase as NetworkBattleManagerBase;
                if (networkBattleManagerBase == null)
                {
                    Logger.AppendLine("networkBattleManagerBase is null");
                    _networkBase = null;
                    return;
                }
                _networkBase = (NetworkBattleManagerBase) battleMgrBase;
                _networkBase.disconnectToLoseChecker.OnDisconnectLose += () =>
                {
                    Logger.AppendLine("disconnectToLoseChecker.OnDisconnectLose");
                    OnLose.Invoke();
                };
                _networkBase.battleFinishToOpponentDisConnectChecker.OnDisConnectWin += () =>
                {
                    Logger.AppendLine("battleFinishToOpponentDisConnectChecker.OnDisConnectWin");
                    OnWin.Invoke();
                };
                _networkBase.notTurnEndToLoseChecker.OnNotTurnEndToLose += () =>
                {
                    Logger.AppendLine("notTurnEndToLoseChecker.OnNotTurnEndToLose");
                    OnLose.Invoke();
                };
                _networkBase.opponentNotTurnEndToWinChecker.OnOpponentNotTurnEndToWin += () =>
                {
                    Logger.AppendLine("opponentNotTurnEndToWinChecker.OnOpponentNotTurnEndToWin");
                    OnWin.Invoke();
                };
                _networkBase.opponentNotTurnStartToWinChecker.OnOpponentNotTurnStartToWin += () =>
                {
                    Logger.AppendLine("opponentNotTurnStartToWinChecker.OnOpponentNotTurnStartToWin");
                    OnWin.Invoke();
                };
                _networkBase.opponentDisconnectToWinChecker.OnOpponentDisconnectToWin += () =>
                {
                    Logger.AppendLine("opponentDisconnectToWinChecker.OnOpponentDisconnectToWin");
                    OnWin.Invoke();
                };
            }
        }
    }
}