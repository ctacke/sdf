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
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

namespace OpenNETCF.Drawing
{
    public partial class GraphicsEx
    {
        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern IntPtr GetDesktopWindow();
        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, CopyPixelOperation dwRop);

        private GraphicsEx(Graphics managedGraphics)
        {
            m_managedGraphics = managedGraphics;
            this.hDC = m_managedGraphics.GetHdc();
        }

        /// <summary>
        /// Performs a bit-block transfer of the color data, corresponding to a rectangle of pixels, from the screen to the drawing surface of the GraphicsEx. 
        /// </summary>
        /// <param name="sourceX">The x-coordinate of the point at the upper-left corner of the source rectangle.</param>
        /// <param name="sourceY">The y-coordinate of the point at the upper-left corner of the source rectangle</param>
        /// <param name="destinationX">The x-coordinate of the point at the upper-left corner of the destination rectangle.</param>
        /// <param name="destinationY">The y-coordinate of the point at the upper-left corner of the destination rectangle.</param>
        /// <param name="blockRegionSize">The size of the area to be transferred.</param>
        public void CopyFromScreen(int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize)
        {
            CopyFromScreen(sourceX, sourceY, destinationX, destinationY, blockRegionSize, CopyPixelOperation.SourceCopy);
        }

        /// <summary>
        /// Performs a bit-block transfer of the color data, corresponding to a rectangle of pixels, from the screen to the drawing surface of the GraphicsEx.
        /// </summary>
        /// <param name="sourceX">The x-coordinate of the point at the upper-left corner of the source rectangle.</param>
        /// <param name="sourceY">The y-coordinate of the point at the upper-left corner of the source rectangle</param>
        /// <param name="destinationX">The x-coordinate of the point at the upper-left corner of the destination rectangle.</param>
        /// <param name="destinationY">The y-coordinate of the point at the upper-left corner of the destination rectangle.</param>
        /// <param name="blockRegionSize">The size of the area to be transferred.</param>
        /// <param name="copyPixelOperation">One of the <c>CopyPixelOperation</c> values.</param>
        public void CopyFromScreen(int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize, CopyPixelOperation copyPixelOperation)
        {
            IntPtr desktopHwnd = GetDesktopWindow();
            if (desktopHwnd == IntPtr.Zero)
            {
                throw new System.ComponentModel.Win32Exception();
            }
            IntPtr desktopDC = GetWindowDC(desktopHwnd);
            if (desktopDC == IntPtr.Zero)
            {
                throw new System.ComponentModel.Win32Exception();
            }
            if (!BitBlt(hDC, destinationX, destinationY, blockRegionSize.Width, blockRegionSize.Height, desktopDC, sourceX, sourceY, copyPixelOperation))
            {
                throw new System.ComponentModel.Win32Exception();
            }
            ReleaseDC(desktopHwnd, desktopDC);
        }

        public static GraphicsEx FromGraphics(Graphics g)
        {
            return new GraphicsEx(g);
        }
    }
}
