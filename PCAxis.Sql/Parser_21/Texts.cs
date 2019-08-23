using System;
using System.Collections.Generic;
using System.Text;

namespace PCAxis.Sql.Parser_21
{
    // This class should read from a configuration file.
    class Texts
    {
        private Dictionary<string,string> mPaxiomAnd;
        public Dictionary<string, string> PaxiomAnd
        {
            get { return mPaxiomAnd; }
            set { mPaxiomAnd = value; }
        }

        private Dictionary<string, string> mPaxiomBy;
        public Dictionary<string, string> PaxiomBy
        {
            get { return mPaxiomBy; }
            set { mPaxiomBy = value; }
        }
        private Dictionary<string, string> mPaxiomType;
        public Dictionary<string, string> PaxiomType
        {
            get { return mPaxiomType; }
            set { mPaxiomType = value; }
        }
        public Texts()
        {
            mPaxiomAnd = new Dictionary<string,string>();
            mPaxiomBy = new Dictionary<string, string>();
            mPaxiomType = new Dictionary<string, string>();

                mPaxiomAnd["en"] = "and";
                mPaxiomBy["en"] = "by";
                mPaxiomType["en"] = "type";
                mPaxiomAnd["no"] = "og";
                mPaxiomBy["no"] = "etter";
                mPaxiomType["no"] = "type";

        }
    }
}
