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
        Me.components = New System.ComponentModel.Container
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.label2 = New System.Windows.Forms.Label
        Me.numericUpDown1 = New System.Windows.Forms.NumericUpDown
        Me.menuItem1 = New System.Windows.Forms.MenuItem
        Me.label1 = New System.Windows.Forms.Label
        Me.batteryLife1 = New OpenNETCF.Windows.Forms.BatteryLife
        Me.textBox21 = New OpenNETCF.Windows.Forms.TextBox2
        Me.batteryMonitor1 = New OpenNETCF.Windows.Forms.BatteryMonitor(Me.components)
        Me.SuspendLayout()
        '
        'label2
        '
        Me.label2.Location = New System.Drawing.Point(9, 60)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(100, 20)
        Me.label2.Text = "Level Trigger:"
        '
        'numericUpDown1
        '
        Me.numericUpDown1.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.numericUpDown1.Location = New System.Drawing.Point(9, 86)
        Me.numericUpDown1.Name = "numericUpDown1"
        Me.numericUpDown1.Size = New System.Drawing.Size(100, 22)
        Me.numericUpDown1.TabIndex = 5
        Me.numericUpDown1.Value = New Decimal(New Integer() {25, 0, 0, 0})
        '
        'menuItem1
        '
        Me.menuItem1.Text = "Start Monitor"
        '
        'label1
        '
        Me.label1.Location = New System.Drawing.Point(9, 7)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(100, 20)
        Me.label1.Text = "Battery Life:"
        '
        'batteryLife1
        '
        Me.batteryLife1.Location = New System.Drawing.Point(9, 154)
        Me.batteryLife1.Name = "batteryLife1"
        Me.batteryLife1.Size = New System.Drawing.Size(189, 20)
        Me.batteryLife1.TabIndex = 9
        '
        'textBox21
        '
        Me.textBox21.CharacterCasing = OpenNETCF.Windows.Forms.CharacterCasing.Normal
        Me.textBox21.Location = New System.Drawing.Point(9, 127)
        Me.textBox21.Name = "textBox21"
        Me.textBox21.Size = New System.Drawing.Size(189, 21)
        Me.textBox21.TabIndex = 8
        '
        'batteryMonitor1
        '
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.batteryLife1)
        Me.Controls.Add(Me.textBox21)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.numericUpDown1)
        Me.Controls.Add(Me.label1)
        Me.KeyPreview = True
        Me.Menu = Me.mainMenu1
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents numericUpDown1 As System.Windows.Forms.NumericUpDown
    Private WithEvents menuItem1 As System.Windows.Forms.MenuItem
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents batteryLife1 As OpenNETCF.Windows.Forms.BatteryLife
    Private WithEvents textBox21 As OpenNETCF.Windows.Forms.TextBox2
    Private WithEvents batteryMonitor1 As OpenNETCF.Windows.Forms.BatteryMonitor

End Class
