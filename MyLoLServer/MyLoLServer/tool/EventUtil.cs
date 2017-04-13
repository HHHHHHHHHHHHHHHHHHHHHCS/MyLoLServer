using GameProtocol.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 创建选人模块事件
/// </summary>
/// <param name="teamOne"></param>
/// <param name="teanTwo"></param>
public delegate void CreateSelect(List<string> teamOne, List<string> teanTwo);

/// <summary>
/// 移除选人模块事件
/// </summary>
/// <param name="roomID"></param>
public delegate void DestroySelect(int roomID);

/// <summary>
/// 创建战斗模块事件
/// </summary>
/// <param name="teamOne"></param>
/// <param name="teamTwo"></param>
public delegate void CreateFight(DTOSelectModel[] teamOne, DTOSelectModel[] teamTwo);

/// <summary>
/// 战斗结束销毁房间
/// </summary>
/// <param name="roomID"></param>
public delegate void DestroyFight(int roomID);
namespace MyLoLServer.tool
{
    public class EventUtil
    {
        public static CreateSelect createSelect;
        public static DestroySelect destroySelect;

        public static CreateFight createFight;
        public static DestroyFight destroyFight;
    }
}
