
using System;
using System.IO;
using System.Xml.Serialization;
//using Dime;

namespace OpenNETCF.Web.Services2.Dime {
    /// <summary>
    /// Used to specify the header and payload data for sending and receiving DIME attachments
    /// with the DimeSoapExtension.DimeExtension.
    /// </summary>
    public class DimeAttachment {
        string id;
        string type;
        TypeFormatEnum typeFormat;
        Stream stream;

        /// <summary>
        /// Creates a new DimeAttachment.
        /// </summary>
        public DimeAttachment() {
        }

        /*
        /// <summary>
        /// Creates a DimeAttachment object and automatically specifies a new unique Id in the form
        /// of a uuid:.
        /// </summary>
        /// <param name="type">The type of payload data in the record. The format of this type is 
        /// specified by the typeFormat parameter. For example, if typeFormat is TypeFormatEnum.MediaType
        /// then a valid type is "plain/text; charset=utf-8"</param>
        /// <param name="typeFormat">The format of the type parameter.</param>
        /// <param name="stream">The stream for reading or writing the payload data.</param>
		public DimeAttachment(String type, TypeFormatEnum typeFormat, Stream stream) {
            Id = "uuid:" + bNb.Sec.Rand.NewGuid().ToString("D");
			this.type = type;
            this.typeFormat = typeFormat;
            this.stream = stream;
        }
		*/

        /// <summary>
        /// Creates a DimeAttachment object
        /// </summary>
        /// <param name="id">The unique id for the DIME record.</param>
        /// <param name="type">The type of payload data in the record. The format of this type is 
        /// specified by the typeFormat parameter. For example, if typeFormat is TypeFormatEnum.MediaType
        /// then a valid type is "plain/text; charset=utf-8"</param>
        /// <param name="typeFormat">The format of the type parameter.</param>
        /// <param name="stream">The stream for reading or writing the payload data.</param>
        public DimeAttachment(string id, String type, TypeFormatEnum typeFormat, Stream stream) {
            Id = id;
            this.type = type;
            this.typeFormat = typeFormat;
            this.stream = stream;
        }

        /// <summary>
        /// The unique id for the attachment. When setting the id it will be
        /// converted into its canonical Uri format.
        /// </summary>    
        [XmlAttribute("href", DataType="anyURI", Namespace="uri://microsoft/dime/reference")]
        public string Id {
            get { return id; }
            set { 
                // Need to canonicalize the Id so that it will match the value
                // in the DimeRecord.
                id = new Uri(value).ToString(); 
            }
        }

        /// <summary>
        /// The type of payload data in the attachment. The format of this type name is 
        /// specified by the TypeFormat property. For example, if TypeFormat is TypeFormatEnum.MediaType
        /// then a valid Type is "plain/text; charset=utf-8"
        /// </summary>
        [XmlIgnore]
        [SoapIgnore]
        public string Type {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// The format of the Type property.
        /// </summary>
        [XmlIgnore]
        [SoapIgnore]
        public TypeFormatEnum TypeFormat {
            get { return typeFormat; }
            set { typeFormat = value; }
        }

        /// <summary>
        /// The stream for reading and writing the attachment payload data.
        /// </summary>
        [XmlIgnore] 
        [SoapIgnore]
        public Stream Stream {
            get { return stream; }
            set {
                if (value == null)
                    throw new ArgumentNullException("stream");
                stream = value;
            }
        }        
    }
}