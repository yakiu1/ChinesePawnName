using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinesePawnName.models
{
    class NickNameWord
    {
        public string word { get; set; }
        // 1 first ,2 last
        public int type { get; set; }

        public NickNameWord() {
            word = "";
            type = 1;
        }
    }
}
