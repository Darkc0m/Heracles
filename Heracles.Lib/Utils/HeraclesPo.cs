using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.Media.Text;

namespace Heracles.Lib.Utils
{
    public static class HeraclesPo
    {
        public static Po generate() {
            return new Po(new PoHeader("Glory of Heracles", "Darkc0m", "es"));
        }
    }
}
