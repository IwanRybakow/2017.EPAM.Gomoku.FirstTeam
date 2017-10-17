using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2017.EPAM.Gomoku.FirstTeam.Algorithm.Rybakov
{
    public class Pattern
    {
        public int Weight { get; set; }
        public string PatternString
        {
            get
            {
                return PatternStringBuilder.ToString();
            }
        }
        public StringBuilder PatternStringBuilder { get; set; }
        public Pattern(int w)
        {
            Weight = w;
            PatternStringBuilder = new StringBuilder();
        }
    }
}
