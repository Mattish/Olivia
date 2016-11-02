using System;
using System.Reflection;
using UnityEngine;

namespace Olivia
{
    public class Bootstrap : MonoBehaviour
    {
        private BattleController _battleController;

        public Bootstrap()
        {
            _battleController = DynamicTypeGenerator.GetNewType<BattleController>();
        }

        public void LateUpdate()
        {
            _battleController.CheckBattle();
        }

        private void Start()
        {
            Logger.AppendLine($"Olivia Start. Loaded {GetType().Name} Version:{Assembly.GetExecutingAssembly().GetName().Version}");
            //DataMgr dataMgrIns = GameMgr.GetIns().GetDataMgrIns();
            //var customDeckNum = dataMgrIns.GetCustomDeckNum();
            //Logger.AppendLine($"dataMgrIns.GetCustomDeckNum(): {dataMgrIns.GetCustomDeckNum()}");
            //for (int i = 0; i < customDeckNum; i++)
            //{
            //    var deck = dataMgrIns.GetCustomDeck_UsingOffset(i);
            //    if (deck.GetDeckIsAvailable() && deck.GetDeckIsComplete())
            //    {
            //        Logger.AppendLine($"Found custom deck with name: {deck.GetDeckName()}");
            //    }
            //}
            _battleController.CheckBattle();
        }

        private void OnDestroy()
        {
            Logger.AppendLine($"Olivia end. {GetType().Name} Version:{Assembly.GetExecutingAssembly().GetName().Version}");
            _battleController.Destroy();
        }


    }
}