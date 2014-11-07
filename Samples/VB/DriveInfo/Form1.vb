Public Class Form1

    Private Sub btnGetInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Get the drive details
        If Me.listBox1.SelectedIndex >= 0 Then
            Dim di As OpenNETCF.IO.DriveInfo = CType(Me.listBox1.SelectedValue, OpenNETCF.IO.DriveInfo)
            Me.AvailableFreeSpace.Text = di.AvailableFreeSpace.ToString("N0")
            Me.RootDirectory.Text = di.RootDirectory.ToString()
            Me.TotalFreeSpace.Text = di.TotalFreeSpace.ToString("N0")
            Me.TotalSize.Text = di.TotalSize.ToString("N0")
        End If

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.listBox1.DataSource = OpenNETCF.IO.DriveInfo.GetDrives()
    End Sub
End Class
