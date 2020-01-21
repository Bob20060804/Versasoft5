using System;

namespace Ersa.Global.Common.Data.Attributes
{
    public class EDC_SpaltenInformation : Attribute
    {
        public string PRO_strName
        {
            get;
            set;
        }

        public int PRO_i32Length
        {
            get;
            set;
        }

        public bool PRO_blnIsRequired
        {
            get;
            set;
        }

        public bool PRO_blnIsPrimary
        {
            get;
            set;
        }

        public bool PRO_blnIsUniqueIndex
        {
            get;
            set;
        }

        public bool PRO_blnIsNonUniqueIndex
        {
            get;
            set;
        }

        public bool PRO_blnIsDynamischeSpalte
        {
            get;
            set;
        }

        public object PRO_objDefaultWert
        {
            get;
            set;
        }

        public EDC_SpaltenInformation()
        {
        }

        public EDC_SpaltenInformation(string i_strName)
        {
            PRO_strName = i_strName;
        }
    }
}
