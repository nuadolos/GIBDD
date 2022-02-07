using GIBDD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GIBDD.Utilities
{
    internal class Transition
    {
        public static Frame MainFrame { get; set; }

        private static GIBDDEntities context;
        public static GIBDDEntities Context
        {
            get
            {
                if (context == null)
                    context = new GIBDDEntities();

                return context;
            }
        }
    }
}
