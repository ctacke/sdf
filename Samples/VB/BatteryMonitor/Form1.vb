Public Class Form1

    Private Sub DeviceManagement_TimeChanged()
        MessageBox.Show("time changed")
    End Sub

    Private Sub DeviceManagement_ACPowerApplied()
        MessageBox.Show("Power")
    End Sub

    Private Sub DeviceManagement_DeviceWake()
        MessageBox.Show("Device wake")
    End Sub

    Private Sub DeviceManagement_ACPowerRemoved()
        MessageBox.Show("No Power!")
    End Sub

    Private Sub menuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItem1.Click
        Me.menuItem1.Enabled = False
        Me.batteryMonitor1.Enabled = True
        Me.textBox21.Text = "Monitor Started...."
    End Sub
    Private Sub numericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles numericUpDown1.ValueChanged
        Me.batteryMonitor1.PrimaryBatteryLifeTrigger = CInt(numericUpDown1.Value)
    End Sub

    Private Sub batteryMonitor1_PrimaryBatteryLifeNotification(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles batteryMonitor1.PrimaryBatteryLifeNotification
        Me.menuItem1.Enabled = True
        Me.textBox21.Text = "Plugin now!!"
    End Sub
End Class
