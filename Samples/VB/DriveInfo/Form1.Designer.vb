<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.TotalSize = New OpenNETCF.Windows.Forms.TextBox2
        Me.TotalFreeSpace = New OpenNETCF.Windows.Forms.TextBox2
        Me.AvailableFreeSpace = New OpenNETCF.Windows.Forms.TextBox2
        Me.RootDirectory = New OpenNETCF.Windows.Forms.TextBox2
        Me.label4 = New System.Windows.Forms.Label
        Me.label3 = New System.Windows.Forms.Label
        Me.label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.listBox1 = New System.Windows.Forms.ListBox
        Me.btnGetInfo = New OpenNETCF.Windows.Forms.Button2
        Me.SuspendLayout()
        '
        'TotalSize
        '
        Me.TotalSize.CharacterCasing = OpenNETCF.Windows.Forms.CharacterCasing.Normal
        Me.TotalSize.Location = New System.Drawing.Point(2, 243)
        Me.TotalSize.Name = "TotalSize"
        Me.TotalSize.ReadOnly = True
        Me.TotalSize.Size = New System.Drawing.Size(233, 21)
        Me.TotalSize.TabIndex = 22
        '
        'TotalFreeSpace
        '
        Me.TotalFreeSpace.CharacterCasing = OpenNETCF.Windows.Forms.CharacterCasing.Normal
        Me.TotalFreeSpace.Location = New System.Drawing.Point(3, 201)
        Me.TotalFreeSpace.Name = "TotalFreeSpace"
        Me.TotalFreeSpace.ReadOnly = True
        Me.TotalFreeSpace.Size = New System.Drawing.Size(233, 21)
        Me.TotalFreeSpace.TabIndex = 21
        '
        'AvailableFreeSpace
        '
        Me.AvailableFreeSpace.CharacterCasing = OpenNETCF.Windows.Forms.CharacterCasing.Normal
        Me.AvailableFreeSpace.Location = New System.Drawing.Point(3, 161)
        Me.AvailableFreeSpace.Name = "AvailableFreeSpace"
        Me.AvailableFreeSpace.ReadOnly = True
        Me.AvailableFreeSpace.Size = New System.Drawing.Size(233, 21)
        Me.AvailableFreeSpace.TabIndex = 20
        '
        'RootDirectory
        '
        Me.RootDirectory.CharacterCasing = OpenNETCF.Windows.Forms.CharacterCasing.Normal
        Me.RootDirectory.Location = New System.Drawing.Point(3, 124)
        Me.RootDirectory.Name = "RootDirectory"
        Me.RootDirectory.ReadOnly = True
        Me.RootDirectory.Size = New System.Drawing.Size(233, 21)
        Me.RootDirectory.TabIndex = 19
        '
        'label4
        '
        Me.label4.Location = New System.Drawing.Point(3, 225)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(127, 18)
        Me.label4.Text = "TotalSize"
        '
        'label3
        '
        Me.label3.Location = New System.Drawing.Point(3, 185)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(127, 18)
        Me.label3.Text = "TotalFreeSpace"
        '
        'label2
        '
        Me.label2.Location = New System.Drawing.Point(3, 146)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(127, 18)
        Me.label2.Text = "AvailableFreeSpace"
        '
        'label1
        '
        Me.label1.Location = New System.Drawing.Point(3, 109)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(100, 18)
        Me.label1.Text = "RootDirectory"
        '
        'listBox1
        '
        Me.listBox1.Location = New System.Drawing.Point(2, 2)
        Me.listBox1.Name = "listBox1"
        Me.listBox1.Size = New System.Drawing.Size(234, 100)
        Me.listBox1.TabIndex = 18
        '
        'btnGetInfo
        '
        Me.btnGetInfo.Image = CType(resources.GetObject("btnGetInfo.Image"), System.Drawing.Image)
        Me.btnGetInfo.ImageAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleRight
        Me.btnGetInfo.Location = New System.Drawing.Point(158, 270)
        Me.btnGetInfo.Name = "btnGetInfo"
        Me.btnGetInfo.Size = New System.Drawing.Size(78, 20)
        Me.btnGetInfo.TabIndex = 17
        Me.btnGetInfo.Text = "Get Info"
        Me.btnGetInfo.TextAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleLeft
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 294)
        Me.Controls.Add(Me.TotalSize)
        Me.Controls.Add(Me.TotalFreeSpace)
        Me.Controls.Add(Me.AvailableFreeSpace)
        Me.Controls.Add(Me.RootDirectory)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.listBox1)
        Me.Controls.Add(Me.btnGetInfo)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents TotalSize As OpenNETCF.Windows.Forms.TextBox2
    Private WithEvents TotalFreeSpace As OpenNETCF.Windows.Forms.TextBox2
    Private WithEvents AvailableFreeSpace As OpenNETCF.Windows.Forms.TextBox2
    Private WithEvents RootDirectory As OpenNETCF.Windows.Forms.TextBox2
    Private WithEvents label4 As System.Windows.Forms.Label
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents listBox1 As System.Windows.Forms.ListBox
    Private WithEvents btnGetInfo As OpenNETCF.Windows.Forms.Button2

End Class
