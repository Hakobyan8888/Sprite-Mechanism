using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2.MultiSelectObjects
{
    public class MultiSelectionHub
    {
        public static MultiSelectionHub Instance;

        public UiObject CurrentMultiSelection { get; set; }

    }
}
