﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComosWebSDK.Data
{
    public class CProject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string UID { get; set; }
        public string ProjectUID { get; set; }
        public string OverlayUID { get; set; }
        public string SystemFullName { get; set; }
        public string ClassType { get; set; }
        public string Picture { get; set; }
        public bool IsClientPicture { get; set; }

        public string Database { get; set; }
    }
}
