using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto
{
    [Serializable]
    public class DTOSelectRoom
    {
        public DTOSelectModel[] teamOne;
        public DTOSelectModel[] teamTwo;

        public int GetTeamByUserID(string userID)
        {
            foreach(DTOSelectModel item in teamOne)
            {
                if(item.userID==userID)
                {
                    return 1;
                }
            }
            foreach (DTOSelectModel item in teamTwo)
            {
                if (item.userID == userID)
                {
                    return 2;
                }
            }
            return -1;
        }
    }
}
