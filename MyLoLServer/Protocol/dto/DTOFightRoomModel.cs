using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto
{
    [Serializable]
    public class DTOFightRoomModel
    {
        public AbsFightModel[] teamOne;
        public AbsFightModel[] teamTwo;
    }
}
