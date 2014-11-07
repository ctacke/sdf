using System;

namespace OpenNETCF.Drawing
{
	/// <summary>
	/// Specifies alignment of content on the drawing surface.
	/// </summary>
	public enum ContentAlignment2
	{
		/// <summary>
		/// Content is vertically aligned at the bottom, and horizontally aligned at the center.
		/// </summary>
		BottomCenter	= 0x200,
		/// <summary>
		/// Content is vertically aligned at the bottom, and horizontally aligned on the left.
		/// </summary>
		BottomLeft		= 0x100,
		/// <summary>
		/// Content is vertically aligned at the bottom, and horizontally aligned on the right.
		/// </summary>
		BottomRight		= 0x400,
		/// <summary>
		/// Content is vertically aligned in the middle, and horizontally aligned at the center.
		/// </summary>
		MiddleCenter	= 0x020,
		/// <summary>
		/// Content is vertically aligned in the middle, and horizontally aligned on the left.
		/// </summary>
		MiddleLeft		= 0x010,
		/// <summary>
		/// Content is vertically aligned in the middle, and horizontally aligned on the right.
		/// </summary>
		MiddleRight		= 0x040,
		/// <summary>
		/// Content is vertically aligned at the top, and horizontally aligned at the center.
		/// </summary>
		TopCenter		= 0x002,
		/// <summary>
		/// Content is vertically aligned at the top, and horizontally aligned on the left.
		/// </summary>
		TopLeft			= 0x001,
		/// <summary>
		/// Content is vertically aligned at the top, and horizontally aligned on the right.
		/// </summary>
		TopRight		= 0x004
	}
}