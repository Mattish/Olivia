using System;
using Wizard.Battle.View.Vfx;

namespace Olivia
{
    public class BattleManagerWatcher
    {
        private BattlePlayerBase _playerBase;
        private BattlePlayerBase _oppoBase;

        public Action OnWin = () => { };
        public Action OnLose = () => { };
        public Action OnStart = () => { };

        public void Check()
        {
            try
            {
                var battleMgrBase = BattleMgrBase.GetIns();

                if (battleMgrBase != null)
                {
                    var player = battleMgrBase.GetBattlePlayer(true);
                    if (!ReferenceEquals(_playerBase, player))
                    {
                        if (player != null)
                        {
                            player.Emotion.OnPlay += ProcessPlayer;
                        }
                        _playerBase = player;
                    }
                    var oppo = battleMgrBase.GetBattlePlayer(false);
                    if (!ReferenceEquals(_oppoBase, oppo))
                    {
                        if (oppo != null)
                        {
                            oppo.Emotion.OnPlay += ProcessOppo;
                        }
                        _oppoBase = oppo;
                    }
                }
            }
            catch (Exception e)
            {
                //ignore
            }
        }

        private VfxBase ProcessPlayer(ClassCharaPrm.EmotionType emotionType)
        {
            Logger.AppendLine($"Player emoted {emotionType}");
            switch (emotionType)
            {
                case ClassCharaPrm.EmotionType.BATTLESTART_DIFF:
                case ClassCharaPrm.EmotionType.BATTLESTART_SAME:
                    OnStart.Invoke();
                    break;
                case ClassCharaPrm.EmotionType.WIN:
                    OnWin.Invoke();
                    break;
                case ClassCharaPrm.EmotionType.LOSE:
                case ClassCharaPrm.EmotionType.SURRENDER_LOSE:
                    OnLose.Invoke();
                    break;
            }
            return null;
        }

        private VfxBase ProcessOppo(ClassCharaPrm.EmotionType emotionType)
        {
            Logger.AppendLine($"Oppo emoted {emotionType}");
            switch (emotionType)
            {
                case ClassCharaPrm.EmotionType.WIN:
                    OnLose.Invoke();
                    break;
                case ClassCharaPrm.EmotionType.LOSE:
                case ClassCharaPrm.EmotionType.SURRENDER_LOSE:
                    OnWin.Invoke();
                    break;
            }
            return null;
        }

        public void Destroy()
        {
            var battleMgrBase = BattleMgrBase.GetIns();

            if (battleMgrBase != null)
            {
                var player = battleMgrBase.GetBattlePlayer(true);
                if (player != null)
                {
                    player.Emotion.OnPlay -= ProcessPlayer;
                }

                var oppo = battleMgrBase.GetBattlePlayer(false);

                if (oppo != null)
                {
                    oppo.Emotion.OnPlay -= ProcessOppo;
                }

            }
        }
    }
}