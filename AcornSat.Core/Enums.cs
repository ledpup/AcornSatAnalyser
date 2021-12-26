﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcornSat.Core
{
    public static class Enums
    {
        public enum DataResolution
        {
            Yearly,
            Monthly,
            Daily,
        }

        public enum MeasurementType
        {
            Unadjusted,
            Adjusted
        }

        public enum EnsoIndex
        {
            Mei,
            Oni,
            Soi,
        }
    }
}
