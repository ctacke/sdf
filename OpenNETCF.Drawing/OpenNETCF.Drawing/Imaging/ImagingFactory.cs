using System;

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.Drawing.Imaging
{
  /// <summary>
  /// This interface is used to create bitmaps and images and to manage image encoders and decoders.
  /// </summary>
  [Guid("327ABDA7-072B-11D3-9D7B-0000F81EF32E")]
  [ComImport, CoClass(typeof(ImagingFactoryClass))]
  [CLSCompliant(false)]
  public interface ImagingFactory : IImagingFactory
  {
  }
}
