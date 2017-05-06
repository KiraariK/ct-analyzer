using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DICOMopener.Serialization
{
    [DataContract]
    public class CtSerializer
    {
        [DataMember]
        public string[] Filenames { get; set; } // filenames of all dicom files of a patient

        [DataMember]
        public short[] DicomMatrices { get; set; } // dicom image data of all dicom files of a patient

        [DataMember]
        public int ImageMatrixHeight { get; set; } // dicom image (slice) height

        [DataMember]
        public int ImageMatrixWidth { get; set; } // dicom image (slice) width

        public CtSerializer(string[] filenames = null, Array[] dicomMatrices = null, int imageMatrixHeight = 0, int imageMatrixWidth = 0)
        {
            Filenames = filenames;

            // reform dicom image data array - it should be an one-dimension array
            if (dicomMatrices == null)
                DicomMatrices = null;
            else
            {
                DicomMatrices = new short[dicomMatrices.Length * imageMatrixHeight * imageMatrixWidth];
                for (int k = 0; k < dicomMatrices.Length; k++)
                {
                    for (int i = 0; i < imageMatrixHeight; i++)
                    {
                        for (int j = 0; j < imageMatrixWidth; j++)
                        {
                            DicomMatrices[(imageMatrixHeight * imageMatrixWidth * k) + (imageMatrixWidth * i) + j] =
                                (short)dicomMatrices[k].GetValue(i, j);
                            //DicomMatrices.SetValue((short)dicomMatrices[k].GetValue(i, j),
                            //    (imageMatrixHeight * imageMatrixWidth * k) + (imageMatrixWidth * i) + j);
                        }
                    }
                }
            }

            ImageMatrixHeight = imageMatrixHeight;
            ImageMatrixWidth = imageMatrixWidth;
        }

        /// <summary>
        /// Serializes object of current class as json
        /// </summary>
        /// <returns>json string</returns>
        public string SerializeCtJson()
        {
            if (Filenames == null || DicomMatrices == null)
                return string.Empty;

            MemoryStream ms = new MemoryStream();

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(CtSerializer));
            serializer.WriteObject(ms, this);
            byte[] json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

        /// <summary>
        /// Deserializes json string into object of current class
        /// </summary>
        /// <param name="json">json string</param>
        /// <returns>Object of current class</returns>
        public static CtSerializer DeserializeCtJson(string json)
        {
            CtSerializer deserializedObject = new CtSerializer();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(deserializedObject.GetType());
            deserializedObject = serializer.ReadObject(ms) as CtSerializer;
            ms.Close();
            return deserializedObject;
        }
    }
}
