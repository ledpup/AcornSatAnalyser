﻿namespace AcornSat.WebApi.Model.DataSetBuilder
{
    public class RawBin
    {
        public BinIdentifier Identifier { get; set; }
        public string BinFriendlyName { get; set; }
        public Bucket[] Buckets { get; set; }
    }
}
