using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleField
{
    public interface IPlayable
    {
        byte[] NextMove(byte[,] currentState, byte maxInRow);
        byte Identificator { get; set; }
    }
}
