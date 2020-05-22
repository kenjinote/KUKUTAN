using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KUKUTAN
{
    class kakezanyomikata
    {
        public static void kukutanyomi()
        {
            for (int i = 1; i <= 9; i++)
            {
                for (int j = 1; j <= 9; j++)
                {
                    var str = Properties.Resources.ResourceManager.GetString(i.ToString() + j.ToString());
                    Module1.kakeyomikata[i, j] = str.Split(',')[0];
                    Module1.kakeyomikatakotae[i, j] = str.Split(',')[1];
                }
            }
        }
    }
}
