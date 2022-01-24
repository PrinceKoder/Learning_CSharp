using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson0
{
    class FactoryJSON : Factory
    {
        public UnitJSON[] unitJSON { get; set; }
    }
    class UnitJSON : Unit
    {
        public Tank[] tank { get; set; }
    }
    class ExportToJSON
    {
        public FactoryJSON[] factoryJSON { get; set; }
    }

}
