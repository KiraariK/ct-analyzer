using System.Collections.Generic;

namespace DICOMopener
{
    public class EWindowType
    {
        // all values are stored in DICOM units

        public enum EWType { Wide, Soft, Lung, Pleural, Bone }
        public Dictionary<EWType, string> EWTypeLabels;

        private short _value; // value wrote in DICOM
        public EWType Type { get; set; }

        public short HounsfieldUnit // value in Hounsfield Units
        {
            get { return (short)(_value - 1024); }
            set { _value = (short)(value + 1024); } // input value in Hounsfield units
        }

        public short DICOMUnit // value in DICOM units
        {
            get { return _value; }
            set { _value = value; }
        }

        public EWindowType()
        {
            EWTypeLabels = new Dictionary<EWType, string>();

            EWTypeLabels.Add(EWType.Wide, "Whole range");
            EWTypeLabels.Add(EWType.Soft, "Soft");
            EWTypeLabels.Add(EWType.Lung, "Lung");
            EWTypeLabels.Add(EWType.Pleural, "Pleural");
            EWTypeLabels.Add(EWType.Bone, "Bone");
        }
    }
}
