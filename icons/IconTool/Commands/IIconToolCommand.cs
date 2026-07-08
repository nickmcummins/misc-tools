using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IconTool.Commands
{
    public interface IIconToolCommand<TArgs>
    {
        public void Handle(TArgs args);
    }
}
