using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordAssistedTools.Models {
  internal class TransApiUserConfig : ITransApiConfigBase {
    public string Name { get; set; }
    public string Key { get; set; }
    public string ClassFullName { get; set; }
  }
}
