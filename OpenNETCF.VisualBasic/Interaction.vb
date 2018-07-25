Imports System.Diagnostics
Imports Microsoft.Win32

Public Module Interaction

    'Requires some changes to match the desktop signatures


    Private Const c_strKeyName As String = "Software\VB and VBA Program Settings\"

    ''' <summary>
    ''' Returns a key setting value from an application's entry in the Windows registry.
    ''' </summary>
    ''' <param name="AppName">Required. <see cref="String"/> expression containing the name of the application or project whose key setting is requested.</param>
    ''' <param name="Section">Required. <see cref="String"/> expression containing the name of the section in which the key setting is found.</param>
    ''' <param name="Key">Required. <see cref="String"/> expression containing the name of the key setting to return.</param>
    ''' <param name="Default">Optional. Expression containing the value to return if no value is set in the Key setting. If omitted, Default is assumed to be a zero-length string ("").</param>
    ''' <returns></returns>
    ''' <remarks>If any of the items named in the <b>GetSetting</b> arguments do not exist, <b>GetSetting</b> returns a value of <paramref name="Default"/>.
    ''' <para><b>GetSetting</b> requires that a user be logged on since it operates under the HKEY_LOCAL_USER registry key, which is not active until a user logs on interactively.</para>
    ''' <para>Registry settings that are to be accessed from a non-interactive process (such as mtx.exe) should be stored under either the HKEY_LOCAL_MACHINE\Software\ or the HKEY_USER\DEFAULT\Software registry keys.</para>
    ''' </remarks>
    Public Function GetSetting(ByVal AppName As String, ByVal Section As String, ByVal Key As String, Optional ByVal [Default] As String = "") As String
        
        Dim rk As RegistryKey

        ' make sure a section was specified
        If (AppName <> String.Empty) AndAlso (Section <> String.Empty) Then

            Try
                ' set up the registry key
                rk = Registry.CurrentUser.OpenSubKey(c_strKeyName & AppName & "\" & Section)

                'section doesn't exist
                If rk Is Nothing Then
                    Return [Default]

                    ' return default if it does not exist
                ElseIf rk.ValueCount = 0 Then
                    Return [Default]
                Else

                    ' return the key
                    Dim value As String = rk.GetValue(Key).ToString

                    ' close the registry key
                    rk.Close()

                    Return value

                    ' the setting was blank-save the default
                    'rk.SetValue(Key, [Default])
                End If
            Catch

                ' in case of an error, return the default
                Return [Default]
            End Try

        Else
            Return [Default]
        End If

        Return [Default]

    End Function

    ''' <summary>
    ''' Saves or creates an application entry in the Windows registry.
    ''' </summary>
    ''' <param name="AppName">Required. String expression containing the name of the application or project to which the setting applies.</param>
    ''' <param name="Section">Required. String expression containing the name of the section in which the key setting is being saved.</param>
    ''' <param name="Key">Required. String expression containing the name of the key setting being saved.</param>
    ''' <param name="Setting">Required. Expression containing the value to which Key is being set.</param>
    ''' <remarks>
    ''' The SaveSetting function adds the key to HKEY_CURRENT_USER\Software\VB and VBA Program Settings.
    ''' <para>If the key setting can't be saved for any reason, an error occurs.</para>
    ''' <para>SaveSetting requires that a user be logged on since it operates under the HKEY_LOCAL_USER registry key, which is not active until a user logs on interactively.</para>
    ''' </remarks>
    Public Sub SaveSetting(ByVal AppName As String, ByVal Section As String, ByVal Key As String, ByVal Setting As String)
        
        Dim rk As RegistryKey = Nothing

        ' make sure the required paramters were specified
        If (AppName <> String.Empty) AndAlso (Section <> String.Empty) AndAlso (Key <> String.Empty) Then

            Try
                ' set up the registry key
                rk = Registry.CurrentUser.CreateSubKey(c_strKeyName & AppName & "\" & Section)

                ' set the value
                rk.SetValue(Key, Setting)
            Catch
                Err.Raise(5, Nothing, "Registry Key could not be created")
            Finally
                ' close the registry key
                If Not rk Is Nothing Then
                    rk.Close()
                End If

            End Try
        Else
            Err.Raise(5, Nothing, "Argument 'Path' is missing or invalid")
        End If


    End Sub

    ''' <summary>
    ''' Deletes a section or key setting from an application's entry in the Windows registry.
    ''' </summary>
    ''' <param name="AppName">Required. String expression containing the name of the application or project to which the section or key setting applies.</param>
    ''' <param name="Section">Required. String expression containing the name of the section from which the key setting is being deleted. If only AppName and Section are provided, the specified section is deleted along with all related key settings.</param>
    ''' <param name="Key">Optional. String expression containing the name of the key setting being deleted.</param>
    ''' <remarks>
    ''' If all arguments are provided, the specified setting is deleted. A run-time error occurs if you attempt to use DeleteSetting on a nonexistent section or key setting.
    ''' <para>DeleteSetting requires that a user be logged on since it operates under the HKEY_LOCAL_USER registry key, which is not active until a user logs on interactively.</para>
    ''' <para>Registry settings that are to be accessed from a non-interactive process (such as mtx.exe) should be stored under either the HKEY_LOCAL_MACHINE\Software\ or the HKEY_USER\DEFAULT\Software registry keys.</para>
    ''' </remarks>
    ''' <example>The following example first uses the <see cref="M:OpenNETCF.VisualBasic.Interaction.SaveSetting"/> procedure to make entries in the Windows registry for the MyApp application, and then uses the <b>DeleteSetting</b> function to remove them.
    ''' Because no Key argument is specified, the whole Startup section is deleted, including the section name and all of its keys.
    ''' <code>[VB]
    ''' ' Place some settings in the registry.
    ''' SaveSetting("MyApp", "Startup", "Top", "75")
    ''' SaveSetting("MyApp","Startup", "Left", "50")
    ''' 
    ''' ' Remove section and all its settings from registry.
    ''' DeleteSetting ("MyApp", "Startup")
    ''' ' Remove MyApp from the registry.
    ''' DeleteSetting ("MyApp")
    ''' </code></example>
    Public Sub DeleteSetting(ByVal AppName As String, Optional ByVal Section As String = Nothing, Optional ByVal Key As String = Nothing)
        
        ' I had some trouble with the deletesubkey call so I am setting it blank
        'SaveSetting(strAppName, strSection, strKey, String.Empty)

        Dim rk As RegistryKey = Nothing

        If (AppName <> String.Empty) AndAlso (Section <> String.Empty) Then

            Try

                If (Key <> String.Empty) Then
                    ' set up the registry key
                    rk = Registry.CurrentUser.OpenSubKey(c_strKeyName & AppName & "\" & Section)

                    'delete the value
                    rk.DeleteValue(Key)
                Else
                    ' set up the registry key
                    rk = Registry.CurrentUser.OpenSubKey(c_strKeyName & AppName)

                    'delete the entire section
                    rk.DeleteSubKey(Section)
                End If

            Catch
                Err.Raise(5, Nothing, "Registry Key could not be deleted")
            Finally

                ' close the registry key
                If Not rk Is Nothing Then
                    rk.Close()
                End If

            End Try

        Else
            Err.Raise(5, Nothing, "Section, AppName, or Key setting does not exist")
        End If
    End Sub

End Module
