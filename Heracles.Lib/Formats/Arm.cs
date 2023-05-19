using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;
using Yarhl.Media.Text;

namespace Heracles.Lib.Formats
{
    public class Arm : IFormat
    {
        public Po status;
        public Po outskirtsLoc;
        public Po locations;
        public Po abilities;
        public Po spells;
        public Po enemySpells;
        public Po playerSkills;
        public Po combatSkillDialogs;
        public Po dialogs;
        public byte[] original;
    }
}
