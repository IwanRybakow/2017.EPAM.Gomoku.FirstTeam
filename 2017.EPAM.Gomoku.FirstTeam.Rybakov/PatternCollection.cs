using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2017.EPAM.Gomoku.FirstTeam.Algorithm.Rybakov
{
    public class PatternCollection
    {
        public List<Pattern> PatternList;
        public PatternCollection(int signsInRowToWin, int ownSign)
        {
            int OppSign = ownSign == 1 ? 2 : 1;
            PatternList = new List<Pattern>();

            // XXXXX
            Pattern victory = new Pattern(1700000000);
            victory.PatternStringBuilder.Append(ownSign.ToString()[0], signsInRowToWin);
            PatternList.Add(victory);

            // XXXXX
            Pattern oppVictory = new Pattern(1600000000);
            oppVictory.PatternStringBuilder.Append(OppSign.ToString()[0], signsInRowToWin);
            PatternList.Add(oppVictory);

            // 0XXXX0
            Pattern oneAwayDouble = new Pattern(100000000);
            oneAwayDouble.PatternStringBuilder.Append('0');
            oneAwayDouble.PatternStringBuilder.Append(ownSign.ToString()[0], signsInRowToWin-1);
            oneAwayDouble.PatternStringBuilder.Append('0');
            PatternList.Add(oneAwayDouble);

            // 0XXXX0
            Pattern oppOneAwayDouble = new Pattern(20000000);
            oppOneAwayDouble.PatternStringBuilder.Append('0');
            oppOneAwayDouble.PatternStringBuilder.Append(OppSign.ToString()[0], signsInRowToWin - 1);
            oppOneAwayDouble.PatternStringBuilder.Append('0');
            PatternList.Add(oppOneAwayDouble);

            // 0XXX0
            Pattern twoAwayDouble = new Pattern(2400000);
            twoAwayDouble.PatternStringBuilder.Append('0');
            twoAwayDouble.PatternStringBuilder.Append(ownSign.ToString()[0], signsInRowToWin - 2);
            twoAwayDouble.PatternStringBuilder.Append('0');
            PatternList.Add(twoAwayDouble);

            // 0XXX0
            Pattern oppTwoAwayDouble = new Pattern(2200000);
            oppTwoAwayDouble.PatternStringBuilder.Append('0');
            oppTwoAwayDouble.PatternStringBuilder.Append(OppSign.ToString()[0], signsInRowToWin - 2);
            oppTwoAwayDouble.PatternStringBuilder.Append('0');
            PatternList.Add(oppTwoAwayDouble);

            // 0XXXX, XXXX0, X0XXX..etc
            for (int i = 0; i < signsInRowToWin; i++)
            {
                Pattern oneAway = new Pattern(4000000);
                Pattern oppOneAway = new Pattern(3500000);
                for (int j = 0; j < signsInRowToWin; j++)
                {
                    char c = i == j ? '0' : ownSign.ToString()[0];
                    char oppC = i == j ? '0' : OppSign.ToString()[0];
                    oneAway.PatternStringBuilder.Append(c);
                    oppOneAway.PatternStringBuilder.Append(oppC);
                }
                PatternList.Add(oneAway);
                PatternList.Add(oppOneAway);
            }

            // 00XXX, XXX00, X00XX..etc
            for (int i = 0; i < signsInRowToWin-1; i++)
            {
                Pattern oneAway = new Pattern(1500000);
                Pattern oppOneAway = new Pattern(1300000);
                for (int j = 0; j < signsInRowToWin-1; j++)
                {
                    string c = i == j ? "00" : ownSign.ToString();
                    string oppC = i == j ? "00" : OppSign.ToString();
                    oneAway.PatternStringBuilder.Append(c);
                    oppOneAway.PatternStringBuilder.Append(oppC);
                }
                PatternList.Add(oneAway);
                PatternList.Add(oppOneAway);
            }


            if (signsInRowToWin>3)
            {
                // 00XX0
                Pattern threeAwayTwoLeftOneRight = new Pattern(40000);
                threeAwayTwoLeftOneRight.PatternStringBuilder.Append('0');
                threeAwayTwoLeftOneRight.PatternStringBuilder.Append('0');
                threeAwayTwoLeftOneRight.PatternStringBuilder.Append(ownSign.ToString()[0], signsInRowToWin - 3);
                threeAwayTwoLeftOneRight.PatternStringBuilder.Append('0');
                PatternList.Add(threeAwayTwoLeftOneRight);

                // 00XX0
                Pattern oppTthreeAwayTwoLeftOneRight = new Pattern(30000);
                oppTthreeAwayTwoLeftOneRight.PatternStringBuilder.Append('0');
                oppTthreeAwayTwoLeftOneRight.PatternStringBuilder.Append('0');
                oppTthreeAwayTwoLeftOneRight.PatternStringBuilder.Append(OppSign.ToString()[0], signsInRowToWin - 2);
                oppTthreeAwayTwoLeftOneRight.PatternStringBuilder.Append('0');
                PatternList.Add(oppTthreeAwayTwoLeftOneRight);

                // 00XX0
                Pattern threeAwayTwoRightOneLeft = new Pattern(40000);
                threeAwayTwoRightOneLeft.PatternStringBuilder.Append('0');
                threeAwayTwoRightOneLeft.PatternStringBuilder.Append(ownSign.ToString()[0], signsInRowToWin - 3);
                threeAwayTwoRightOneLeft.PatternStringBuilder.Append('0');
                threeAwayTwoRightOneLeft.PatternStringBuilder.Append('0');
                PatternList.Add(threeAwayTwoRightOneLeft);

                // 00XX0
                Pattern oppThreeAwayTwoRightOneLeft = new Pattern(30000);
                oppThreeAwayTwoRightOneLeft.PatternStringBuilder.Append('0');
                oppThreeAwayTwoRightOneLeft.PatternStringBuilder.Append(OppSign.ToString()[0], signsInRowToWin - 3);
                oppThreeAwayTwoRightOneLeft.PatternStringBuilder.Append('0');
                oppThreeAwayTwoRightOneLeft.PatternStringBuilder.Append('0');
                PatternList.Add(oppThreeAwayTwoRightOneLeft);
            }
        }
    }
}
