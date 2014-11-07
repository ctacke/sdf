Imports Microsoft.WindowsMobile.Forms
Imports OpenNETCF.Drawing.Imaging

Public Class Form1

    Private Sub mnuLoadImaging_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim dlg As New SelectPictureDialog()
        If dlg.ShowDialog() = DialogResult.OK Then
            Try
                Dim factory As ImagingFactory = New ImagingFactoryClass()
                Dim img As IImage
                factory.CreateImageFromFile(dlg.FileName, img)
                Dim imgB As IBitmapImage
                factory.CreateBitmapFromImage(img, CUInt(pbImage.Width), CUInt(pbImage.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb, InterpolationHint.InterpolationHintDefault, imgB)
                pbImage.Image = ImageUtils.IBitmapImageToBitmap(imgB)
            Catch generatedExceptionName As OutOfMemoryException
                MessageBox.Show("Out of memory")
            End Try
        End If

    End Sub
    Private Sub mnuLoadDirect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim dlg As SelectPictureDialog = New SelectPictureDialog()
        If dlg.ShowDialog() = DialogResult.OK Then
            Try
                pbImage.Image = New Bitmap(dlg.FileName)
            Catch
                MessageBox.Show("Out of memory")
            End Try
        End If
    End Sub
End Class
