namespace DICOMopener
{
    public class EWindow
    {
        // all values are stored in DICOM units

        public EWindowType MinBorder { get; set; }
        public EWindowType MaxBorder { get; set; }
        public EWindowType MinLevel { get; set; }
        public EWindowType MaxLevel { get; set; }
        public string WindowLabel { get; set; }
        public string WindowCenterLabelDICOM { get; set; }
        public string WindowCenterLabelHounsfield { get; set; }
        public string WindowWidthLabelDICOM { get; set; }
        public string WindowWidthLabelHounsfield { get; set; }
        public bool isDICOMunits { get; set; }

        public short CenterDICOM
        {
            get { return (short)(MinLevel.DICOMUnit + (MaxLevel.DICOMUnit - MinLevel.DICOMUnit) / 2); }
            set { }
        }

        public short WidthDICOM
        {
            get { return (short)(MaxLevel.DICOMUnit - MinLevel.DICOMUnit); }
            set { }
        }

        public short CenterHounsfield
        {
            get { return (short)(MinLevel.HounsfieldUnit + (MaxLevel.HounsfieldUnit - MinLevel.HounsfieldUnit) / 2); }
            set { }
        }

        public short WidthHounsfield
        {
            get { return (short)(MaxLevel.HounsfieldUnit - MinLevel.HounsfieldUnit); }
            set { }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="type">Type of Electronic Window</param>
        public EWindow(EWindowType.EWType type, bool isDICOMu = true)
        {
            MinBorder = new EWindowType();
            MaxBorder = new EWindowType();
            MinLevel = new EWindowType();
            MaxLevel = new EWindowType();

            MinBorder.DICOMUnit = 0;
            MaxBorder.DICOMUnit = 3600;
            MinBorder.Type = type;
            MaxBorder.Type = type;

            switch (type)
            {
                case (EWindowType.EWType.Wide):
                    MinLevel = MinBorder;
                    MaxLevel = MaxBorder;
                    WindowCenterLabelDICOM = string.Format("Recomended center: {0}", CenterDICOM);
                    WindowCenterLabelHounsfield = string.Format("Recomended center: {0}", CenterHounsfield);
                    WindowWidthLabelDICOM = string.Format("Recomended width: {0}", WidthDICOM);
                    WindowWidthLabelHounsfield = string.Format("Recomended width: {0}", WidthHounsfield);
                    break;

                case (EWindowType.EWType.Soft):
                    //MinLevel.DICOMUnit = 889;
                    //MaxLevel.DICOMUnit = 1239;
                    MinLevel.DICOMUnit = 852;
                    MaxLevel.DICOMUnit = 1277;
                    WindowCenterLabelDICOM = string.Format("Recomended center: {0}", 1064);
                    WindowCenterLabelHounsfield = string.Format("Recomended center: {0}", 40);
                    WindowWidthLabelDICOM = string.Format("Recomended width: {0}..{1}", 350, 500);
                    WindowWidthLabelHounsfield = string.Format("Recomended width: {0}..{1}", 350, 500);
                    break;

                case (EWindowType.EWType.Lung):
                    //MinLevel.DICOMUnit = 51 * (-1);
                    //MaxLevel.DICOMUnit = 699;
                    MinLevel.DICOMUnit = 164 * (-1);
                    MaxLevel.DICOMUnit = 712;
                    WindowCenterLabelDICOM = string.Format("Recomended center: {0}..{1}", 224, 324);
                    WindowCenterLabelHounsfield = string.Format("Recomended center: {0}..{1}", 800 * (-1), 700 * (-1));
                    WindowWidthLabelDICOM = string.Format("Recomended width: {0}..{1}", 750, 1000);
                    WindowWidthLabelHounsfield = string.Format("Recomended width: {0}..{1}", 750, 1000);
                    break;

                case (EWindowType.EWType.Pleural):
                    //MinLevel.DICOMUnit = 376 * (-1);
                    //MaxLevel.DICOMUnit = 1124;
                    MinLevel.DICOMUnit = 301 * (-1);
                    MaxLevel.DICOMUnit = 1449;
                    WindowCenterLabelDICOM = string.Format("Recomended center: {0}..{1}", 374, 774);
                    WindowCenterLabelHounsfield = string.Format("Recomended center: {0}..{1}", 650 * (-1), 250 * (-1));
                    WindowWidthLabelDICOM = string.Format("Recomended width: {0}..{1}", 1500, 2000);
                    WindowWidthLabelHounsfield = string.Format("Recomended width: {0}..{1}", 1500, 2000);
                    break;

                case (EWindowType.EWType.Bone):
                    //MinLevel.DICOMUnit = 674;
                    //MaxLevel.DICOMUnit = 1674;
                    MinLevel.DICOMUnit = 524;
                    MaxLevel.DICOMUnit = 2024;
                    WindowCenterLabelDICOM = string.Format("Recomended center: {0}..{1}", 1174, 1374);
                    WindowCenterLabelHounsfield = string.Format("Recomended center: {0}..{1}", 150, 350);
                    WindowWidthLabelDICOM = string.Format("Recomended width: {0}..{1}", 1000, 2000);
                    WindowWidthLabelHounsfield = string.Format("Recomended width: {0}..{1}", 1000, 2000);
                    break;

                default:
                    MinLevel = MinBorder;
                    MaxLevel = MaxBorder;
                    WindowCenterLabelDICOM = string.Format("Recomended center: {0}", CenterDICOM);
                    WindowCenterLabelHounsfield = string.Format("Recomended center: {0}", CenterHounsfield);
                    WindowWidthLabelDICOM = string.Format("Recomended width: {0}", WidthDICOM);
                    WindowWidthLabelHounsfield = string.Format("Recomended width: {0}", WidthHounsfield);
                    break;
            }

            MinLevel.Type = type;
            MaxLevel.Type = type;

            WindowLabel = MinLevel.EWTypeLabels[type]; // It is possible to use any EWindowType property here

            isDICOMunits = isDICOMu;
        }

        /// <summary>
        /// Special constructor
        /// </summary>
        /// <param name="type">Type of Electronic Window</param>
        /// <param name="centerDICOM">The Center of the Window in DICOM units</param>
        /// <param name="widthDICOM">The Width of the Window in DICOM units</param>
        public EWindow(EWindowType.EWType type, short center, short width, bool isDICOMu = true)
        {
            MinBorder = new EWindowType();
            MaxBorder = new EWindowType();
            MinLevel = new EWindowType();
            MaxLevel = new EWindowType();

            MinBorder.DICOMUnit = 0;
            MaxBorder.DICOMUnit = 3600;
            MinBorder.Type = type;
            MaxBorder.Type = type;

            if (isDICOMu)
            {
                MinLevel.DICOMUnit = (short)(center - (width / 2));
                MaxLevel.DICOMUnit = (short)(center + (width / 2));
            }
            else
            {
                MinLevel.HounsfieldUnit = (short)(center - (width / 2));
                MaxLevel.HounsfieldUnit = (short)(center + (width / 2));
            }

            switch (type)
            {
                case (EWindowType.EWType.Wide):
                    WindowCenterLabelDICOM = string.Format("Recomended center: {0}", CenterDICOM);
                    WindowCenterLabelHounsfield = string.Format("Recomended center: {0}", CenterHounsfield);
                    WindowWidthLabelDICOM = string.Format("Recomended width: {0}", WidthDICOM);
                    WindowWidthLabelHounsfield = string.Format("Recomended width: {0}", WidthHounsfield);
                    break;

                case (EWindowType.EWType.Soft):
                    WindowCenterLabelDICOM = string.Format("Recomended center: {0}", 1064);
                    WindowCenterLabelHounsfield = string.Format("Recomended center: {0}", 40);
                    WindowWidthLabelDICOM = string.Format("Recomended width: {0}..{1}", 350, 500);
                    WindowWidthLabelHounsfield = string.Format("Recomended width: {0}..{1}", 350, 500);
                    break;

                case (EWindowType.EWType.Lung):
                    WindowCenterLabelDICOM = string.Format("Recomended center: {0}..{1}", 224, 324);
                    WindowCenterLabelHounsfield = string.Format("Recomended center: {0}..{1}", 800 * (-1), 700 * (-1));
                    WindowWidthLabelDICOM = string.Format("Recomended width: {0}..{1}", 750, 1000);
                    WindowWidthLabelHounsfield = string.Format("Recomended width: {0}..{1}", 750, 1000);
                    break;

                case (EWindowType.EWType.Pleural):
                    WindowCenterLabelDICOM = string.Format("Recomended center: {0}..{1}", 374, 774);
                    WindowCenterLabelHounsfield = string.Format("Recomended center: {0}..{1}", 650 * (-1), 250 * (-1));
                    WindowWidthLabelDICOM = string.Format("Recomended width: {0}..{1}", 1500, 2000);
                    WindowWidthLabelHounsfield = string.Format("Recomended width: {0}..{1}", 1500, 2000);
                    break;

                case (EWindowType.EWType.Bone):
                    WindowCenterLabelDICOM = string.Format("Recomended center: {0}..{1}", 1174, 1374);
                    WindowCenterLabelHounsfield = string.Format("Recomended center: {0}..{1}", 150, 350);
                    WindowWidthLabelDICOM = string.Format("Recomended width: {0}..{1}", 1000, 2000);
                    WindowWidthLabelHounsfield = string.Format("Recomended width: {0}..{1}", 1000, 2000);
                    break;

                default:
                    WindowCenterLabelDICOM = string.Format("Recomended center: {0}", CenterDICOM);
                    WindowCenterLabelHounsfield = string.Format("Recomended center: {0}", CenterHounsfield);
                    WindowWidthLabelDICOM = string.Format("Recomended width: {0}", WidthDICOM);
                    WindowWidthLabelHounsfield = string.Format("Recomended width: {0}", WidthHounsfield);
                    break;
            }

            MinLevel.Type = type;
            MaxLevel.Type = type;

            WindowLabel = MinLevel.EWTypeLabels[type]; // It is possible to use any EWindowType property here

            isDICOMunits = isDICOMu;
        }
    }
}
