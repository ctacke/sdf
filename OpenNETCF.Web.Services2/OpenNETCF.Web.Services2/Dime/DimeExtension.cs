
using System;
using System.IO;
using System.Collections;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Diagnostics;
using System.Text;

namespace OpenNETCF.Web.Services2.Dime 
{
	/// <summary>
	/// A soap extension that provides DIME encapsulation of SOAP messages and sending
	/// and receiving of DIME attachments.
	/// </summary>
	public class DimeExtension : SoapExtension 
	{
		Stream networkStream;
		Stream newStream;
		ArrayList outputAttachments;
		Hashtable inputAttachments;
		String contentType;
		String id;
		const string DimeContentType = "application/dime";
		const string SoapContentType = "http://schemas.xmlsoap.org/soap/envelope/";
		const int DefaultBufferSize = 512;
		byte[] copyBuffer;

		private DimeDir dimeDir = DimeDir.Both;

		public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute) 
		{
			return ((DimeExtensionAttribute) attribute).DimeDirection;
			//return null;
		}

		public override object GetInitializer(System.Type type) 
		{
			return DimeDir.Both;
			//return null;
		}

		public override void Initialize(object initializer) 
		{
			this.dimeDir = (DimeDir) initializer;
		}

		public override void ProcessMessage(SoapMessage message) 
		{
			switch (message.Stage) 
			{
				case SoapMessageStage.BeforeSerialize:
					BeforeSerialize(message );
					break;

				case SoapMessageStage.AfterSerialize:
					AfterSerialize( message );
					break;

				case SoapMessageStage.BeforeDeserialize:
					BeforeDeserialize( message );
					break;

				case SoapMessageStage.AfterDeserialize:
					AfterDeSerialize(message);
					break;

				default:
					throw new Exception("invalid stage");
			}
		}

    
		public override Stream ChainStream(Stream stream)
		{
			networkStream = stream;
			newStream = new MemoryStream(DefaultBufferSize);
			return newStream;
		}

		/// <summary>
		/// Collects the the set of DimeAttachment objects to send in the message.
		/// For referenced attachments these are collected from the parameter list or return value.
		/// For unreferenced attachments these are collected from the IDimeAttachmentContainter
		/// collections. The SOAP envelope and attachments are written into the network stream
		/// in the AfterSerialize method.
		/// If an exception has been thrown, the soap message containing the exception is 
		/// not encapsulated.
		/// </summary>
		private void BeforeSerialize(SoapMessage message) 
		{
			if(dimeDir == DimeDir.Response)
				return; //because not on request

			if (message.Exception == null) 
			{
				id = message.Action;
				message.ContentType = DimeContentType;
				outputAttachments = new ArrayList(); 
                
				if (message.GetType() != typeof(SoapClientMessage)) 
				{
					throw new Exception("DIME library not for server side processing");
					/*
					// check for unreferenced attachments in the container
					IDimeAttachmentContainer container = ((SoapServerMessage)message).Server as IDimeAttachmentContainer;
					if (container != null)
						outputAttachments.AddRange(container.ResponseAttachments);
					else 
					{
						// check for referenced attachments in out parameter list
						ParameterInfo[] parameters = message.MethodInfo.OutParameters;
						for (int i = 0; i < parameters.Length; i++) 
						{      
							// Note, using the as operator to test the type since out params have a unique type
							object outValue = message.GetOutParameterValue(i);
							if ((outValue as DimeAttachment) != null || (outValue as DimeAttachment[]) != null)
								AddAttachmentValue(outValue.GetType(), outValue);
						}

						// check for referenced attachment in return value
						Type returnType = message.MethodInfo.ReturnType;
						if (returnType == typeof(DimeAttachment) || returnType == typeof(DimeAttachment[]))
							AddAttachmentValue(returnType, message.GetReturnValue());
					}
					*/
				}
				else //client side
				{                    
					// check for unreferenced attachments in the container
					IDimeAttachmentContainer container = ((SoapClientMessage)message).Client as IDimeAttachmentContainer;
					if (container != null)
						outputAttachments.AddRange(container.RequestAttachments);
					else 
					{                    
						// check for referenced attachments in the parameter list
						ParameterInfo[] parameters = message.MethodInfo.InParameters;
						for (int i = 0;i < parameters.Length;i++) 
						{
							Type type = parameters[i].ParameterType;
							if (type == typeof(DimeAttachment) || type == typeof(DimeAttachment[]))
								AddAttachmentValue(type, message.GetInParameterValue(i));
						}
					}
				}
			}
		}
        
		void AddAttachmentValue(Type t, object o) 
		{
			if (t.IsArray) 
			{
				DimeAttachment[] atts = (DimeAttachment[]) o;
				for (int i = 0; i < atts.Length; i++) 
				{
					CheckAttachment(atts[i]);
					outputAttachments.Add(atts[i]);
				}
			}
			else 
			{
				DimeAttachment a = (DimeAttachment)o;
				CheckAttachment(a);
				outputAttachments.Add(a);
			}
		}
        
		/// <summary>
		/// Encapsulates the SOAP message into a DIME message.  The stored attachments are added as DIME records.
		/// If an exception has been thrown, the soap message containing the exception is 
		/// not encapsulated.
		/// </summary>
		private void AfterSerialize(SoapMessage message) 
		{
			newStream.Position = 0;
			//if response only, then not on request
			if (dimeDir != DimeDir.Response && message.Exception == null) 
			{
				DimeWriter dw = new DimeWriter(networkStream);
				int contentLength = (newStream.CanSeek && newStream.Length <= DefaultBufferSize) ? (int) newStream.Length : -1;
				DimeRecord record = null;
				if(outputAttachments.Count > 0)
					record = dw.NewRecord(id, SoapContentType, TypeFormatEnum.AbsoluteUri, contentLength);
				else
					record = dw.LastRecord(id, SoapContentType, TypeFormatEnum.AbsoluteUri, contentLength);
				BinaryCopy(newStream, record.BodyStream);                

				//add attachments                
				for (int i = 0; i < outputAttachments.Count; i++) 
				{
					DimeAttachment attachment = (DimeAttachment)outputAttachments[i];                    
					contentLength = attachment.Stream.CanSeek ? (int)attachment.Stream.Length : -1;
					if(i == (outputAttachments.Count - 1))
						record = dw.LastRecord(attachment.Id, attachment.Type, attachment.TypeFormat, contentLength);
					else
						record = dw.NewRecord(attachment.Id, attachment.Type, attachment.TypeFormat, contentLength);
					BinaryCopy(attachment.Stream, record.BodyStream);                    
				}
				dw.Close();
			}
			else
				BinaryCopy(newStream, networkStream);
		}

		/// <summary>
		/// Retrieves the SOAP message from the DIME message.  DIME attachments
		/// are removed from the stream and stored for future use by the AfterDeserialize
		/// method.
		/// </summary>
		private void BeforeDeserialize(SoapMessage message) 
		{
			if (message.ContentType == DimeContentType) 
			{
				contentType = message.ContentType;
				inputAttachments = new Hashtable();

				DimeReader dr = new DimeReader(networkStream);
				DimeRecord record = dr.ReadRecord();
				if (record.Type != SoapContentType)
					throw new Exception(String.Format("Expected content type '{0}' in record containing SOAP payload.", SoapContentType));
				message.ContentType = "text/xml";                
				BinaryCopy(record.BodyStream, newStream);
				record.Close();

				//get attachments
				while ((record = dr.ReadRecord()) != null) 
				{             
					//OutOfMemoryException
					Stream stream = new MemoryStream(record.Chunked ? DefaultBufferSize : record.ContentLength);
					BinaryCopy(record.BodyStream, stream);
					stream.Position = 0;
					DimeAttachment attachment = new DimeAttachment(record.Id.ToString(), record.Type, record.TypeFormat, stream);
					inputAttachments.Add(attachment.Id, attachment);                    
					record.Close();
				}
				dr.Close();
			}
			else
				BinaryCopy(networkStream, newStream);
			newStream.Position = 0;
		}


		/// <summary>
		/// Sets the method's DimeAttachment parameters and return value to the stored values. 
		/// </summary>
		private void AfterDeSerialize(SoapMessage message) 
		{
			if (contentType == DimeContentType) 
			{
				if (message.GetType() != typeof(SoapClientMessage)) 
				{
					throw new Exception("DIME library not for server side processing");
					/*
					// check for unreferenced attachments in the container
					IDimeAttachmentContainer container = ((SoapServerMessage)message).Server as IDimeAttachmentContainer;
					if (container != null) 
					{                        
						if (container.RequestAttachments == null) 
							throw new InvalidOperationException("The IDimeAttachmentContainer.RequestAttachments property must not be null.");
						container.RequestAttachments.AddRange(inputAttachments.Values);                        
					}
					else 
					{
						// check for referenced attachments in the parameter list
						ParameterInfo[] parameters = message.MethodInfo.InParameters;
						for (int i = 0; i < parameters.Length; i++) 
						{
							Type type = parameters[i].ParameterType;
							if (type == typeof(DimeAttachment)) 
							{
								// only the id is in the SOAP body so copy over other attachment fields into
								// the DimeAttachment object created during deserialization
								CopyFieldsFromInputAttachment((DimeAttachment)message.GetInParameterValue(i));
							}
							else if (type == typeof(DimeAttachment[])) 
							{
								CopyFieldsFromInputAttachment((DimeAttachment[])message.GetInParameterValue(i));
							}
						}
					}
					*/
				}
				else //client side
				{
					// check for unreferenced attachments in the container
					IDimeAttachmentContainer container = ((SoapClientMessage)message).Client as IDimeAttachmentContainer;
					if (container != null) 
					{                        
						if (container.ResponseAttachments == null) 
							throw new InvalidOperationException("The IDimeAttachmentContainer.ResponseAttachments property must not be null.");
						container.ResponseAttachments.AddRange(inputAttachments.Values);                        
					}
					else 
					{
						// check for referenced attachments in the out parameter list
						ParameterInfo[] parameters = message.MethodInfo.OutParameters;
						for (int i = 0; i < parameters.Length; i++) 
						{      
							// Note, using the as operator to test the type since out params have a unique type
							object outValue = message.GetOutParameterValue(i);
							DimeAttachment a =  outValue as DimeAttachment;
							if (a != null)
								CopyFieldsFromInputAttachment(a); 
							else 
							{                            
								DimeAttachment[] aa = outValue as DimeAttachment[];
								if (aa != null)
									CopyFieldsFromInputAttachment(aa); 
							}
						}
						Type returnType = message.MethodInfo.ReturnType;
						if (returnType == typeof(DimeAttachment)) 
							CopyFieldsFromInputAttachment((DimeAttachment)message.GetReturnValue());                                            
						else if (returnType == typeof(DimeAttachment[])) 
						{                        
							CopyFieldsFromInputAttachment((DimeAttachment[])message.GetReturnValue());
						}
					}
				}
			}
		}

		// Lookup the referenced attachment in the attachment table by id
		// and copy over the additional attachment data.
		private void CopyFieldsFromInputAttachment(DimeAttachment attachment) 
		{
			if (attachment == null) return;

			DimeAttachment fromAttachment = (DimeAttachment)inputAttachments[attachment.Id];            
			attachment.Type = fromAttachment.Type;
			attachment.TypeFormat = fromAttachment.TypeFormat;
			attachment.Stream = fromAttachment.Stream;
		}

		private void CopyFieldsFromInputAttachment(DimeAttachment[] attachments) 
		{
			if (attachments == null) return;
			for (int i = 0; i < attachments.Length; i++) 
			{
				CopyFieldsFromInputAttachment(attachments[i]);
			}
		}

		/// <summary>
		/// Validates the attachment.
		/// </summary>
		private DimeAttachment CheckAttachment(DimeAttachment attachment) 
		{
			Debug.Assert(attachment != null);
			if (attachment.Type == null || attachment.Stream == null || !attachment.Stream.CanRead)
				throw new Exception("DimeAttachment requires a valid type and a readable stream");
			return attachment;
		}

		/// <summary>
		/// Binary copy from one stream to another.
		/// </summary>
		private void BinaryCopy(Stream from, Stream to) 
		{
			if (copyBuffer == null)
				copyBuffer = new byte[DefaultBufferSize];

			int count;
			while ((count = from.Read(copyBuffer, 0, copyBuffer.Length)) > 0)
				to.Write(copyBuffer, 0, count);
		}
	}
}
