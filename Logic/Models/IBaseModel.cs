using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Json
{
    public interface IBaseModel
    {
        string Title { get; set; }
        string Url { get; set; }
        DateTime DateTime { get; set; }
    }
}
