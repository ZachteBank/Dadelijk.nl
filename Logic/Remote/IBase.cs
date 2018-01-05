using System;
using System.Collections.Generic;
using System.Text;
using Logic.Json;

namespace Logic.Remote
{
    interface IBase
    {
        IEnumerable<IBaseModel> GetAllItems(int limit = 25);
    }
}
