Imports System
Imports System.Collections.Generic
Imports System.Text
Imports OpenNETCF.WindowsMobile
Imports System.Threading
Imports System.Diagnostics

Namespace RadioToggle

    Module Module1

        Sub Main()
            Dim radios As Radios = radios.GetRadios()

            Debug.WriteLine("" & Chr(10) & "Before" & Chr(13) & Chr(10) & "--------")
            For Each radio As IRadio In radios
                Debug.WriteLine(String.Format("Name: {0}, Type: {1}, State: {2}", radio.DeviceName, radio.RadioType.ToString(), radio.RadioState.ToString()))

                ' toggle all radio states 
                radio.RadioState = IIf((radio.RadioState = RadioState.On), RadioState.Off, RadioState.On)
            Next

            ' give the radios enough time to change state - some (like BT) seem to be slow 
            Thread.Sleep(1000)

            radios.Refresh()

            ' display again 
            Debug.WriteLine("" & Chr(13) & Chr(10) & "After" & Chr(13) & Chr(10) & "--------")
            For Each radio As IRadio In radios
                Debug.WriteLine(String.Format("Name: {0}, Type: {1}, State: {2}", radio.DeviceName, radio.RadioType.ToString(), radio.RadioState.ToString()))
            Next
            Debug.WriteLine("" & Chr(13) & Chr(10) & Chr(10))
            Thread.Sleep(100)

        End Sub

    End Module
End Namespace

