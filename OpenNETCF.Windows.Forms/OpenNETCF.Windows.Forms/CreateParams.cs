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

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Encapsulates the information needed when creating a control.
	/// </summary>
	public class CreateParams
	{
        #region Fields

        private string caption;
        private string className;
        private int classStyle;
        private int style;
        private int exStyle;
        private int height;
        private IntPtr param;
        private IntPtr parent;
        private int width;
        private int x;
        private int y;

        #endregion

        /// <summary>
        /// Initializes a new instance of the CreateParams class.
        /// </summary>
        public CreateParams()
        {
            param = IntPtr.Zero;
            parent = IntPtr.Zero;
            caption = String.Empty;
            className = String.Empty;
        }

        /// <summary>
        /// Gets or sets the control's initial text.
        /// </summary>
        public string Caption
        {
            get
            {
                return caption;
            }
            set
            {
                caption = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the Windows class to derive the control from.
        /// </summary>
        public string ClassName
        {
            get
            {
                return className;
            }
            set
            {
                className = value;
            }
        }

        /// <summary>
        /// Gets or sets a bitwise combination of class style values.
        /// </summary>
        public int ClassStyle
        {
            get
            {
                return classStyle;
            }
            set
            {
                classStyle = value;
            }
        }

        /// <summary>
        /// Gets or sets a bitwise combination of window style values.
        /// </summary>
        public int Style
        {
            get
            {
                return style;
            }
            set
            {
                style = value;
            }
        }


        /// <summary>
        /// Gets or sets additional parameter information needed to create the control.
        /// </summary>
        public IntPtr Param
        {
            get
            {
                return param;
            }
            set
            {
                param = value;
            }
        }

        /// <summary>
        /// Gets or sets a bitwise combination of extended window style values.
        /// </summary>
        public int ExStyle
        {
            get
            {
                return exStyle;
            }
            set
            {
                exStyle = value;
            }
        }

        /// <summary>
        /// Gets or sets the initial height of the control.
        /// </summary>
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        /// <summary>
        /// Gets or sets the control's parent.
        /// </summary>
        public IntPtr Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }

        /// <summary>
        /// Gets or sets the initial width of the control.
        /// </summary>
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        /// <summary>
        /// Gets or sets the initial left position of the control.
        /// </summary>
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        /// <summary>
        /// Gets or sets the initial top position of the control.
        /// </summary>
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
	}
}
