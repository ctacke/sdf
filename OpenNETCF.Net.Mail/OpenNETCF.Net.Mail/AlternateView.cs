#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



using System;

using System.Collections.Generic;
using System.Text;
using System.IO;
using OpenNETCF.Net.Mime;

namespace OpenNETCF.Net.Mail
{
    public class AlternateView : AttachmentBase
    {
        // Fields
        //private LinkedResourceCollection linkedResources;

        // Methods
        internal AlternateView()
        {
        }

        public AlternateView(Stream contentStream)
            : base(contentStream)
        {
        }

        public AlternateView(string fileName)
            : base(fileName)
        {
        }

        public AlternateView(Stream contentStream, ContentType contentType)
            : base(contentStream, contentType)
        {
        }

        public AlternateView(Stream contentStream, string mediaType)
            : base(contentStream, mediaType)
        {
        }

        public AlternateView(string fileName, ContentType contentType)
            : base(fileName, contentType)
        {
        }

        public AlternateView(string fileName, string mediaType)
            : base(fileName, mediaType)
        {
        }

        public static AlternateView CreateAlternateViewFromString(string content)
        {
            AlternateView view = new AlternateView();
            view.SetContentFromString(content, null, string.Empty);
            return view;
        }

        public static AlternateView CreateAlternateViewFromString(string content, ContentType contentType)
        {
            AlternateView view = new AlternateView();
            view.SetContentFromString(content, contentType);
            return view;
        }

        public static AlternateView CreateAlternateViewFromString(string content, Encoding contentEncoding, string mediaType)
        {
            AlternateView view = new AlternateView();
            view.SetContentFromString(content, contentEncoding, mediaType);
            return view;
        }

        protected override void Dispose(bool disposing)
        {
            if (!base.disposed)
            {
                //if (disposing && (this.linkedResources != null))
                //{
                //    this.linkedResources.Dispose();
                //}
                base.Dispose(disposing);
            }
        }

        // Properties
        public Uri BaseUri
        {
            get
            {
                return base.ContentLocation;
            }
            set
            {
                base.ContentLocation = value;
            }
        }

        //public LinkedResourceCollection LinkedResources
        //{
        //    get
        //    {
        //        if (base.disposed)
        //        {
        //            throw new ObjectDisposedException(base.GetType().FullName);
        //        }
        //        if (this.linkedResources == null)
        //        {
        //            this.linkedResources = new LinkedResourceCollection();
        //        }
        //        return this.linkedResources;
        //    }
        //}
    }
}