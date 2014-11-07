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
    private mainMenu1 As System.Windows.Forms.MainMenu

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.apList = New System.Windows.Forms.ListView
        Me.refresh = New System.Windows.Forms.Button
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.nicName = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'apList
        '
        Me.apList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.apList.Columns.Add(Me.ColumnHeader1)
        Me.apList.Columns.Add(Me.ColumnHeader2)
        Me.apList.Columns.Add(Me.ColumnHeader3)
        Me.apList.FullRowSelect = True
        Me.apList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.apList.Location = New System.Drawing.Point(3, 69)
        Me.apList.Name = "apList"
        Me.apList.Size = New System.Drawing.Size(232, 223)
        Me.apList.TabIndex = 0
        Me.apList.View = System.Windows.Forms.View.Details
        '
        'refresh
        '
        Me.refresh.Location = New System.Drawing.Point(163, 43)
        Me.refresh.Name = "refresh"
        Me.refresh.Size = New System.Drawing.Size(72, 20)
        Me.refresh.TabIndex = 1
        Me.refresh.Text = "Refresh"
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "SSID"
        Me.ColumnHeader1.Width = 64
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "MAC"
        Me.ColumnHeader2.Width = 103
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Signal"
        Me.ColumnHeader3.Width = 60
        '
        'nicName
        '
        Me.nicName.Location = New System.Drawing.Point(4, 43)
        Me.nicName.Name = "nicName"
        Me.nicName.Size = New System.Drawing.Size(153, 20)
        Me.nicName.Text = "No NIC"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(238, 295)
        Me.Controls.Add(Me.nicName)
        Me.Controls.Add(Me.refresh)
        Me.Controls.Add(Me.apList)
        Me.Menu = Me.mainMenu1
        Me.Name = "Form1"
        Me.Text = "Basic WiFi Sample"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents apList As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents refresh As System.Windows.Forms.Button
    Friend WithEvents nicName As System.Windows.Forms.Label

End Class
