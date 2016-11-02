using System;
using System.Collections.Generic;
using Wizard;
using Wizard.RoomMatch;

namespace Olivia
{
    public class DataReceiver
    {
        public Action<RealTimeNetworkBattleAgent.NetworkDataURI, bool> OnMatchEnd = (uri, isPlayer) => { };
        public Action<string> OnBid = bid => { };
        private string lastBid = String.Empty;
        public NetworkBattleReceiver.ReceiveData ReceiveData = new NetworkBattleReceiver.ReceiveData();

        private RealTimeNetworkBattleAgent _agent;
        private Action<Dictionary<string, object>> _action;

        public void CheckEventBinding()
        {
            if (!ReferenceEquals(_agent, ToolboxGame.RealTimeNetworkBattle))
            {
                Logger.AppendLine("RealTimeNetworkBattleAgent Changed.");
                if (ToolboxGame.RealTimeNetworkBattle == null)
                {
                    Logger.AppendLine("RealTimeNetworkBattleAgent is null.");
                    _agent = null;
                    return;
                }
                _agent = ToolboxGame.RealTimeNetworkBattle;
            }
            if (ToolboxGame.RealTimeNetworkBattle != null && !ReferenceEquals(_agent.OnReceivedEvent, _action))
            {
                Logger.AppendLine("_agent.OnReceivedEvent Changed.");
                try
                {
                    ToolboxGame.RealTimeNetworkBattle.OnReceivedEvent -= OnReceivedEvent();
                }
                catch (Exception e)
                {
                    //ignored
                }
                ToolboxGame.RealTimeNetworkBattle.OnReceivedEvent += OnReceivedEvent();
                _action = ToolboxGame.RealTimeNetworkBattle.OnReceivedEvent;
            }

            if (ToolboxGame.RealTimeNetworkBattle != null)
            {
                string battleId;
                int maxSeqNum;
                int curSeqNum;
                int turnState;
                ToolboxGame.RealTimeNetworkBattle.GetRecoveryInfo(out battleId, out maxSeqNum, out curSeqNum, out turnState);
                if (lastBid != battleId)
                {
                    lastBid = battleId;
                    OnBid.Invoke(battleId);
                }
            }
        }

        private Action<Dictionary<string, object>> OnReceivedEvent()
        {
            return objects =>
            {
                try
                {
                    PopulateReceiveData(objects);
                    //BattleDataPrinter.Print(ReceiveData);

                }
                catch (Exception e)
                {
                    Logger.AppendLine(e.ToString());
                }
            };
        }

        private void ConvertRecive_FirstCard(KeyValuePair<string, object> data)
        {
            ReceiveData.opponentMulliganAfterCardIndices = new List<int>();
            foreach (object current in (data.Value as List<object>))
            {
                ReceiveData.opponentMulliganAfterCardIndices.Add(Convert.ToInt32(current));
            }
        }

        private void PopulateReceiveData(Dictionary<string, object> synchronizeData)
        {
            var uri = (RealTimeNetworkBattleAgent.NetworkDataURI)(int)Enum.Parse(typeof(RealTimeNetworkBattleAgent.NetworkDataURI), synchronizeData["uri"].ToString());
            ConvertReceiveDataToMakeData(uri, false, synchronizeData);
        }

        private void ConvertReceiveDataToMakeData(RealTimeNetworkBattleAgent.NetworkDataURI uri, bool isHaveSequence,
            Dictionary<string, object> datas, bool isPlayer = false, WatchDataHandler handler = null)
        {
            ReceiveData.dataUri = uri;
            switch (uri)
            {
                case RealTimeNetworkBattleAgent.NetworkDataURI.Resume:
                case RealTimeNetworkBattleAgent.NetworkDataURI.RoomEntry:
                case RealTimeNetworkBattleAgent.NetworkDataURI.InitBattle:
                case RealTimeNetworkBattleAgent.NetworkDataURI.InitRoomBattle:
                case RealTimeNetworkBattleAgent.NetworkDataURI.Matched:
                case RealTimeNetworkBattleAgent.NetworkDataURI.Alive:
                case RealTimeNetworkBattleAgent.NetworkDataURI.Loaded:
                case RealTimeNetworkBattleAgent.NetworkDataURI.FirstCard:
                case RealTimeNetworkBattleAgent.NetworkDataURI.Ready:
                case RealTimeNetworkBattleAgent.NetworkDataURI.TurnStart:
                case RealTimeNetworkBattleAgent.NetworkDataURI.TurnEndActions:
                case RealTimeNetworkBattleAgent.NetworkDataURI.TurnEnd:
                case RealTimeNetworkBattleAgent.NetworkDataURI.TurnEndOppo:
                case RealTimeNetworkBattleAgent.NetworkDataURI.AllowTurnEndOppo:
                case RealTimeNetworkBattleAgent.NetworkDataURI.PlayHand:
                case RealTimeNetworkBattleAgent.NetworkDataURI.PlayActions:
                case RealTimeNetworkBattleAgent.NetworkDataURI.PlayHandActions:
                case RealTimeNetworkBattleAgent.NetworkDataURI.BattleStart:
                case RealTimeNetworkBattleAgent.NetworkDataURI.ChatStamp:
                case RealTimeNetworkBattleAgent.NetworkDataURI.PingStat:
                case RealTimeNetworkBattleAgent.NetworkDataURI.Echo:
                case RealTimeNetworkBattleAgent.NetworkDataURI.Judge:
                case RealTimeNetworkBattleAgent.NetworkDataURI.Touch:
                case RealTimeNetworkBattleAgent.NetworkDataURI.Maintenance:
                    break;
                case RealTimeNetworkBattleAgent.NetworkDataURI.Retire:
                case RealTimeNetworkBattleAgent.NetworkDataURI.SpecialWin:
                case RealTimeNetworkBattleAgent.NetworkDataURI.OppoDisconnect:
                case RealTimeNetworkBattleAgent.NetworkDataURI.BattleFinish:
                case RealTimeNetworkBattleAgent.NetworkDataURI.End:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(uri), uri, null);
            }
            foreach (KeyValuePair<string, object> current in datas)
            {
                string key = current.Key;
                switch (key)
                {
                    case "idx":
                        ReceiveData.idx = Convert.ToInt32(current.Value);
                        break;
                    case "isSelf":
                        ReceiveData.isSelf = (Convert.ToInt32(current.Value) == 1);
                        break;
                    case "type":
                        ReceiveData.actionType = (NetworkBattleReceiver.NetworkBattleDataType)Convert.ToInt32(current.Value);
                        break;
                    case "firstCard":
                        ConvertRecive_FirstCard(current);
                        break;
                    case "playIdx":
                        ReceiveData.playCardIndex = Convert.ToInt32(current.Value);
                        break;
                    case "cards":
                        {
                            List<object> cardData = current.Value as List<object>;

                            ReceiveData.watchCardList = MakeReceiveCardDataList(cardData, NetworkBattleMgr.GetIns(), handler);
                            break;
                        }
                    case "uList":
                        ReceiveData.unapprovedList = MakeReceiveCardDataList(current.Value as List<object>, NetworkBattleMgr.GetIns(), handler);
                        break;
                    case "targetList":
                        {
                            Dictionary<string, object> dictionary = new Dictionary<string, object>();
                            List<object> list = current.Value as List<object>;
                            foreach (object current2 in list)
                            {
                                Dictionary<string, object> dictionary2 = current2 as Dictionary<string, object>;
                                string key2 = RealTimeNetworkBattleAgent.NetworkPram.targetIdx.ToString();
                                if (dictionary2.ContainsKey(key2))
                                {
                                    dictionary.Add(key2, Convert.ToInt32(dictionary2[key2]));
                                }
                                key2 = RealTimeNetworkBattleAgent.MoveListParam.vid.ToString();
                                if (dictionary2.ContainsKey(key2))
                                {
                                    int num2 = !handler.isOwner(dictionary2[key2].ToString()) ? 1 : 0;
                                    dictionary.Add(RealTimeNetworkBattleAgent.NetworkPram.isSelf.ToString(), num2);
                                }
                            }
                            if (isPlayer)
                            {
                                ReceiveData.playActions = dictionary;
                            }
                            else
                            {
                                ReceiveData.oppoActions = dictionary;
                            }
                            break;
                        }
                    case "oppoTargetList":
                        {
                            Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
                            List<object> list2 = current.Value as List<object>;
                            foreach (object current3 in list2)
                            {
                                Dictionary<string, object> dictionary4 = current3 as Dictionary<string, object>;
                                string key3 = RealTimeNetworkBattleAgent.NetworkPram.targetIdx.ToString();
                                if (dictionary4.ContainsKey(key3))
                                {
                                    dictionary3.Add(key3, Convert.ToInt32(dictionary4[key3]));
                                }
                                key3 = RealTimeNetworkBattleAgent.NetworkPram.isSelf.ToString();
                                if (dictionary4.ContainsKey(key3))
                                {
                                    dictionary3.Add(key3, Convert.ToInt32(dictionary4[key3]));
                                }
                            }
                            ReceiveData.oppoActions = dictionary3;
                            break;
                        }
                    case "battleFinish":
                        ReceiveData.oppoActions = (current.Value as Dictionary<string, object>);
                        break;
                    case "chatStamp":
                        {
                            Dictionary<string, object> dictionary5 = current.Value as Dictionary<string, object>;
                            int num3 = Convert.ToInt32(dictionary5["stamp"]);
                            if (isPlayer)
                            {
                                ReceiveData.playChatStamp = num3;
                            }
                            else
                            {
                                ReceiveData.oppoChatStamp = num3;
                            }
                            break;
                        }
                    case "isWin":
                        ReceiveData.isWin = (Convert.ToInt32(current.Value) != 0);
                        OnMatchEnd.Invoke(ReceiveData.dataUri, ReceiveData.isWin);
                        break;
                    case "knownList":
                        ReceiveData.knownCardList = MakeReceiveCardDataList(current.Value as List<object>, NetworkBattleMgr.GetIns(), handler);
                        break;
                    case "bid":
                        OnBid.Invoke(current.Value.ToString());
                        break;
                }
            }
        }

        private List<CardDataModel> MakeReceiveCardDataList(List<object> cardData, BattleMgrBase mgr, WatchDataHandler handler)
        {
            List<CardDataModel> list = new List<CardDataModel>();
            foreach (object current in cardData)
            {
                list.Add(this.MakeReceiveCardData(current, mgr, handler));
            }
            return list;
        }

        private CardDataModel MakeReceiveCardData(object cardData, BattleMgrBase mgr, WatchDataHandler handler)
        {
            Dictionary<string, object> dictionary = cardData as Dictionary<string, object>;
            CardDataModel cardDataModel = new CardDataModel();
            if (dictionary.ContainsKey(RealTimeNetworkBattleAgent.MoveListParam.idx.ToString()))
            {
                cardDataModel.Index = Convert.ToInt32(dictionary[RealTimeNetworkBattleAgent.MoveListParam.idx.ToString()]);
            }
            else
            {
                Debug.LogError("BaseMakeReceiveCardData Not idx ", null);
            }
            if (dictionary.ContainsKey(RealTimeNetworkBattleAgent.MoveListParam.card_id.ToString()))
            {
                cardDataModel.CardId = Convert.ToInt32(dictionary[RealTimeNetworkBattleAgent.MoveListParam.card_id.ToString()]);
            }
            if (dictionary.ContainsKey(RealTimeNetworkBattleAgent.MoveListParam.to.ToString()))
            {
                cardDataModel.toState = (RealTimeNetworkBattleAgent.NetworkCardPlaceState)Convert.ToInt32(dictionary[RealTimeNetworkBattleAgent.MoveListParam.to.ToString()]);
            }
            if (dictionary.ContainsKey(RealTimeNetworkBattleAgent.MoveListParam.isSelf.ToString()))
            {
                cardDataModel.isSelf = (Convert.ToInt32(dictionary[RealTimeNetworkBattleAgent.MoveListParam.isSelf.ToString()]) != 1);
            }
            if (dictionary.ContainsKey(RealTimeNetworkBattleAgent.MoveListParam.skillIdx.ToString()))
            {
                cardDataModel.skillIndex = Convert.ToInt32(dictionary[RealTimeNetworkBattleAgent.MoveListParam.skillIdx.ToString()]);
            }
            if (dictionary.ContainsKey(RealTimeNetworkBattleAgent.MoveListParam.cost.ToString()))
            {
                cardDataModel.playCardCost = Convert.ToInt32(dictionary[RealTimeNetworkBattleAgent.MoveListParam.cost.ToString()]);
            }
            if (mgr != null)
            {
                cardDataModel.fromState = NetworkBattleTool.GetCardPlaceState(mgr.GetBattlePlayer(cardDataModel.isSelf), cardDataModel.Index);
            }
            return cardDataModel;
        }

        public void Destroy()
        {
            ToolboxGame.RealTimeNetworkBattle.OnReceivedEvent -= OnReceivedEvent();
        }
    }
}