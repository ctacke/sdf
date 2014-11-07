Imports OpenNETCF.WindowsCE

Public Class Form1
    ' create the timer object
    Dim WithEvents timer As New LargeIntervalTimer

    Private Sub start_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles start.Click
        ' disable the start button so it can be clicked only once
        start.Enabled = False

        ' update a label to show when we started
        started.Text = DateTime.Now.ToString("HH:mm:ss")

        ' set the timer's properties
        timer.OneShot = False                               'run forever
        timer.FirstEventTime = DateTime.Now.AddSeconds(15)  ' start in 15 seconds
        timer.Interval = New TimeSpan(0, 0, 30)             ' fire every 30 seconds after that

        ' start the timer
        timer.Enabled = True
    End Sub

    Private Sub OnTick(ByVal sender As Object, ByVal args As EventArgs) Handles timer.Tick
        ' update a label to show we've ticked
        lastTick.Text = DateTime.Now.ToString("HH:mm:ss")
    End Sub
End Class

