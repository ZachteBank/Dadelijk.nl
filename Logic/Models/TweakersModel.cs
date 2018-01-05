using System;
using System.Collections.Generic;
using System.Text;
using Logic.Json;

namespace Logic.Models
{
    class TweakersModel: IBaseModel
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime DateTime { get; set; }
    }
}
