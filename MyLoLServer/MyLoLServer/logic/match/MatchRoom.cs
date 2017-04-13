using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLoLServer.logic.match
{
    /// <summary>
    /// 战斗匹配房间模型
    /// </summary>
    public class MatchRoom
    {
        public string id;//房间唯一ID
        public byte teamMax = 1;//每支队伍需要匹配到的人数
        public List<string> teamOne = new List<string>();//队伍一 人员ID
        public List<string> teamTwo = new List<string>();//队伍二 人员ID

        public MatchRoom(string roomID)
        {
            id = roomID;
        }
    }
}
