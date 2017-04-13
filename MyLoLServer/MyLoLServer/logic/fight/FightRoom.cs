using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using NetFrame.auto;
using System.Collections.Concurrent;
using GameProtocol.dto;
using MyLoLServer.tool;
using GameProtocol.constans.Hero;
using GameProtocol.constans.Skill;
using GameProtocol.constans.Build;
using GameProtocol;

namespace MyLoLServer.logic.fight
{
    class FightRoom : AbsMulitHandlr, InterfaceHandler
    {
        public override byte Type
        {
            get
            {
                return Protocol.TYPE_FIGHT;
            }
        }

        public Dictionary<int, AbsFightModel> teamOne = new Dictionary<int, AbsFightModel>();
        public Dictionary<int, AbsFightModel> teamTwo = new Dictionary<int, AbsFightModel>();

        private ConcurrentIntegerID index;

        private int enterCount;

        public void Init(DTOSelectModel[] _teamOne, DTOSelectModel[] _teamTwo)
        {
            index = new ConcurrentIntegerID();
            enterCount = _teamOne.Length + _teamTwo.Length;
            Console.WriteLine("ALL:"+enterCount);
            //初始化英雄数据
            foreach (DTOSelectModel item in _teamOne)
            {
                DTOFightPlayerModel newItem = Create(item);
                teamOne.Add(newItem.id, newItem);
            }

            foreach (DTOSelectModel item in _teamTwo)
            {
                DTOFightPlayerModel newItem = Create(item);
                teamTwo.Add(newItem.id, newItem);
            }

            //实例化队伍的建筑
            for (int i = 1; i < 4; i++)
            {
                DTOFightBuildModel item1 = CreateBuild(i);
                teamOne.Add(item1.id, item1);
                DTOFightBuildModel item2 = CreateBuild(i);
                teamTwo.Add(item2.id, item2);
            }
        }

        private DTOFightBuildModel CreateBuild(int id)
        {
            BuildData.BuildDataModel data = BuildData.GetBuildDataByID(id);
            DTOFightBuildModel model = new DTOFightBuildModel()
            {
                id = index.GetAndAdd(),
                name = data.name,
                fightUnitType=FightUnitType.BUILD,
                code = data.code,
                hp = data.hp,
                maxHp = data.hp,
                attack = data.atk,
                armor = data.arrorm,
                born = data.reborn,
                bornTime = data.rebornTime,
                initative = data.initiative,
                infrared = data.infrared
            };
            return model;
        }

        private DTOFightPlayerModel Create(DTOSelectModel model)
        {
            DTOFightPlayerModel player = new DTOFightPlayerModel();
            player.id = index.GetAndAdd();
            player.userID = model.userID;
            player.code = model.hero;
            player.name = GetUser(model.userID).name;
            player.fightUnitType = FightUnitType.HERO;
            player.exp = 0;
            player.level = 1;
            player.freeSkillPoint = 1;
            player.money = 0;

            //从配置表里面取出对应的英雄数据
            HeroData.HeroDataModel data = HeroData.FindHeroDataByID(player.code);
            player.hp = data.hpBase;
            player.maxHp = data.hpBase;
            player.mp = data.mpBase;
            player.maxMp = data.mpBase;
            player.attack = data.atkBase;
            player.armor = data.armorBase;
            player.atkSpeed = data.atkSpeedBase;
            player.moveSpeed = data.moveSpeedBase;
            player.atkRange = data.atkRangeBase;
            player.eyeRange = data.eyeRangeBase;
            player.skillList = InitSkill(data.skillsBase);

            return player;
        }

        private DTOFightSkill[] InitSkill(int[] value)
        {
            DTOFightSkill[] skills = new DTOFightSkill[value.Length];

            for (int i = 0; i < value.Length; i++)
            {
                int skillCode = value[i];
                SkillData.SkillDataModel data = SkillData.GetSkillDataByID(skillCode);
                SkillData.SkillLevelData levelData = data.levels[0];
                DTOFightSkill skill = new DTOFightSkill()
                {
                    code = skillCode,
                    level = 0,
                    studyNextLevel = levelData.level,
                    cdTime = levelData.time,
                    name = data.name,
                    range = levelData.range,
                    skillDes = data.info,
                    skillTarget = data.target,
                    skillType = data.type
                };
                skills[i] = skill;
            }

            return skills;
        }

        public void ClientClose(UserToken token, string error)
        {
            Leave(token);
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            switch (message.command)
            {
                case FightProtocol.ENTER_CREQ:
                    {
                        Enter(token);
                        break;
                    }
            }

        }


        private void Enter(UserToken token)
        {
            if (IsEntered(token))
            {
                return;
            }
            EnterList(token);
            enterCount--;
            //所有人准备 发送房间信息
            Console.WriteLine(Area+"/////"+ enterCount);
            if (enterCount == 0)
            {
                DTOFightRoomModel room = new DTOFightRoomModel();
                room.teamOne = teamOne.Values.ToArray();
                room.teamTwo = teamTwo.Values.ToArray();
                Console.WriteLine(list);
                Brocast(FightProtocol.START_BRO, room);
            }
        }

        public void Destroy()
        {
            list.Clear();
            index = null;
            teamOne = null;
            teamTwo = null;
            enterCount = 0;
        }
    }
}
